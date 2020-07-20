# Hydra Order
This microservice will implement the process to:
- Create a order and order Item
 
- Apply vourche

- Commands handlers

This project was developed by using TDD.


### Rules:

#### Order


#### Voucher
1 - To apply voucher:

1.1 - Voucher only is applied wheither it is valid. To validate this, it will need to check:

1.1.1 - Has a code.

1.1.2 - Expiration date is greater than now.

1.1.3 - Voucher is active.

1.1.4 = Voucher is available (based on limit of use).

1.1.5 - Voucher should apply discount when the value is greater than 0.

1.2 - Calculate discount based on the type of voucher:

1.2.1 - Check weither voucher will apply discount in percentage.

1.2.2 - Check weither voucher will apply based on amount.

1.3 - When the discount is greater than the order amount, the amount should updated to 0.

1.4 Once the voucher is applied, for everytime that there is an update in items the discount should be recalculated.

#### Hydra Core
It is a project that shares common libraries across the microservices.
###### - Packages:
1- Fluent Validation

### Tests
- Packages:
1 - xUnit

