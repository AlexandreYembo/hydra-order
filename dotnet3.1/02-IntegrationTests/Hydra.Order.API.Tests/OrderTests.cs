using System;
using System.Net.Http;
using System.Threading.Tasks;
using Hydra.Order.API.Models;
using Hydra.Order.API.Tests.Config;
using Hydra.Tests.Core.Order;
using Xunit;

namespace Hydra.Order.API.Tests
{
    [TestCaseOrderer("Hydra.Tests.Core.Order.PriorityOrderer", "Hydra.Tests.Core.Order")]
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class OrderTests
    {
        private readonly IntegrationTestFixture<StartupApiTests> _testsFixture;

        public OrderTests(IntegrationTestFixture<StartupApiTests> testFixture){
            _testsFixture = testFixture;
        }


        [Fact(DisplayName="Add Item to new order"), TestPriority(1)]
        public async Task AddItem_NewOrder_ShouldReturnSuccess()
        {
            //Arrange
            var orderItem = new OrderItemDto
            {
                Id = Guid.NewGuid(),
                Qty = 2
            };

            //Act
            var postResponse = await _testsFixture.Client.PostAsJsonAsync("api/order", orderItem);

            //Assert
            postResponse.EnsureSuccessStatusCode(); //return 200
        }
    }
}