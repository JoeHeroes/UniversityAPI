
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using System.Reflection;
using System.Text;
using UniAPI;
using UniAPI.Authorization;
using UniAPI.Authorization.Policy;
using UniAPI.Entites;
using UniAPI.Middleware;
using UniAPI.Models;
using UniAPI.Models.Validators;
using UniAPI.Seeder;
using UniAPI.Services;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder();

    //Nlog
    builder.Services.AddControllersWithViews();
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    //services.AddTransient<>();           
    var authenticationSettings = new AuthenticationSettings();

    builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);

    //Autoorization
    builder.Services.AddSingleton(authenticationSettings);

    builder.Services.AddAuthentication(option =>
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

    builder.Services.AddAuthorization(options => {
        options.AddPolicy("HasNationality", builder => builder.RequireClaim("Nationality", "German", "Polish"));
        options.AddPolicy("Atleast20", builder => builder.AddRequirements(new MinimumAgeRequirment(20)));
        options.AddPolicy("CreatedAtleast2Univeristy", builder => builder.AddRequirements(new CreateMultipleUniversityRequirment(2)));

    });

    //RequirmentHandler
    builder.Services.AddScoped<IAuthorizationHandler, MinimumAgeRequirmentHandler>();
    builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();
    builder.Services.AddScoped<IAuthorizationHandler, CreateMultipleUniversityRequirmentHandler>();

    //Validator
    builder.Services.AddControllers().AddFluentValidation();

    //Sedder
    builder.Services.AddScoped<UniversitySeeder>();

    //Mapper
    builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

    //Interface
    builder.Services.AddScoped<IAccountServices, AccountServices>();
    builder.Services.AddScoped<IUniversityService, UniversityService>();
    builder.Services.AddScoped<IStudentService, StudentService>();
    builder.Services.AddScoped<IDepartmentService, DepartmentService>();
    builder.Services.AddScoped<IUserContextService, UserContextService>();

    //Middleware
    builder.Services.AddScoped<ErrorHandlingMiddleware>();
    builder.Services.AddScoped<RequestTimeMiddleware>();

    //Hasser
    builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

    //Validetor
    builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
    builder.Services.AddScoped<IValidator<UniversityQuery>, UniversityQueryValidator>();

    //ContextAccessor
    builder.Services.AddHttpContextAccessor();

    //Swagger
    builder.Services.AddSwaggerGen();

    //Cors
    builder.Services.AddCors(
        options => options.AddPolicy("FrontEndClient", policyBuilder =>
        policyBuilder.AllowAnyMethod()
        .AllowAnyHeader()
        .WithOrigins(builder.Configuration["AllowedOrigins"])
        ));

    //DbContext
    builder.Services.AddDbContext<UniversityDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("RestaurantDbConection")));


    var app = builder.Build();

    var scope = app.Services.CreateScope();

    var seeder = scope.ServiceProvider.GetRequiredService<UniversitySeeder>();

    app.UseResponseCaching();

    app.UseStaticFiles();

    app.UseCors("FrontEndClient");

    seeder.Seed();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseMiddleware<RequestTimeMiddleware>();

    app.UseAuthentication();

    app.UseHttpsRedirection();

    app.UseSwagger();

    app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "v1.0"));

    app.UseRouting();

    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });

    app.Run();
}
catch (Exception exception)
{
    
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}