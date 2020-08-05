namespace Hydra.Order.API.Tests
{
    public class OrderTests
    {
        private readonly IntegrationTestFixture<StartupApiTests> _testsFixture;

        public OrderTests(IntegrationTestFixture<StartupApiTests> testFixture){
            _testsFixture = testFixture;
        }
    }
}