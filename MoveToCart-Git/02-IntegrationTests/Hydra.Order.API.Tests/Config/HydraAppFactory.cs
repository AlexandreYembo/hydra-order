using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Hydra.Order.API.Tests.Config
{
    /// <summary>
    /// Class used to run the webapi
    /// TStartup is a specific startup - avoid use from the api that will be tested.
    /// </summary>
    public class HydraAppFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseStartup<TStartup>();
            builder.UseEnvironment("Testing"); 
        }
    }
}