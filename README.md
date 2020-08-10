# Alza product catalog api

# Interview task

## About:

Simple Alza product catalog api as interview task.
 Build in asp.net core web api with versioning and swagger schema.
 MSSQL is used as data storage together with Entity Framework Core ORM.
 Integration tests can use real database or in-memory Entity Framework provider.
 Uses EF core migrations to automatically create/update database.
 Runnable within Visual Studio or with Docker compose.
 There are two version of api – V1 and V2. V2 supports pagination when getting list of products.

Even when api is very simple I used Businness layer for demonstrating DDD practices. I didn&#39;t used Repository and Unit of work patterns explicitly, because Entity framework&#39;s DbContext already implements those patterns and instead mocks in-memory provider can be used for &quot;faster&quot; tests. Testing line of business applications is better with feature/end-to-end tests with docker (e.g. no need to unit test business layer with mocks today).

## Prerequisites to run:

- MSSQL 2016+ any edition, if docker compose is not used or

running tests with real db

- Visual Studio 2019

And/Or

- .net core 3.1 sdk and docker

## How to run application:

For better user testing experience I removed SSL(https) from all configurations.
 Browser window with swagger UI will shows up automatically.
 Database is automatically seed with some dummy products.

- Visual Studio 2019 (run configuration – green triangle – AlzaProductApi project)
  - IIS Express
  - Run as console
  - Run in docker
- .net core 3.1 sdk and docker (without Visual Studio)

Navigate to solution directory and run following command:

_docker-compose up_

## How to run tests:

Tests uses MSTEST framework, should be discoverable in all standard test runners.
 After building solution it should show up in visual studio test explorer, where they can be run.
 Tesst are in project AlzaProductApi.Tests.
 There is standard appsettings.json configuration file where can be set if test should run in in-memory db mode (default) or use real db, there is also connection string (default pointing to localhost default MSSQL instance). Those configuration values can also set trough enviromental variables with TEST\_ALZA\_ prefix.

## Things I wanted to make but didn&#39;t have time:

- Proper structured logging across application
- End to end tests with docker compose
- Use Mediator pattern with commands/command handlers to move application logic from controllers
- More DDD things in business layer (is too simple now)