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
        #endregion



        #endregion
    }
}
