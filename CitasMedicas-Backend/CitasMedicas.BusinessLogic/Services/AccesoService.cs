using CitasMedicas.BusinessLogic.Configuration;
using CitasMedicas.DataAccess;
using CitasMedicas.DataAccess.Repositories.Accesos;
using CitasMedicas.DataAccess.Repositories.Clinica;
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
        private readonly PacientesRepository _pacientesRepository;

        public AccesoService(IAuthRepository authRepository, IUserRepository userRepository, PacientesRepository pacientesRepository)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _pacientesRepository = pacientesRepository;
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
                new Claim(ClaimTypes.Role, rol?.CodigoRol ?? ""),
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

        public ServiceResult UsuariosObtenerPorId(int usuarioId)
        {
            if (usuarioId <= 0)
                return new ServiceResult().BadRequest("El id del usuario es requerido");

            try
            {
                var usuario = _userRepository.ObtenerPorId(usuarioId);
                if (usuario == null)
                    return new ServiceResult().NotFound("Usuario no encontrado");

                return new ServiceResult().Ok("Usuario encontrado", usuario);
            }
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

        #region Registro Paciente
        public ServiceResult RegistroPaciente(RegistroPacienteDTO registro)
        {
            if (registro == null)
                return new ServiceResult().BadRequest("Los datos de registro son requeridos");

            if (string.IsNullOrWhiteSpace(registro.NombreUsuario))
                return new ServiceResult().BadRequest("El nombre de usuario es requerido");

            if (string.IsNullOrWhiteSpace(registro.Clave) || registro.Clave.Length < 6)
                return new ServiceResult().BadRequest("La contraseña debe tener al menos 6 caracteres");

            if (string.IsNullOrWhiteSpace(registro.Correo))
                return new ServiceResult().BadRequest("El correo es requerido");

            if (string.IsNullOrWhiteSpace(registro.Nombres))
                return new ServiceResult().BadRequest("Los nombres son requeridos");

            if (string.IsNullOrWhiteSpace(registro.Apellidos))
                return new ServiceResult().BadRequest("Los apellidos son requeridos");

            if (string.IsNullOrWhiteSpace(registro.NumeroIdentidad))
                return new ServiceResult().BadRequest("El número de identidad es requerido");

            return Execute(() =>
            {
                // Obtener el rolId de PACIENTE
                var roles = _authRepository.Listar();
                var rolPaciente = roles.FirstOrDefault(r => r.CodigoRol == "PACIENTE");
                if (rolPaciente == null)
                    return new ServiceResult().Error("No se encontró el rol de paciente en el sistema");

                // 1. Crear usuario inactivo con rol PACIENTE
                var usuario = new UsuariosDTO
                {
                    NombreUsuario = registro.NombreUsuario.Trim(),
                    Correo = registro.Correo.Trim(),
                    Telefono = registro.Telefono?.Trim(),
                    Clave = registro.Clave,
                    RolId = rolPaciente.RolId,
                    Activo = false // Necesita aprobación del admin
                };

                var resultadoUsuario = _userRepository.Insertar(usuario);
                if (resultadoUsuario.CodeStatus != 1)
                    return new ServiceResult().Conflict(resultadoUsuario.MessageStatus, resultadoUsuario);

                // 2. Obtener el usuario recién creado para tener su ID
                var usuarios = _userRepository.Listar();
                var usuarioCreado = usuarios
                    .Where(u => u.NombreUsuario == registro.NombreUsuario.Trim())
                    .OrderByDescending(u => u.UsuarioId)
                    .FirstOrDefault();

                if (usuarioCreado == null)
                    return new ServiceResult().Error("El usuario fue creado pero no se pudo recuperar");

                // 3. Crear registro de paciente vinculado al usuario
                var paciente = new PacientesDTO
                {
                    UsuarioId = usuarioCreado.UsuarioId,
                    Nombres = registro.Nombres.Trim(),
                    Apellidos = registro.Apellidos.Trim(),
                    Telefono = registro.Telefono?.Trim() ?? "",
                    Correo = registro.Correo.Trim(),
                    NumeroIdentidad = registro.NumeroIdentidad.Trim(),
                    FechaNacimiento = registro.FechaNacimiento,
                    Activo = true
                };

                var resultadoPaciente = _pacientesRepository.PacienteInsertar(paciente);
                if (resultadoPaciente.CodeStatus != 1)
                    return new ServiceResult().Conflict(resultadoPaciente.MessageStatus, resultadoPaciente);

                return new ServiceResult().Ok(
                    "Registro exitoso. Tu cuenta será activada por un administrador.",
                    new { UsuarioId = usuarioCreado.UsuarioId, NombreUsuario = usuarioCreado.NombreUsuario }
                );
            });
        }
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