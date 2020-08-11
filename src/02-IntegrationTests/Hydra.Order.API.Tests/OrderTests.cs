using System.Threading.Tasks;
using Hydra.Order.API.Tests.Config;
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
            
        }
    }
}