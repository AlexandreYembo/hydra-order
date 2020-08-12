using System;
using TechTalk.SpecFlow;

namespace Hydra.BDD.Tests.Order
{
    [Binding]
    public class Order_AddOrderItemSteps
    {
        [Given(@"Has stock available")]
        public void GivenHasStockAvailable()
        {
           //Arrange

            //Act

            //Assert
        }
        
        [Given(@"The user is logged")]
        public void GivenTheUserIsLogged()
        {
            //Arrange

            //Act

            //Assert
        }

        [Given(@"The same product has been added to the order previously")]
        public void GivenTheSameProductHasBeenAddedToTheOrderPreviously()
        {
            //Arrange

            //Act

            //Assert
        }

        [Given(@"That a product is present in the showcase")]
        public void GivenThatAProductIsPresentInTheShowcase()
        {
            //Arrange

            //Act

            //Assert
        }


        [When(@"The user add a new item to the basket")]
        public void WhenTheUserAddANewItemToTheBasket()
        {
            //Arrange

            //Act

            //Assert
        }

        [When(@"The user add a new item up to the stock available")]
        public void WhenTheUserAddANewItemUpToTheStockAvailable()
        {
            //Arrange

            //Act

            //Assert
        }

        [Then(@"The user will be redirected to order process")]
        public void ThenTheUserWillBeRedirectedToOrderProcess()
        {
            //Arrange

            //Act

            //Assert
        }

        [Then(@"The amount of order will be the same of the same of the order item added")]
        public void ThenTheAmountOfOrderWillBeTheSameOfTheSameOfTheOrderItemAdded()
        {
            //Arrange

            //Act

            //Assert
        }

        [Then(@"The user will receive a message that is allow to add more items that the limit specified")]
        public void ThenTheUserWillReceiveAMessageThatIsAllowToAddMoreItemsThatTheLimitSpecified()
        {
            //Arrange

            //Act

            //Assert
        }

        [Then(@"The quantity will be added to the product")]
        public void ThenTheQuantityWillBeAddedToTheProduct()
        {
            //Arrange

            //Act

            //Assert
        }

        [Then(@"The amount will consider the multiplcation of quantity and the product price")]
        public void ThenTheAmountWillConsiderTheMultiplcationOfQuantityAndTheProductPrice()
        {
            //Arrange

            //Act

            //Assert
        }
    }
}
