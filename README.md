# Hacker News API

This project is a .NET Core application that interacts with the Hacker News API to fetch and cache stories. It includes services, adapters, and configurations to manage the data flow and caching mechanisms.

## Table of Contents

- [Prerequisites](#prerequisites)
- [Setup](#setup)
- [Running the Application](#running-the-application)
- [Running Tests](#running-tests)
- [Assumptions](#assumptions)
- [Enhancements and Changes](#enhancements-and-changes)

## Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Visual Studio Code](https://code.visualstudio.com/) or any other preferred IDE
- Internet connection (for accessing Hacker News API)

## Setup

1. **Clone the repository:**

   ```sh
   git clone https://github.com/otemko/HackerNews.git
   cd HackerNews

2. **Restore dependencies:**

    ```sh
    dotnet restore

3. **Build the project:**

    ```sh
    dotnet build

## Running the Application

1. **Navigate to the API project directory:**

    ```sh
    cd HackerNews.API

2. **Run the application:**

    ```sh
    dotnet run

The application will start and listen on the default port (usually https://localhost:7228).

## Running tests

1. **Navigate to the test project directory:**
    ```sh
    cd HackerNews.Tests

2. **Run the tests:**
    ```sh
    dotnet test

This will execute all the unit tests and display the results in the terminal.

## Assumptions

* The HackerNews API is available and accessible.
* The configuration settings in appsettings.json are correctly set up for the API base URL and cache settings.
* The application uses in-memory caching for simplicity and performan

## Enhancements and Changes

- **Separate Implementation for Memory Cache:** Refactor the memory caching logic into a separate service or utility class to improve modularity and maintainability.

* **Optimize Data Fetching:** That is possibly avoid retrieving MaxItemId from the Hacker News API and instead directly fetch the best results when possible, reducing unnecessary calls.

* **Code Quality Improvements:** Integrate StyleCop and define clear coding standards and rules to enforce consistent code style.

* **Enhanced Logging:** Cover all critical sections of the code with logging to improve traceability. Implement a robust logging framework such as Serilog or NLog for structured and detailed logs, aiding debugging and monitoring.
* **Robust Configuration Management:** Leverage external configuration management systems like Azure App Configuration or AWS Parameter Store to securely manage and store configuration settings, ensuring better scalability and security.

* **Containerization:** Add Docker support to containerize the application, making it easier to deploy across various environments and enabling scalability.

* **Automated CI/CD Pipeline:** Set up a Continuous Integration/Continuous Deployment (CI/CD) pipeline using tools like GitHub Actions, Azure DevOps, or Jenkins to automate the build, testing, and deployment processes, ensuring faster and more reliable releases.

* **Authentication and Authorization:** Implement authentication mechanisms (e.g., JWT, OAuth2.0) and role-based authorization to secure the API endpoints from unauthorized access.

* **Environment-Specific Configurations:** Add support for multiple environments (e.g., development, staging, production) with varying configurations such as logging levels, rate-limiting policies, and cache durations, allowing the application to adapt to different deployment contexts.

* **Comprehensive Unit Testing:** Write unit tests to cover all layers of the application (controllers, services, etc.) to ensure correctness and reliability.
* **Documentation:** Ensure that all components are well-documented. Add inline documentation where missing.

    
