using CitasMedicas.BusinessLogic.Services;
using CitasMedicas.DataAccess;
using CitasMedicas.DataAccess.Repositories.Accesos;
using CitasMedicas.DataAccess.Repositories.Catalogos;
using CitasMedicas.DataAccess.Repositories.Clinica;
using CitasMedicas.DataAccess.Repositories.Consultas;
using Microsoft.Extensions.DependencyInjection;

namespace CitasMedicas.BusinessLogic
{
    public static class ServiceConfiguration
    {
        public static void DataAccess(this IServiceCollection services, string connectionString)
        {
            // Configura la cadena de conexión del context
            CitasMedicasContext.BuildConnectionString(connectionString);

            // Repositorios - INTERFACES PARA TESTS
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEspecialidadesRepository, EspecialidadesRepository>();
            
            // Repositorios de dev
            services.AddScoped<SolicitudesRepository>();
            services.AddScoped<PropuestasReprogramacionRepository>();
            services.AddScoped<CitasRepository>();
            services.AddScoped<ConsultasRepository>();
            services.AddScoped<DoctoresRepository>();
            services.AddScoped<PacientesRepository>();
            services.AddScoped<EstadosRepository>();
            services.AddScoped<HorariosDoctorRepository>();
        }

        public static void BusinessLogic(this IServiceCollection services)
        {
            services.AddScoped<ClinicaService>();
            services.AddScoped<CatalogoService>();
            services.AddScoped<AccesoService>();
            services.AddScoped<ConsultasService>();
        }
    }
}
