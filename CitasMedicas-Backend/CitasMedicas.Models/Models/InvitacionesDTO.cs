namespace CitasMedicas.Models.Models
{
    // Datos que retorna el SP al validar/listar invitaciones
    public class InvitacionDTO
    {
        public int CodeStatus { get; set; }
        public string? MessageStatus { get; set; }

        public int InvitacionId { get; set; }
        public int PacienteId { get; set; }
        public string? HashToken { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public string? Estado { get; set; }
        public DateTime? UsadoEn { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Datos del paciente — solo los retorna SP_ValidarInvitacion
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? Correo { get; set; }
        public string? NumeroIdentidad { get; set; }
    }

    // Body: POST /api/invitaciones  →  { "pacienteId": 5 }
    public class GenerarInvitacionRequest
    {
        public int PacienteId { get; set; }
    }

    // Body: POST /api/invitaciones/usar  →  { "token": "abc123..." }
    public class UsarInvitacionRequest
    {
        public string? Token { get; set; }
    }
}