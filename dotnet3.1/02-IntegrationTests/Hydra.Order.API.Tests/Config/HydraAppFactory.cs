using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Hydra.Order.API.Tests.Config
{
    /// <summary>
    /// Class used to run the webapi
    /// TStartup is a specific startup - avoid use from the api that will be tested.
    /// WebApplicationFactory is a factory to run an webapplication, similar IIS
    /// </summary>
    public class HydraAppFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseStartup<TStartup>();
            
            //Will read from the file appsettings.Testing.json
            builder.UseEnvironment("Testing");
        }
    }
}