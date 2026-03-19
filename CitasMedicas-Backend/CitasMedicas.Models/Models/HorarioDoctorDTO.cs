using System;

namespace CitasMedicas.Models.Models
{
    public class HorarioDoctorDTO
    {
        // El ID único del horario (Para cuando queramos editar o eliminar)
        public int HorarioId { get; set; }

        // El ID del doctor al que le pertenece este horario
        public int DoctorId { get; set; }

        // El día de la semana (1 = Lunes, 7 = Domingo)
        public int DiaSemana { get; set; }

        // La hora a la que empieza a atender (TimeSpan equivale a TIME en SQL)
        public TimeSpan HoraInicio { get; set; }

        // La hora a la que termina de atender
        public TimeSpan HoraFin { get; set; }

        // Si el horario está activo (1) o fue eliminado (0) (bool equivale a BIT en SQL)
        public bool Activo { get; set; }
    }
}