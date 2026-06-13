using Application;
using CoreLib.Api.Controllers;
using CoreLib.Audit;
using CoreLib.Database.Migrations;
using CoreLib.User;
using CoreLib.Middlewares;
using IdentityLib;
using Infrastructure;
using IdentityLib.Jwt.RsaKeys.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

namespace Api;

public sealed class Startup(IWebHostEnvironment env, IConfiguration configuration)
{
    /// <summary>
    /// Конфигурация приложения
    /// </summary>
    private IConfiguration Configuration { get; } = configuration;
    
    /// <summary>
    /// Окружение приложения
    /// </summary>
    private IWebHostEnvironment Environment { get; } = env;

    /// <summary>
    /// Регистрация сервисов
    /// </summary>
    public void ConfigureServices(IServiceCollection services)
    {
        var connectionString = Configuration.GetConnectionString("DefaultConnection");
        
        services.AddControllers();
        services.AddApplication();
        services.AddInfrastructure(connectionString!);
        
        services.AddCoreControllers();
        services.AddCoreUser();
        services.AddCoreAudit();
        
        services.AddIdentityLib(Configuration);
        
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = Configuration["Jwt:AccessTokenSettings:Issuer"],
                    ValidAudience = Configuration["Jwt:AccessTokenSettings:Audience"],

                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IRsaKeyProvider>((options, rsaKeyProvider) =>
            {
                options.TokenValidationParameters.IssuerSigningKey = rsaKeyProvider.ValidationKey;
            });
    
        services.AddAuthorization();
        
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "IdentityApi",
                Version = "v1"
            });
            options.AddServer(new OpenApiServer
            {
                Url = "/identity"
            });
            options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Type = SecuritySchemeType.Http,
                In = ParameterLocation.Header,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
                Description = "Введите JWT токен в формате: {token}"
            });
            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("bearer", document)] = []
            });
        });
    }

    /// <summary>
    /// Конфигурация middleware пайплайна
    /// </summary>
    public void Configure(IApplicationBuilder app)
    {
        if (Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "IdentityApi v1");
            });
        }
        
        app.MigrateDatabase();

        app.UseMiddleware<ErrorHandlingMiddleware>();
        
        app.UseRouting();
        
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}