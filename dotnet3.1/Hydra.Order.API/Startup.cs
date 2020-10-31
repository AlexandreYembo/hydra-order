using Hydra.Order.API.Setup;
using Hydra.Order.Data;
using Hydra.WebAPI.Core.Identity;
using Hydra.WebAPI.Core.Setups;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hydra.Order.API
{
    public class Startup
    {
       public Startup(IHostEnvironment hostEnvironment)
        {
           Configuration = HostEnvironmentConfiguration.AddHostEnvironment(hostEnvironment);
    }
 
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiConfiguration(Configuration);
            services.AddJwtConfiguration(Configuration);
            services.AddSwaggerConfiguration();
            services.RegisterServices();
           
            services.AddMediatR(typeof(Startup));

            services.RegisterServices();
            
            services.AddMessageBusConfiguration(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwaggerConfiguration();
            app.UseApiConfiguration(env);
        }
    }
}
