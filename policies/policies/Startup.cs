using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using policies.Models;
using policies.Services;
using Microsoft.AspNetCore.Cors;

namespace policies
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
            // Configuration for MongoDB and other services
            services.Configure<PoliciesSettings>(Configuration.GetSection(nameof(PoliciesSettings)));
            services.Configure<UsersSettings>(Configuration.GetSection(nameof(UsersSettings)));

            services.AddSingleton<IPoliciesSettings>(d => d.GetRequiredService<IOptions<PoliciesSettings>>().Value);
            services.AddSingleton<IUsersSettings>(d => d.GetRequiredService<IOptions<UsersSettings>>().Value);
            services.AddSingleton<IPoliciesService, PoliciesService>(); // Agrega esta línea

            services.AddSingleton<PoliciesService>();
            services.AddSingleton<UsersService>();

            // Add JWT authentication
            var jwtSettings = Configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // In development, you can set this to true for HTTPS
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)), // Use the secret key from configuration
                    ValidateIssuer = false, // Set to true if you want to validate the issuer
                    ValidateAudience = false, // Set to true if you want to validate the audience
                    ClockSkew = TimeSpan.FromHours(24)
                };
            });

            // Swagger configuration
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Policies API", Version = "v1" });

                // Agregar configuración para el token JWT en Swagger
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Bearer",
                    Description = "JWT Authorization header using the Bearer scheme",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securityScheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        securityScheme,
                        Array.Empty<string>()
                    }
                });
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Enable Swagger in Development mode
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Policies API V1");

                    // Habilitar la autenticación en SwaggerUI
                    c.OAuthClientId("swagger");
                    c.OAuthAppName("Swagger UI");
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors("AllowAllOrigins");


            // Use authentication before authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "GetPolicy",
                    pattern: "api/policies",
                    defaults: new { controller = "Policies", action = "Post" }
                );
            });
        }
    }
}
