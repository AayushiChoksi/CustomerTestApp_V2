# CustomerTestApp

CustomerTestApp is a WPF/MVVM/Microservice application for managing customers. It includes a stylized list of customers and a customer editor, with a backend powered by gRPC and ASP.NET Core.

## Features

- Add, update, delete, and filter customers.
- Split view with a list of customers and an editor for customer details.
- Validation for customer properties.
- Logging with Serilog.
- gRPC communication between the desktop application and the microservice.
- Entity Framework for database operations.

## Setup Instructions

### Prerequisites

- .NET 8.0
- Visual Studio 2022
- SQLite


### Clone the Repository
- git clone https://github.com/yourusername/CustomerTestApp.git
- cd CustomerTestApp

### Setting Up the Database
1. Install SQLite
If you haven't installed SQLite, download and install it from the SQLite Download Page.
