# CustomerTestApp

CustomerTestApp is a WPF/MVVM/Microservice application for managing customers. It includes a stylized list of customers and a customer editor, with a backend powered by gRPC and ASP.NET Core.

## Features

- Add, update, delete, and filter customers.
- Split view with a list of customers and an editor for customer details.
- Validation for customer properties, including email and discount.
  - Email must contain a valid domain.
  - Discount must be a number between 1 and 30.
  - The "Add Customer" button will only be enabled when both fields are valid.
- Logging with Serilog.
- gRPC communication between the desktop application and the microservice.
- Entity Framework for database operations.

## Setup Instructions

### Prerequisites

- .NET 8.0
- Visual Studio 2022
- SQLite

### Setting Up the Database
1. Install SQLite
If you haven't installed SQLite, download and install it from the SQLite Download Page.
2. *Browse and Select the Database File*:
   - Open the CustomerService folder in your project directory.
   - Ensure the customers.db file is present in the folder.
   - Configure your application to use this database file by updating the connection string in appsettings.json (or similar configuration file) to point to the customers.db file. Example:

     json
     "ConnectionStrings": {
       "DefaultConnection": "Data Source=CustomerService/customers.db"
     }
     

### Running the Application

#### Backend (CustomerService)

1. *Navigate to the CustomerService directory*:

    bash
    cd CustomerService
    

2. *Restore dependencies and build the project*:

    bash
    dotnet restore
    dotnet build
    

3. *Run the backend service*:

    bash
    dotnet run
    

#### Frontend (CustomersTestApp)

1. *Open the solution file* CustomersTestApp.sln in Visual Studio.

2. *Restore dependencies and build the project*.

3. *Run the WPF application*.

### Usage

- *Add Customer*: Enter the customer's name, email (must contain @gmail.com), and discount (1-30), then click "Add Customer". The button will only be enabled when both fields are valid.
- *Remove Customer*: Select a customer from the list and click "Remove".
- *Edit Customer*: Select a customer, modify their details, and click "Save".
- *Filter Customers*: Use the TextBox and ComboBox to filter the customer list by name or email.

### Project Structure

- CustomerService: Contains the ASP.NET Core gRPC microservice.
  - Controllers: gRPC service implementations.
  - Models: Data models.
  - Repositories: Repository pattern for data access.
  - Services: Business logic and service methods.
  - Protos: gRPC proto files.
  - Interceptors: Custom interceptors for logging.
  - Program.cs: Main entry point.
  - Startup.cs: Configuration and middleware setup.

- CustomersTestApp: Contains the WPF application.
  - Views: XAML files for UI.
  - ViewModels: ViewModel classes for data binding.
  - Models: Data models for the application.
  - Commands: Command implementations.
  - Messaging: Messenger implementation for view model communication.
  - App.xaml: Application configuration.
  - MainWindow.xaml: Main window layout.
