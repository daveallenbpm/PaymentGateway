# Payment Gateway

A simple payment gateway. Requires .netcore version 3.0.

## Setup

If you're running on Windows, run the powershell script, Setup.ps1. This will:
- Install Sqlite3
- Create the sqlite database
- Install Entity Framework (dotnet-ef)
- Update the database to the latest migration

## Exploring the endpoints

The following endpoints are provided:
- GET /api/Payment/{id}
- GET /api/Payment/paymentid/{paymentId}
- POST /api/Payment

More info on these can be found from the Payment Gateway's swagger page (can be found at /swaggerui/index.html).

## Testing

- Unit tests can be found in PaymentGateway.Test.Unit
- An end to end test can be found in PaymentGateway.Test.Integration. This requires the site to be running. The host url (if that needs changing) can be configured in the appsettings.json file in the project. Can be run via RunIntegrationTests.ps1

## Assumptions

- Bank is mocked out. In here, I've assumed an expired card number will return a Rejected status, success otherwise.
- Assumed the card number field contains 16 characters.
- Assumed negative amounts aren't allowed.

## Areas that could be improved

- The currencies could be inserted into a table in the database, rather than being used as an enum.
- More integration testing could be added - e.g. could test repositories using an in-memory database.
- Further exploration of what App Metrics can add into the project.
- Build script doesn't do much!
- Would be interested to try out Fake for scripts.
- Not to mention app logging, containerization, authentication etc!
