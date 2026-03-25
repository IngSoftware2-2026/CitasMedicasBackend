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
            CitasMedicasContext.BuildConnectionString(connectionString);

            // Repositorios con interfaz (para tests)
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEspecialidadesRepository, EspecialidadesRepository>();

            // Repositorios concretos - Clinica
            services.AddScoped<SolicitudesRepository>();
            services.AddScoped<PropuestasReprogramacionRepository>();
            services.AddScoped<CitasRepository>();
            services.AddScoped<DoctoresRepository>();
            services.AddScoped<PacientesRepository>();
            services.AddScoped<EstadosRepository>();
            services.AddScoped<HorariosDoctorRepository>();
            services.AddScoped<InvitacionesRepository>(); // CLIN-08

            // Repositorios concretos - Consultas (namespace distinto)
            services.AddScoped<ConsultasRepository>();
        }

        public static void BusinessLogic(this IServiceCollection services)
        {
            services.AddScoped<ClinicaService>();
            services.AddScoped<CatalogoService>();
            services.AddScoped<AccesoService>();
            services.AddScoped<ConsultasService>();
            services.AddScoped<InvitacionesService>(); // CLIN-08
        }
    }
}