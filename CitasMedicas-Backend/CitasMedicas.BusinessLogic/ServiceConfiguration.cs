using CitasMedicas.BusinessLogic.Services;
using CitasMedicas.DataAccess;
using CitasMedicas.DataAccess.Repositories.Accesos;
using CitasMedicas.DataAccess.Repositories.Catalogos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitasMedicas.BusinessLogic
{
    public static class ServiceConfiguration
    {
        public static void DataAccess(this IServiceCollection services, string connectionString)
        {
            // Configura la cadena de conexión del context
            CitasMedicasContext.BuildConnectionString(connectionString);

            // Repositorios
            
            services.AddScoped<AuthRepository>();
            services.AddScoped<UserRepository>();
            services.AddScoped<EspecialidadesRepository>();



        }

        public static void BusinessLogic(this IServiceCollection services)
        {
            services.AddScoped<ClinicaService>();
            services.AddScoped<CatalogoService>();
            services.AddScoped<AccesoService>();
        }
    }
}
