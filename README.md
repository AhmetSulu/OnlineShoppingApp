# OnlineShoppingApp API

OnlineShoppingApp is a backend API developed with .NET 8.0, designed to support the core functionalities of an online shopping platform. The API offers a range of services for user management, product handling, order processing, and application configuration. Additionally, it follows best practices in software architecture by implementing the repository and unit of work patterns, which make data access and transaction management efficient and organized.

## Table of Contents

- [Features](#features)
- [Technologies](#technologies)
- [Setup Instructions](#setup-instructions)
- [Running the Application](#running-the-application)
- [Authentication](#authentication)
- [Project Architecture](#project-architecture)
- [Contribution Guidelines](#contribution-guidelines)
- [License](#license)

## Features

- **User Management**: Supports secure user registration, login, and JWT-based authentication.
- **Product Management**: Comprehensive CRUD functionality for products.
- **Order Management**: Order creation, processing, and tracking.
- **Settings Management**: Configurable application settings.
- **Data Protection**: Secure handling and storage of sensitive information.
- **Maintenance Mode**: Easily enable/disable API access for maintenance.
- **Repository and Unit of Work Patterns**: Organized and flexible data access layer.

## Technologies

- **.NET 8.0**: Core framework used to develop the API.
- **Entity Framework Core**: ORM used for database management.
- **SQL Server**: Database used to store application data.
- **JWT Authentication**: Secure token-based authentication.
- **Swagger UI**: Documentation and testing for API endpoints.
- **Data Protection API**: Provides secure handling of sensitive data.

## Setup Instructions

1. **Clone the Repository**

   ```bash
   
   git clone https://github.com/AhmetSulu/OnlineShoppingApp.git
   cd OnlineShoppingApp
   
2. **Install Dependencies**
   
   ```bash
   dotnet restore
3. **Configure Database**
   
   ```bash
   Open appsettings.json and update the connection string under "ConnectionStrings": { "Default": "Your_SQL_Server_Connection_String" }.
   
4. **Run Migrations**
   Initialize the database with the required schema using Entity Framework migrations:
   
   ```bash
   dotnet ef database update
   
5. **Configure JWT Settings**
   
   ```bash
   In appsettings.json, configure the JWT settings under "Jwt" with values for Issuer, Audience, and SecretKey.

## Running the Application

- Start the application locally by running:
  
  ```bash
  dotnet run
  
## Authentication

- This API uses JWT for secure authentication. Upon successful login, users receive a JWT token, which must be included in the headers of subsequent requests as follows:
  
  ```bash
  Authorization: Bearer {token}
  
## Project Architecture
- Repository Pattern: The IRepository interface provides an abstraction for data access operations, promoting flexibility and maintainability in managing database interactions.
- Unit of Work Pattern: The IUnitOfWork interface manages transaction control, allowing for efficient handling of multiple operations within a single transaction scope.
- Middleware: Custom middleware, such as MaintenanceMode, dynamically manages the application state.
- Data Protection: User data, particularly sensitive information, is protected using the Data Protection API, with persisted keys stored in App_Data.
- Action Filter : Action filters provide additional control over requests and responses. For example, custom filters like TimeControlFilter limit access based on specific access times, adding extra security.

## Contribution Guidelines
- Fork the repository.
- Create a new branch for your feature or bug fix.
- Commit your changes and push them to your fork.
- Open a Pull Request with a description of your changes.
 
## License

This project is licensed under the MIT License.

## Contact

For any questions or support, please email ahmet.sulu1993@gmail.com
