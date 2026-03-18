using CitasMedicas.BusinessLogic.Configuration;
using CitasMedicas.DataAccess;
using CitasMedicas.DataAccess.Repositories.Accesos;
using CitasMedicas.Models.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CitasMedicas.BusinessLogic.Services
{
    public class AccesoService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;

        public AccesoService(IAuthRepository authRepository, IUserRepository userRepository)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
        }

        #region Login
        public ServiceResult Login(LoginRequest loginRequest)
        {
            if (loginRequest == null)
                return new ServiceResult().BadRequest("Las credenciales son requeridas");

            if (string.IsNullOrEmpty(loginRequest.NombreUsuario) || string.IsNullOrEmpty(loginRequest.Clave))
                return new ServiceResult().BadRequest("Usuario y contraseña son requeridos");

            return Execute(() =>
            {
                var usuario = _authRepository.ValidarUsuario(loginRequest.NombreUsuario, loginRequest.Clave);
                Console.WriteLine($"[LOGIN] Usuario encontrado: {usuario?.NombreUsuario ?? "NULL"}");

                if (usuario == null)
                    return new ServiceResult().Unauthorized("Usuario o contraseña incorrectos");

                if (!usuario.Activo ?? false)
                    return new ServiceResult().Unauthorized("Usuario inactivo");

                var rol = _authRepository.Listar().FirstOrDefault(r => r.RolId == (usuario.RolId ?? 0));

                return new ServiceResult().Ok("Login exitoso", new LoginResponse
                {
                    UsuarioId = usuario.UsuarioId,
                    NombreUsuario = usuario.NombreUsuario,
                    Correo = usuario.Correo,
                    Token = GenerateJwtToken(usuario, rol),
                    Rol = rol
                });
            });
        }

        public ServiceResult LoginDebug(LoginRequest loginRequest)
        {
            if (loginRequest == null)
                return new ServiceResult().BadRequest("Las credenciales son requeridas");

            return Execute(() =>
            {
                var usuario = _authRepository.ValidarUsuario(loginRequest.NombreUsuario, loginRequest.Clave);
                return new ServiceResult().Ok("Debug", usuario);
            });
        }

        private string GenerateJwtToken(UsuariosDTO usuario, RolDTO rol)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.UsuarioId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, usuario.NombreUsuario),
                new Claim(ClaimTypes.Role, rol?.NombreRol ?? ""),
                new Claim("RolId", usuario.RolId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: JwtSettings.Issuer,
                audience: JwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(JwtSettings.ExpirationHours),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.Key)),
                    SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion

        #region Roles
        public ServiceResult ListarRoles()
        {
            try { return new ServiceResult().Ok(_authRepository.Listar()); }
            catch (Exception ex) { return new ServiceResult().Error($"Error: {ex.Message}"); }
        }

        public ServiceResult RolesInsertar(RolDTO rol)
            => ValidateAndExecute(rol, r => r?.RolId == 0, () => _authRepository.Insertar(rol));

        public ServiceResult RolesEditar(RolDTO rol)
            => ValidateAndExecute(rol, r => r?.RolId > 0, () => _authRepository.Editar(rol));

        public ServiceResult RolesEliminar(int rolId)
            => rolId > 0 ? MapRequestStatusToServiceResult(_authRepository.Eliminar(rolId))
                         : new ServiceResult().BadRequest("El id del rol es requerido");
        #endregion

        #region Usuarios
        public ServiceResult ListarUsuarios()
        {
            try { return new ServiceResult().Ok(_userRepository.Listar()); }
            catch (Exception ex) { return new ServiceResult().Error($"Error: {ex.Message}"); }
        }

        public ServiceResult UsuariosInsertar(UsuariosDTO usuario)
            => ValidateAndExecute(usuario, u => true, () => _userRepository.Insertar(usuario));

        public ServiceResult UsuariosEditar(UsuarioEditarDTO usuario)
        {
            var usuarioDto = new UsuariosDTO
            {
                UsuarioId = usuario.UsuarioId,
                NombreUsuario = usuario.NombreUsuario,
                Correo = usuario.Correo,
                Telefono = usuario.Telefono,
                RolId = usuario.RolId,
                Activo = usuario.Activo
            };
            return ValidateAndExecute(usuarioDto, u => u?.UsuarioId > 0, () => _userRepository.Editar(usuarioDto));
        }

        public ServiceResult UsuariosEliminar(int usuarioId)
            => usuarioId > 0 ? MapRequestStatusToServiceResult(_userRepository.Eliminar(usuarioId))
                            : new ServiceResult().BadRequest("El id del usuario es requerido");
        #endregion

        #region Helpers
        private ServiceResult Execute(Func<ServiceResult> action)
        {
            try { return action(); }
            catch (Exception ex) { return new ServiceResult().Error($"Error: {ex.Message}"); }
        }

        private ServiceResult ValidateAndExecute<T>(T obj, Func<T, bool> validate, Func<RequestStatus> action)
        {
            if (obj == null) return new ServiceResult().BadRequest("Los datos son requeridos");
            if (!validate(obj)) return new ServiceResult().BadRequest("El id es requerido");

            try { return MapRequestStatusToServiceResult(action()); }
            catch (Exception ex) { return new ServiceResult().Error($"Error: {ex.Message}"); }
        }

        private ServiceResult MapRequestStatusToServiceResult(RequestStatus response)
        {
            if (response == null) return new ServiceResult().Error("La operación no devolvió resultados.");

            return response.CodeStatus switch
            {
                1 => new ServiceResult().Ok(response.MessageStatus, response),
                -1 or -2 => new ServiceResult().Conflict(response.MessageStatus, response),
                0 => new ServiceResult().Error(response.MessageStatus),
                _ => new ServiceResult().Error("Error desconocido.")
            };
        }
        #endregion
    }
}