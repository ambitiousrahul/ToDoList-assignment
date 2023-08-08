using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace UrbanFTProject.ToDoList.Web.Middlewares
{
    public static class JWTConfigurationExtension
    {
        public static void AddJWTConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services                
                .AddAuthentication()
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
            {
                var Key = Encoding.UTF8.GetBytes(configuration["JWT:SigningKey"]);
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Key)                    
                };

                o.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}
