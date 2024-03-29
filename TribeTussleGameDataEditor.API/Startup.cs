using TribeTussleGameDataEditor.API.Controllers.Filters;
using TribeTussleGameDataEditor.API.Controllers.Util;
using TribeTussleGameDataEditor.API.DAL;
using TribeTussleGameDataEditor.API.DAL.Models;
using TribeTussleGameDataEditor.API.DAL.Repositories;
using TribeTussleGameDataEditor.API.Util;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace TribeTussleGameDataEditor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions<UserRequestDTO>()
                .Bind(Configuration.GetSection("AdminUser"))
                .ValidateDataAnnotations();
            services.AddOptions<SmtpConfig>()
                .Bind(Configuration.GetSection("Smtp"))
                .ValidateDataAnnotations();

            LoadDbContexts(services);

            services.AddControllers(options =>
            {
                options.UseGeneralRoutePrefix("api/v{version:apiVersion}");
                options.Filters.Add(typeof(AuthenticationFilter));
                options.Filters.Add(typeof(InvalidModelStateFilter));
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddApiVersioning(options => {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IVerificationTokenRepository, VerificationTokenRepository>();
            services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();
            services.AddScoped<IGameDataRepository, GameDataRepository>();
            services.AddScoped<IResponseObjectFactory, ResponseObjectFactory>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IDALInit, DALInit>();
            services.AddSingleton(AutoMapperConfig.GetConfig().CreateMapper());
            services.AddSingleton<ISmtpClientFactory, SmtpClientFactory>();
            services.AddSingleton<IMailer, Mailer>();
            services.AddSingleton<IFileReader, FileReader>();
            services.AddSingleton<IFileWriter, FileWriter>();
            services.AddSingleton<IYAMLReader<GameDataData>, YAMLReader<GameDataData>>();
            services.AddSingleton<IYAMLWriter<GameDataData>, YAMLWriter<GameDataData>>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = Program.AppName, Version = "v1" });
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromDays(14);
                    options.SlidingExpiration = true;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{Program.AppName} v1"));
            }

            app.UseHttpsRedirection();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("/index.html");
            });

            if (env.IsProduction())
                MigrateDb(app.ApplicationServices);
            InitDb(app.ApplicationServices);
        }

        private void LoadDbContexts(IServiceCollection services)
        {
            DbType dbType = (DbType)Enum.Parse(typeof(DbType), Configuration["Database:Type"]);
            Action<DbContextOptionsBuilder> dbContextOptionsBuilder = GetDbContextOptionsBuilder(dbType);
            services.AddDbContext<ApplicationDbContext>(dbContextOptionsBuilder);
        }

        private Action<DbContextOptionsBuilder> GetDbContextOptionsBuilder(DbType dbType)
        {
            void Action(DbContextOptionsBuilder opt)
            {
                switch (dbType)
                {
                    case DbType.SQLite:
                        opt.UseSqlite(Configuration.GetConnectionString("SQLite"));
                        break;
                    case DbType.SQLServer:
                        opt.UseSqlServer(Configuration.GetConnectionString("SQLServer"));
                        break;
                    default:
                        opt.UseInMemoryDatabase(Program.AppName);
                        break;
                }
            }
            return Action;
        }

        private void MigrateDb(IServiceProvider serviceProvider)
        {
            using var serviceScope = serviceProvider.CreateScope();
            using var dbContext = (ApplicationDbContext)serviceScope.ServiceProvider.GetService(typeof(ApplicationDbContext));
            dbContext.Database.Migrate();
        }

        private void InitDb(IServiceProvider serviceProvider)
        {
            using var serviceScope = serviceProvider.CreateScope();
            var DALInit = (IDALInit)serviceScope.ServiceProvider.GetService(typeof(IDALInit));
            DALInit.Init();
        }
    }

    public enum DbType { InMemory, SQLite, SQLServer }
}
