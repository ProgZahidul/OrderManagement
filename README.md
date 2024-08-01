# Order Management System

Order Management System built with ASP.NET Core MVC. It allows users to create, edit, view, and print orders, customers, and products. The application dynamically calculates item prices and order totals using JavaScript.

## Getting Started

These instructions will help you set up the project on your local machine for development and testing purposes.

### Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or [Visual Studio Code](https://code.visualstudio.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Installation

1. Clone the repository:
    ```bash
    git clone https://github.com/your-username/order-management-system.git
    ```

2. Navigate to the project directory:
    ```bash
    cd order-management-system
    ```

3. Restore the dependencies:
    ```bash
    dotnet restore
    ```

4. Update the `appsettings.json` file with your SQL Server connection string:
    ```json
    {
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft.AspNetCore": "Warning"
        }
      },
      "AllowedHosts": "*",
      "ConnectionStrings": { "DefaultConnection": "server=(localdb)\\MSSQLLocalDB; database=ProductOrderDb; trusted_connection=true; trust server certificate=true" }
    }
    ```

5. Apply the migrations to create the database schema:
    ```bash
    dotnet ef database update
    ```

6. Run the application:
    ```bash
    dotnet run
    ```

## Usage

1. Open your web browser and navigate to `https://localhost:7182`.

2. You will see the home page of the Order Management System. From here, you can navigate to the Orders, Customers, and Products sections.

3. To create a new order:
    - Navigate to the "Orders" section.
    - Click on "Create New Order".
    - Fill in the customer information and add products to the order.
    - The total price will be calculated dynamically as you add products and quantities.
    - Click "Create Order" to save the order.

4. To edit an existing order:
    - Navigate to the "Orders" section.
    - Click on "Edit" next to the order you want to modify.
    - Update the order details and click "Save".

5. To print an invoice:
    - Navigate to the "Orders" section.
    - Click on the "Print Invoice" button to generate a printable invoice.

## Features

- Create, edit, view, and print orders
- Create, edit, and view products
- Dynamically calculate item prices and order totals
