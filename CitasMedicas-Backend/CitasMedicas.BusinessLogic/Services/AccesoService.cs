using CitasMedicas.BusinessLogic.Configuration;
using CitasMedicas.DataAccess;
using CitasMedicas.DataAccess.Repositories.Accesos;
using CitasMedicas.Models.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CitasMedicas.BusinessLogic.Services
{
    public class AccesoService
    {
        private readonly AuthRepository _authRepository;
        private readonly UserRepository _userRepository;

        public AccesoService(AuthRepository authRepository, UserRepository userRepository)
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

            try
            {
                var usuario = _authRepository.ValidarUsuario(loginRequest.NombreUsuario, loginRequest.Clave);

                if (usuario == null)
                    return new ServiceResult().Unauthorized("Usuario o contraseña incorrectos");

                if (!usuario.Activo)
                    return new ServiceResult().Unauthorized("Usuario inactivo");

                var token = GenerateJwtToken(usuario);

                var roles = _authRepository.Listar();
                var rol = roles.FirstOrDefault(r => r.RolId == usuario.RolId);

                var loginResponse = new LoginResponse
                {
                    UsuarioId = usuario.UsuarioId,
                    NombreUsuario = usuario.NombreUsuario,
                    Correo = usuario.Correo,
                    Token = token,
                    Rol = rol
                };

                return new ServiceResult().Ok("Login exitoso", loginResponse);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado durante el login: {ex.Message}");
            }
        }

        private string GenerateJwtToken(UsuariosDTO usuario)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = _authRepository.Listar();
            var rol = roles.FirstOrDefault(r => r.RolId == usuario.RolId);

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
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion

        #region Método genérico de mapeo
        private ServiceResult MapRequestStatusToServiceResult(RequestStatus response)
        {
            var result = new ServiceResult();

            if (response == null)
                return result.Error("La operación no devolvió resultados.");

            switch (response.CodeStatus)
            {
                case 1:
                    return result.Ok(response.MessageStatus, response);

                case -1:
                    return result.Conflict(response.MessageStatus, response);

                case -2:
                    return result.Conflict(response.MessageStatus, response);

                case 0:
                    return result.Error(response.MessageStatus);

                default:
                    return result.Error("Ocurrió un error desconocido.");
            }
        }
        #endregion

        #region Roles
        public ServiceResult ListarRoles()
        {
            var result = new ServiceResult();
            try
            {
                var response = _authRepository.Listar();
                return result.Ok(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Error inesperado al listar roles: {ex.Message}");
            }
        }

        public ServiceResult RolesInsertar(RolDTO rol)
        {
            if (rol == null)
                return new ServiceResult().BadRequest("Los datos del rol son requeridos");

            try
            {
                var response = _authRepository.Insertar(rol);
                return MapRequestStatusToServiceResult(response);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado durante la inserción: {ex.Message}");
            }
        }

        public ServiceResult RolesEditar(RolDTO rol)
        {
            if (rol == null)
                return new ServiceResult().BadRequest("Los datos del rol son requeridos");

            if (rol.RolId <= 0)
                return new ServiceResult().BadRequest("El id del rol es requerido");

            try
            {
                var response = _authRepository.Editar(rol);
                return MapRequestStatusToServiceResult(response);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado durante la edición: {ex.Message}");
            }
        }

        public ServiceResult RolesEliminar(int rolId)
        {
            if (rolId <= 0)
                return new ServiceResult().BadRequest("El id del rol es requerido");

            try
            {
                var response = _authRepository.Eliminar(rolId);
                return MapRequestStatusToServiceResult(response);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado durante la eliminación: {ex.Message}");
            }
        }
        #endregion

        #region Usuarios
        public ServiceResult ListarUsuarios()
        {
            var result = new ServiceResult();
            try
            {
                var response = _userRepository.Listar();
                return result.Ok(response);
            }
            catch (Exception ex)
            {
                return result.Error($"Error inesperado al listar usuarios: {ex.Message}");
            }
        }

        public ServiceResult UsuariosInsertar(UsuariosDTO usuario)
        {
            if (usuario == null)
                return new ServiceResult().BadRequest("Los datos del usuario son requeridos");

            try
            {
                var response = _userRepository.Insertar(usuario);
                return MapRequestStatusToServiceResult(response);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado durante la inserción: {ex.Message}");
            }
        }

        public ServiceResult UsuariosEditar(UsuariosDTO usuario)
        {
            if (usuario == null)
                return new ServiceResult().BadRequest("Los datos del usuario son requeridos");

            if (usuario.UsuarioId <= 0)
                return new ServiceResult().BadRequest("El id del usuario es requerido");

            try
            {
                var response = _userRepository.Editar(usuario);
                return MapRequestStatusToServiceResult(response);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado durante la edición: {ex.Message}");
            }
        }

        public ServiceResult UsuariosEliminar(int usuarioId)
        {
            if (usuarioId <= 0)
                return new ServiceResult().BadRequest("El id del usuario es requerido");

            try
            {
                var response = _userRepository.Eliminar(usuarioId);
                return MapRequestStatusToServiceResult(response);
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado durante la eliminación: {ex.Message}");
            }
        }
        #endregion
    }
}
