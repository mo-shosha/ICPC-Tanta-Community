using Core.Entities.Identity;
using Core.IRepositories;
using Core.IServices;
using ICPC_Tanta_Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Repository.Data;
using Repository.Repositories;

namespace ICPC_Tanta_Web.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            //  تسجيل AppDbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            //  إعداد Identity
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            // تسجيل الـ Services و Repositories
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IFileProcessingService, FileProcessingService>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IMemeberServices, MemeberServices>();
            services.AddScoped<ITrainingLevelServices, TrainingLevelServices>();
            services.AddScoped<ITrainingContentServices, TrainingContentServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ITokenServices, TokenServices>();
            services.AddScoped<ICodeforcesService, CodeforcesService>();
            services.AddScoped<IInfoService, InfoService>();
            services.AddHttpClient<ICodeforcesService, CodeforcesService>();

            services.AddSignalR();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllers();
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", policy =>
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
                          //.AllowCredentials());
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ICPC API", Version = "v1" });
            });
        }
    }
}
