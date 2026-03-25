using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;  
using System.Text;
using System.Threading.Tasks;

namespace CitasMedicas.DataAccess
{
    public class ScriptDatabase
    {
        #region Especialidades
        public static string SP_Especialidades_Listar   = "Catalogos.SP_Especialidades_Listar";
        public static string SP_Especialidades_Insertar = "Catalogos.SP_Especialidades_Insertar";
        public static string SP_Especialidades_Editar   = "Catalogos.SP_Especialidades_Editar";
        public static string SP_Especialidades_Eliminar = "Catalogos.SP_Especialidades_Eliminar";
        #endregion

        #region Roles
        public static string SP_Roles_Listar   = "Accesos.SP_Roles_Listar";
        public static string SP_Roles_Insertar = "Accesos.SP_Roles_Insertar";
        public static string SP_Roles_Editar   = "Accesos.SP_Roles_Editar";
        public static string SP_Roles_Eliminar = "Accesos.SP_Roles_Eliminar";
        #endregion

        #region Usuarios
        public static string SP_Usuarios_Listar   = "Accesos.SP_Usuarios_Listar";
        public static string SP_Usuarios_Insertar = "Accesos.SP_Usuarios_Insertar";
        public static string SP_Usuarios_Editar   = "Accesos.SP_Usuarios_Editar";
        public static string SP_Usuarios_Eliminar = "Accesos.SP_Usuarios_Eliminar";
        public static string SP_Usuarios_Login    = "Accesos.SP_Usuarios_Login";
        #endregion

        #region Solicitudes
        public static string SP_SolicitudesPublicas_Insertar = "Clinica.SP_SolicitudesPublicas_Insertar";
        public static string SP_SolicitudesCita_Insertar     = "Clinica.SP_SolicitudesCita_Insertar";
        #endregion

        #region Propuestas Reprogramacion
        public static string SP_CrearPropuestaReprogramacion    = "Clinica.SP_CrearPropuestaReprogramacion";
        public static string SP_AceptarPropuestaReprogramacion  = "Clinica.SP_AceptarPropuestaReprogramacion";
        public static string SP_RechazarPropuestaReprogramacion = "Clinica.SP_RechazarPropuestaReprogramacion";
        #endregion

        #region Citas
        public static string SP_Citas_Insertar         = "Clinica.SP_Citas_Insertar";
        public static string SP_Citas_ObtenerPorFiltro = "Clinica.SP_Citas_ObtenerPorFiltro";
        public static string SP_Citas_ObtenerPorId     = "Clinica.SP_Citas_ObtenerPorId";
        public static string SP_Citas_CambiarEstado    = "Clinica.SP_Citas_CambiarEstado";
        public static string SP_Citas_ActualizarSala   = "Clinica.SP_Citas_ActualizarSala";
        #endregion

        #region Salas
        public static string SP_Sala_Listar        = "Catalogos.SP_Sala_Listar";
        public static string SP_Sala_Crear         = "Catalogos.SP_Sala_Crear";
        public static string SP_Sala_Editar        = "Catalogos.SP_Sala_Editar";
        public static string SP_Sala_CambiarEstado = "Catalogos.SP_Sala_CambiarEstado";
        #endregion

        #region Consultas
        public static string SP_Consulta_Crear               = "Clinica.sp_Consulta_Crear";
        public static string SP_Consulta_Actualizar          = "Clinica.sp_ActualizarConsulta";
        public static string SP_Consulta_Obtener_PorCita     = "Clinica.sp_ObtenerConsultaPorCita";
        public static string SP_Consulta_Obtener_PorPaciente = "Clinica.sp_ObtenerConsultasPorPaciente";
        public static string SP_Todas_Consulta               = "Clinica.sp_ObtenerTodasConsultas";
        #endregion

        #region Doctores
        public static string SP_Doctores_Listar    = "Catalogos.SP_Doctores_Listar";
        public static string SP_Doctor_ObtenerPorId = "Clinica.sp_ObtenerDoctorPorId";
        #endregion

        #region Estados
        public static string SP_EstadosCita_Listar      = "Catalogos.SP_EstadosCita_Listar";
        public static string SP_EstadosSolicitud_Listar = "Catalogos.SP_EstadosSolicitud_Listar";
        #endregion

        #region Pacientes
        public static string SP_Pacientes_Listar       = "Clinica.SP_Pacientes_Listar";
        public static string SP_Pacientes_ObtenerPorId = "Clinica.SP_Pacientes_ObtenerPorId";
        public static string SP_Pacientes_Insertar     = "Clinica.SP_Pacientes_Insertar";
        public static string SP_Pacientes_Editar       = "Clinica.SP_Pacientes_Editar";
        public static string SP_Pacientes_Eliminar     = "Clinica.SP_Pacientes_Eliminar";
        #endregion

        #region Horarios Doctor
        public const string SP_HorariosDoctor_Obtener    = "Clinica.SP_HorariosDoctor_Obtener";
        public const string SP_HorariosDoctor_Crear      = "Clinica.SP_HorariosDoctor_Crear";
        public const string SP_HorariosDoctor_Actualizar = "Clinica.SP_HorariosDoctor_Actualizar";
        public const string SP_HorariosDoctor_Eliminar   = "Clinica.SP_HorariosDoctor_Eliminar";
        #endregion

        #region Invitaciones Paciente
        public static string SP_GenerarInvitacion              = "Clinica.SP_GenerarInvitacion";
        public static string SP_ValidarInvitacion              = "Clinica.SP_ValidarInvitacion";
        public static string SP_UsarInvitacion                 = "Clinica.SP_UsarInvitacion";
        public static string SP_ObtenerInvitacionesPorPaciente = "Clinica.SP_ObtenerInvitacionesPorPaciente";
        #endregion
    }
}