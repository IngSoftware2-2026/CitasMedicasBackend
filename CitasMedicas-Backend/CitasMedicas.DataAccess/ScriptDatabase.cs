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
        public static string SP_Especialidades_Listar = "Catalogos.SP_Especialidades_Listar";
        public static string SP_Especialidades_Insertar = "Catalogos.SP_Especialidades_Insertar";
        public static string SP_Especialidades_Editar = "Catalogos.SP_Especialidades_Editar";
        public static string SP_Especialidades_Eliminar = "Catalogos.SP_Especialidades_Eliminar";
        #endregion

        #region Solicitudes
        public static string SP_SolicitudesPublicas_Insertar = "Clinica.SP_SolicitudesPublicas_Insertar";
        public static string SP_SolicitudesCita_Insertar = "Clinica.SP_SolicitudesCita_Insertar";
        #endregion

        #region Propuestas Reprogramacion
        public static string SP_CrearPropuestaReprogramacion = "Clinica.SP_CrearPropuestaReprogramacion";
        public static string SP_AceptarPropuestaReprogramacion = "Clinica.SP_AceptarPropuestaReprogramacion";
        public static string SP_RechazarPropuestaReprogramacion = "Clinica.SP_RechazarPropuestaReprogramacion";
        #endregion

        #region Citas
        public static string SP_Citas_Insertar = "Clinica.SP_Citas_Insertar";
        public static string SP_Citas_ObtenerPorFiltro = "Clinica.SP_Citas_ObtenerPorFiltro";
        public static string SP_Citas_ObtenerPorId = "Clinica.SP_Citas_ObtenerPorId";
        public static string SP_Citas_CambiarEstado = "Clinica.SP_Citas_CambiarEstado";
        public static string SP_Citas_ActualizarSala = "Clinica.SP_Citas_ActualizarSala";
        #endregion
        
        #region Salas

        public static string SP_Sala_Listar = "Catalogos.SP_Salas_Listar";
        public static string SP_Sala_Crear = "Catalogos.SP_Salas_Crear";
        public static string SP_Sala_Editar = "Catalogos.SP_Salas_Editar";
        public static string SP_Sala_CambiarEstado = "Catalogos.SP_Salas_CambiarEstado";

        #endregion

        #region Estados

        public static string SP_EstadosCita_Listar = "Catalogos.SP_EstadosCita_Listar";
        public static string SP_EstadosSolicitud_Listar = "Catalogos.SP_EstadosSolicitud_Listar";

        #endregion
    }
}
