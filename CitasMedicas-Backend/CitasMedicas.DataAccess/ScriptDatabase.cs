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

        #region Pacientes
        public static string SP_Pacientes_Listar = "Clinica.SP_Pacientes_Listar";
        public static string SP_Pacientes_ObtenerPorId = "Clinica.SP_Pacientes_ObtenerPorId";
        public static string SP_Pacientes_Insertar = "Clinica.SP_Pacientes_Insertar";
        public static string SP_Pacientes_Editar = "Clinica.SP_Pacientes_Editar";
        public static string SP_Pacientes_Eliminar = "Clinica.SP_Pacientes_Eliminar";
        #endregion

        #region Usuarios
        public static string SP_Usuarios_Login = "Accesos.SP_Usuarios_Login";
        #endregion
    }
}
