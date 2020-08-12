Feature: Order - Add order item
    As a user
    I want to add this item to the order
    So, that I can buy this item in the future

Scenario: Add order item with sucess to a new order
Given That a product is present in the showcase
And Has stock available
And The user is logged
When The user add a new item to the basket
Then The user will be redirected to order process
And The amount of order will be the same of the same of the order item added

Scenario: Add order item with fail to a new order
Given That a product is present in the showcase
And Has stock available
And The user is logged
When The user add a new item up to the stock available
Then The user will receive a message that is allow to add more items that the limit specified

Scenario: Add order item that is already present
Given That a product is present in the showcase
And Has stock available
And The user is logged
And The same product has been added to the order previously
When The user add a new item to the basket
Then The user will be redirected to order process
And The quantity will be added to the product
And The amount will consider the multiplcation of quantity and the product price