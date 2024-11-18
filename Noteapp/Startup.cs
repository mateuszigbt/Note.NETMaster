using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NoteApp;
using NoteApp.Models;
using NoteApp.Repositories;
using NoteApp.Services;
using Noteapp.Security;
using System;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Noteapp.Services;
using Noteapp.Util;

/// <summary>
/// Configures services and the application's request pipeline.
/// </summary>
public class Startup
{

    /// <summary>
    /// Initializes a new instance of the <see cref="Startup"/> class.
    /// </summary>
    /// <param name="configuration">The configuration settings.</param>
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// Gets the configuration settings.
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// Configures services for the application.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public void ConfigureServices(IServiceCollection services)
    {
        // Add logging service
        services.AddLogging();

        // Add controllers and configure JSON serialization
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        });

        // Add Swagger
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Notes API", Version = "v1" });

            // Add JWT authentication scheme to Swagger
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "JWT Authorization header using the Bearer scheme",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer", // Bearer token scheme
                BearerFormat = "JWT" // Format of Bearer token
            };

            c.AddSecurityDefinition("Bearer", securityScheme);

            // Make sure Swagger UI requires a Bearer token to be passed with requests
            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            };

            c.AddSecurityRequirement(securityRequirement);
        });

        // Add HttpContextAccessor
        services.AddHttpContextAccessor();

        // Configure DbContext and database connection
        services.AddDbContext<ApiDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

        // Configure Identity with custom options
        services.AddIdentity<User, IdentityRole>(options =>
        {
            // Password requirements
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 0;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ApiDbContext>()
        .AddDefaultTokenProviders();

        // Configure JWT authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            string key = Configuration["Jwt:Key"];
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });

        // Configure password hashing service
        services.AddTransient<IPasswordHasher<User>, PasswordHasher>();

        // Add scoped services
        services.AddScoped<UserService>();
        services.AddScoped<JwtTokenProvider>();
        services.AddScoped<AuthService>();
        services.AddScoped<NoteService>();

        // Register repositories and services
        services.AddScoped<INoteRepository, NoteRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        // Register SeedAdminUser
        services.AddScoped<SeedAdminUser>();
    }

    /// <summary>
    /// Configures the HTTP request pipeline.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <param name="env">The web hosting environment.</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            // Use Swagger in development environment
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notes API v1");
            });
        }
        else
        {
            // Handle exceptions in production environment
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        // Configure CORS policy
        app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .SetIsOriginAllowed(origin => true));

        app.UseRouting();

        // Use authentication and authorization
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.UseSeedAdminUser();
    }
}