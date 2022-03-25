using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UniAPI.Authorization;
using UniAPI.Entites;
using UniAPI.Middleware;
using UniAPI.Models;
using UniAPI.Models.Validators;
using UniAPI.Seeder;
using UniAPI.Services;

namespace UniAPI
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
            //services.AddTransient<>();

            
            var authenticatioSettings = new AuthenticationSettings();

            Configuration.GetSection("Authentication").Bind(authenticatioSettings);

            //Autoorization

            services.AddSingleton(authenticatioSettings);

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultScheme = "Bearer";
                option.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authenticatioSettings.JwtIssuer,
                    ValidAudience = authenticatioSettings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticatioSettings.JwtKey)),
                };
            });


            
            //Validator
            services.AddControllers().AddFluentValidation();
            //DbContext
            services.AddDbContext<UniversityDbContext>();
            //Sedder
            services.AddScoped<UniversitySeeder>();
            //Mapper
            services.AddAutoMapper(this.GetType().Assembly);
            //Interface
            services.AddScoped<IAccountServices, AccountServices>();
            services.AddScoped<IUniversityService, UniversityService>();

            //Middleware
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<RequestTimeMiddleware>();
            //Swagger
            services.AddSwaggerGen();

            //Hasser
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            //Validetor
            services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();

          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UniversitySeeder seeder)
        {
            seeder.Seed();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<RequestTimeMiddleware>();

            //app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "Uni API"));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
