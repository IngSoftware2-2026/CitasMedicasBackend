using CitasMedicas.BusinessLogic.Configuration;
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
        private readonly AuthRepository _authRepository;

        public AccesoService(AuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public ServiceResult Login(LoginDTO login)
        {
            if (login == null)
                return new ServiceResult().BadRequest("Los datos de login son requeridos.");

            if (string.IsNullOrWhiteSpace(login.NombreUsuario))
                return new ServiceResult().BadRequest("El nombre de usuario es requerido.");

            if (string.IsNullOrWhiteSpace(login.Clave))
                return new ServiceResult().BadRequest("La clave es requerida.");

            try
            {
                var usuario = _authRepository.Login(login.NombreUsuario, login.Clave);

                if (usuario == null)
                    return new ServiceResult().Unauthorized("Credenciales incorrectas.");

                if (!usuario.Activo)
                    return new ServiceResult().Unauthorized("El usuario está inactivo.");

                var token = GenerarToken(usuario);

                return new ServiceResult().Ok(new
                {
                    token,
                    usuario.UsuarioId,
                    usuario.NombreUsuario,
                    usuario.Correo,
                    usuario.RolId,
                    usuario.CodigoRol,
                    usuario.NombreRol
                });
            }
            catch (Exception ex)
            {
                return new ServiceResult().Error($"Error inesperado al iniciar sesión: {ex.Message}");
            }
        }

        private string GenerarToken(UsuarioDTO usuario)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()),
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Email, usuario.Correo ?? ""),
                new Claim(ClaimTypes.Role, usuario.CodigoRol ?? ""),
                new Claim("RolId", usuario.RolId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: JwtSettings.Issuer,
                audience: JwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(JwtSettings.ExpirationHours),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
