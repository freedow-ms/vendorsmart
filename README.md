# VendorSmart Assessment

## Assessment goals

The assessment goal was to create an API that expose the following endpoints:

- **#1** - Authenticated endpoint to create jobs.
- **#2** - Authenticated endpoint to create vendors.
- **#3** - Authenticated endpoint to return potential vendors for a given job.
- **#4** - Public endpoint to return the total vendors for a job detail.

The API offers the following endpoints:

- **/job** - For the **#1** task.
- **/vendor** - For the **#2** task.
- **/vendor/potential and /vendor/potential/job/{jobId}** - For the **#3** task.
- **/vendor/total** - For the **#4** task.

## Software Engineering

The API was created using a **Modular Monolith** approach, where each module has its own data (stored in memory), responsibilities and Boundary Apis for business related communication between modules. Every layer and module of the solution is oriented in a **DDD** and **Clean Architecture** approach (the definitions will not go beyond the scope of this assessment):

- **Domain Layer**: which contains domain entities and all that is bounded to domain business rules
- **Application Layer**: which contains use cases (requests) and all that is bounded to application rules. Also where Boundary APIs contracts are implemented.
- **Infrastructure Layer**: which contains all the logic in a IOC style (respecting domain contracts) and information where and how data is stored.
- **Crosscutting**: dependency injection for modules usage (both application and infrastructure services).

**The structure of the API can be explained as**:

- At the first level (./src) 4 folder
  - **Job** - Contains all the code for job module.
  - **Vendor** - Contains all the code for the vendor module.
  - **Shared** - Contains all the code for shared data.
  - **Presentation** - Contains the API.

Both Job and Vendor folder have the same structure that follows the example above. The shared folder also respects the layers explained but instead of a single library it contains a **Core** and **SharedKernel** library.

The Core library contains middlewares, helpers, patterns and other pieces of code that are used by every module at each layer. I will enumerate them just for the sake of understanding the Core of the solution:

- **Application**
  - **Exceptions**: Application Exceptions such as EntityNotFoundException.
  - **Extensions**:
    - MediatR extensions to use requests as CQRS and translate [Results](https://github.com/victorDivino/operationResult) to [Http Problem Detail](https://www.rfc-editor.org/rfc/rfc7807)
  - **Middlewares**:
    - MediatR middlewares for handling Requests validations and translating to Result.
    - A simple exception middleware to log and generically recover unhandled exceptions.
  - **Problem Details**: A simple wrapper for [Problem Detail](https://www.rfc-editor.org/rfc/rfc7807).
- **Domain**
  - **Exceptions**: Common domain exceptions such as BusinessException.
  - **Entities**: Common entities implementations.
  - **Contracts**: Common domain contracts.
- **Util**
  - **Exceptions**: Global exception for errors on the Application as a whole.
  - **OperationResult**: A pattern to avoid throwing exceptions and allow propagating them via call stacks wrapped with success and result states. An almost identical version of [OperationResult](https://github.com/victorDivino/operationResult) but with support for errors to integrate with FluentValidation and OperationResult.
  - **Specification**: Specification pattern with evaluator to allow validating specifications used.
- **DI**: Aggregate of configurations for the API (basic auth, swagger, services injection, mediatr and flutentvalidation configs, etc).

Besides the Core, all the other libraries are related to Bounded Contexts:

## SharedKernel

The shared kernel pattern was used to fulfill the common needs of Vendor and Job modules. Since Location and ServiceCategory doesnt seem to belong to any of them, they were designed for the SharedKernel to be used by both modules (Other patterns could be used such as Customer/Supplier).

- **Domain**
  - Location and ServiceCategory entities
  - Location and ServiceCategory data access contracts
- **Infrastructure**
  - DbContext for storing Location and ServiceCategory
  - Seed for Location and ServiceCategory
  - Repositories implementation
- **DI** - SharedKernel Services injection
- **Boundaries** - All Boundary API contracts.

## Vendor and Job Modules

Both modules contains all the domain entities and business rules for each bounded context.

- **Domain**
  - Domain entity
  - Specification for querying potential vendors
  - Repository contract
  - UnitTests for entities
- **Application**
  - Requests for use cases
    - The application could have named the Requests as Command and Queries but it named with the suffix Request to avoid making the solution more complex.
  - Validations for requests
  - Dtos for each request
  - UnitTests for use cases
- **Infrastructure**
  - Entity repositories
  - DbContexts
  - Entity seeds
- **DI**
  - Module injection
  - Boundary Apis

# Run

To run the API, use `docker compose up --build` and open `http://localhost:5100/swagger/index.html` to run Swagger.
