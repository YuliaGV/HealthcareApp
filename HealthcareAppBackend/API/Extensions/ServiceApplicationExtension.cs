using Data.Interfaces;
using Data.Services;
using Data;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using API.Errors;

namespace API.Extensions
{
    public static class ServiceApplicationExtension
    {
        public static IServiceCollection AddServicesCollection(this IServiceCollection services, IConfiguration config)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Add Bearer [space] Token \r\n\r\n ",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                 {
                       {
                           new OpenApiSecurityScheme
                           {
                           Reference = new OpenApiReference
                           {
                               Type = ReferenceType.SecurityScheme,
                               Id = "Bearer"
                           },
                           Scheme = "oauth2",
                           Name = "Bearer",
                           In = ParameterLocation.Header


                           },
                           new List<string>()
                       }


                 });
            });

            var connectionString = config.GetConnectionString("db_conn");
            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                                  .Where(e => e.Value.Errors.Count > 0)
                                  .SelectMany(x => x.Value.Errors)
                                  .Select(x => x.ErrorMessage).ToArray();
                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorResponse);
                };
            });



            return services;
        }
    }
}
