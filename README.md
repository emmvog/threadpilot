# ThreadPilot Integration Layer

## Overview

This project is the first version of an integration layer between a new core system (**ThreadPilot**) and multiple legacy systems.

It consists of two Azure Function Apps written in **.NET 8**:

- **VehicleFunctionApp**: Accepts a vehicle registration number and returns vehicle information.
- **InsuranceFunctionApp**: Accepts a personal identification number and returns all insurances the person has, along with monthly costs. If the person has car insurance, vehicle information is retrieved by calling the VehicleFunctionApp.

The apps communicate via HTTP and are kept separate for modularity, testability, and future scalability.

---

## Architecture & Design

- Each Function App is independently hosted and runs separately.
- Vehicle data is mocked in-memory for simplicity.
- The `InsuranceFunctionApp` uses an injected `IVehicleClient` interface to call `VehicleFunctionApp` — promoting clean separation of concerns and making it easy to mock in tests.
- Dependency injection, async programming, and best practices are applied throughout for maintainability and extensibility.

---

## API Versioning & Future Extensibility

This solution is designed with long-term versioning. Although the current implementation uses unversioned endpoints for simplicity, the routing structure supports **URL-based versioning** such as:

```
/api/v1/vehicle/{registrationNumber}
/api/v1/insurance/{personId}
```

Using this pattern makes it easy to introduce breaking changes or improvements in a future **`v2`** release, while still supporting older clients.

For production readiness, these APIs could be exposed through **Azure API Management (APIM)** to:
- Enforce authentication and authorization (e.g., JWT, OAuth, API keys)
- Apply rate limits and caching
- Manage multiple API versions simultaneously
- Provide developer-friendly documentation and onboarding
- Work with Swagger or Dev portal

The codebase is also structured for **extensibility**:
- Interfaces like `IVehicleClient` allow swapping or extending service logic (e.g., using an external database or real API).
- Modular, separated projects make it easier to test, deploy, and scale services independently.
- Adding new insurance types or services (e.g., home insurance, travel insurance) would only require extending the insurance data logic without altering the core integration.

---

## How to Run It Locally

### Prerequisites

- .NET 8 SDK
- Azure Functions Core Tools

### Steps

Start each function in its own terminal:

```bash
# Start Vehicle Function
cd VehicleFunctionApp
func start --port 7071
```

```bash
# Start Insurance Function
cd ../InsuranceFunctionApp
func start --port 7072
```

You can then use tools like **Postman** or `curl` to call:

- `GET http://localhost:7071/api/vehicle/ABC123`
- `GET http://localhost:7072/api/insurance/12345`

---

## Testing

- Each project includes unit tests using **xUnit** and **Moq**.
- Tests validate business logic, data returned, and cross-function integration via a mocked `IVehicleClient`.
- To run tests:
  ```bash
  dotnet test
  ```

---

## CI/CD Pipeline

A GitHub Actions workflow is included to:
- Restore NuGet packages
- Build all projects
- Run unit tests

You’ll find the workflow at `.github/workflows/dotnet-ci.yml`.

---

## Personal Reflection

**Similar project or experience:**  
I’ve worked with Azure Functions for GDPR cleanups, data transformations, back end for frontend data transfer, API caching, and workflow orchestration for Logic Apps.
I have used functions to process large data files from Datafactory, sftp and other external sources.

**What was challenging or interesting in this assignment:**  
Using the isolated model in .NET 8 and wiring HTTP-based communication between two Function Apps was a good test of architectural planning and local dev coordination.

**What I would improve with more time:**  
I’d enhance the solution with Azure API Management, add JWT-based authentication, expand the test suite (including integration tests), and implement stricter input validation and HTTPS enforcement.
Add more time to build Postman test runs, load tests, pipelines for generating the resources in Azure
Work with feature toggle if required