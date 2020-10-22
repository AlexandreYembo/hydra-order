using Hydra.WebAPI.Core.Setups;
using Hydra.WebAPI.Core.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Hydra.Order.API.Setup
{
    public static class SwaggerSetup
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerConfiguration(new SwaggerConfig{Title = "Hydra Order API", 
                        Description = "This API can be used as part of an ecommerce or any other type of enterprise application", 
                        Version = "v1"});
        }

        public static void UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            app.UseSwaggerConfiguration(new SwaggerConfig{Version = "v1"});
        }
    }
}