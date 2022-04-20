using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UniAPI.Authorization;
using UniAPI.Authorization.Policy;
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

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddTransient<>();           
            var authenticationSettings = new AuthenticationSettings();

            Configuration.GetSection("Authentication").Bind(authenticationSettings);

            //Autoorization
            services.AddSingleton(authenticationSettings);

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
                    ValidIssuer = authenticationSettings.JwtIssuer,
                    ValidAudience = authenticationSettings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
                };
            });

            services.AddAuthorization(options => { 
                options.AddPolicy("HasNationality", builder => builder.RequireClaim("Nationality","German","Polish"));
                options.AddPolicy("Atleast20", builder => builder.AddRequirements(new MinimumAgeRequirment(20)));
                options.AddPolicy("CreatedAtleast2Univeristy", builder => builder.AddRequirements(new CreateMultipleUniversityRequirment(2)));
            
            });

            //RequirmentHandler
            services.AddScoped<IAuthorizationHandler, MinimumAgeRequirmentHandler>();
            services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, CreateMultipleUniversityRequirmentHandler>();
            //Validator
            services.AddControllers().AddFluentValidation();
            //Sedder
            services.AddScoped<UniversitySeeder>();
            //Mapper
            services.AddAutoMapper(this.GetType().Assembly);
            //Interface
            services.AddScoped<IAccountServices, AccountServices>();
            services.AddScoped<IUniversityService, UniversityService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IUserContextService, UserContextService>();

            //Middleware
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<RequestTimeMiddleware>();

            //Hasser
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            //Validetor
            services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
            services.AddScoped<IValidator<UniversityQuery>, UniversityQueryValidator>();

            //ContextAccessor
            services.AddHttpContextAccessor();

            //Swagger
            services.AddSwaggerGen();

            //Cors
            services.AddCors(
                options => options.AddPolicy("FrontEndClient", builder =>
                builder.AllowAnyMethod()
                .AllowAnyHeader()
                .WithOrigins(Configuration["AllowedOrigins"])
                ));

            //DbContext
            services.AddDbContext<UniversityDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("RestaurantDbConection")));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UniversitySeeder seeder)
        {
            app.UseResponseCaching();

            app.UseStaticFiles();

            app.UseCors("FrontEndClient");

            seeder.Seed();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<RequestTimeMiddleware>();

            app.UseAuthentication();

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
