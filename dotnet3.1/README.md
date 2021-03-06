# Hydra Order

### Architecture Pattern:
- CQRS

    -- Hydra.Order.Application
    
    1. Commands

    2. Events

    3. Queries
    

- DDD

This microservice will implement the process to:
- Create an order and order Item
 
- Apply voucher

- Commands handlers

This project was developed by using TDD.


### Rules:

#### Order


#### Voucher
1 - To apply voucher:

1.1 - Voucher only is applied whether it is valid. To validate this, it will need to check:

1.1.1 - Has a code.

1.1.2 - Expiration date is greater than now.

1.1.3 - Voucher is active.

1.1.4 = Voucher is available (based on limit of use).

1.1.5 - Voucher should apply discount when the value is greater than 0.

1.2 - Calculate discount based on the type of voucher:

1.2.1 - Check whether voucher will apply discount in percentage.

1.2.2 - Check whether voucher will apply based on amount.

1.3 - When the discount is greater than the order amount, the amount should updated to 0.

1.4 Once the voucher is applied, for everytime that there is an update in items the discount should be recalculated.


#### Order Commands - Handler
Command handler will manage a command for each approach of order. There will have few validations:
1 - Comand is valid.

2 - Order exists

3 - Order item exists

4 - When change the order status:

4.1 - Should be done via repository.

4.2 - Should send an event.

5 - AddOrderItemCommand:
5.1 - Check is the order status is new or in progress.
5.2 - Check whether the item has already been added to the order.


#### Hydra Core
It is a project that shares common libraries across the microservices.
###### - Packages:
1- Fluent Validation

2- Mediatr

### Tests

#### Domain tests
- Packages:

1 - xUnit

#### Application tests
- Packages:

1- xUnit

2- MOQ (to mock dependency injection)

3- MOQ.automock (to use the library to create auto mock instance of interfaces)


#### Integration tests
##### Hydra Order API
- Packages:

1 - xUnit

2 - Microsoft.AspNetCore.Mvc.Testing

3 - Microsoft.AspNetCore.App