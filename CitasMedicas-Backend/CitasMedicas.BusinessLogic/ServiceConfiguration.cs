using CitasMedicas.BusinessLogic.Services;
using CitasMedicas.DataAccess;
using CitasMedicas.DataAccess.Repositories.Accesos;
using CitasMedicas.DataAccess.Repositories.Catalogos;
using CitasMedicas.DataAccess.Repositories.Clinica;
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
            // Configura la cadena de conexi�n del context
            CitasMedicasContext.BuildConnectionString(connectionString);

            // Repositorios
            services.AddScoped<AuthRepository>();
            services.AddScoped<EspecialidadesRepository>();
            services.AddScoped<PacientesRepository>();
            services.AddScoped<SolicitudesRepository>();
            services.AddScoped<PropuestasReprogramacionRepository>();
            services.AddScoped<CitasRepository>();
        }

        public static void BusinessLogic(this IServiceCollection services)
        {
            services.AddScoped<ClinicaService>();
            services.AddScoped<CatalogoService>();
            services.AddScoped<AccesoService>();
        }
    }
}
