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



        #region Roles
        public static string SP_Roles_Listar = "Accesos.SP_Roles_Listar";
        public static string SP_Roles_Insertar = "Accesos.SP_Roles_Insertar";
        public static string SP_Roles_Editar = "Accesos.SP_Roles_Editar";
        public static string SP_Roles_Eliminar = "Accesos.SP_Roles_Eliminar";
        #endregion

        #region Usuarios
        public static string SP_Usuarios_Listar = "Accesos.SP_Usuarios_Listar";
        public static string SP_Usuarios_Insertar = "Accesos.SP_Usuarios_Insertar";
        public static string SP_Usuarios_Editar = "Accesos.SP_Usuarios_Editar";
        public static string SP_Usuarios_Eliminar = "Accesos.SP_Usuarios_Eliminar";


        public static string SP_Usuarios_Login = "Accesos.SP_Usuarios_Login";
        #endregion



        #endregion

        #region Solicitudes
        public static string SP_SolicitudesPublicas_Insertar = "Clinica.SP_SolicitudesPublicas_Insertar";
        public static string SP_SolicitudesCita_Insertar = "Clinica.SP_SolicitudesCita_Insertar";
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
