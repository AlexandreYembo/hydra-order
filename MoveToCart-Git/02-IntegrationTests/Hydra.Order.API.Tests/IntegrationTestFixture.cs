using System;
using Xunit;
using Hydra.Order.API;
using Hydra.Order.API.Tests.Config;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Hydra.Order.API.Tests
{
    /// <summary>
    /// It will test the API using the test startup
    /// </summary>
    [CollectionDefinition(nameof(IntegrationApiTestsFixtureCollection))]
    public class IntegrationApiTestsFixtureCollection : ICollectionFixture<IntegrationTestFixture<StartupApiTests>>
    {   }

/// <summary>
/// Fixture will storage the state, it does not need to recreate the service everytime that start a new test.
/// </summary>
/// <typeparam name="TStartup"></typeparam>
    public class IntegrationTestFixture<TStartup> : IDisposable where TStartup : class
    {
        //Factory: Emulate the API in memory
        public readonly HydraAppFactory<TStartup> Factory;
        public HttpClient Client;

        public IntegrationTestFixture()
        {
            var clientOptions = new WebApplicationFactoryClientOptions
            {
                
            };

            Factory = new HydraAppFactory<TStartup>();
            Client = Factory.CreateClient(clientOptions);
        }

        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
        }
    }
}