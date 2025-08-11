# Bookshop Management System

A comprehensive WPF application for managing book inventory and sales, built with C# .NET 8, Entity Framework Core, and PostgreSQL.

## Features

- **Book Management**: Add, edit, delete, and view book inventory
- **Sales Interface**: Sell books with automatic stock management
- **Sales Reporting**: Generate reports by date with sales analytics
- **Database Integration**: PostgreSQL database with Entity Framework Core

## Prerequisites

- .NET 8.0 SDK or later
- PostgreSQL database server
- Visual Studio 2022 or compatible IDE (optional)

## Database Setup

1. Install PostgreSQL and ensure it's running
2. Create a database named `bookshop`
3. Update connection string in `App.xaml.cs` if needed:
   ```csharp
   var connectionString = "Host=localhost;Database=bookshop;Username=postgres;Password=password";
   ```

## Running the Application

1. **Clone/Download** the project files
2. **Open terminal/command prompt** in the project directory
3. **Restore dependencies**:
   ```bash
   dotnet restore
   ```
4. **Build the application**:
   ```bash
   dotnet build
   ```
5. **Run the application**:
   ```bash
   dotnet run
   ```

## First Time Setup

1. Launch the application
2. Click **"Initialize Database"** from the menu to create tables and sample data
3. The application will create sample books to get you started

## Database Schema

### Book Entity
- Id (Primary Key)
- Title
- Author
- ISBN (Unique)
- Price
- StockQuantity
- CreatedAt

### Sale Entity
- Id (Primary Key)
- BookId (Foreign Key to Book)
- SalePrice
- SaleDate
- Quantity

## Application Structure

```
Bookshop/
├── Models/
│   ├── Book.cs
│   └── Sale.cs
├── Data/
│   └── BookshopDbContext.cs
├── Services/
│   └── BookService.cs
├── Views/
│   ├── BookManagementView.xaml(.cs)
│   ├── SalesView.xaml(.cs)
│   └── SalesReportView.xaml(.cs)
├── MainWindow.xaml(.cs)
├── App.xaml(.cs)
└── Bookshop.csproj
```

## Usage

### Book Management
- View all books in a data grid
- Add new books using the form at the bottom
- Select a book to edit or delete it
- All fields are validated before submission

### Sales Interface
- View available books (stock > 0)
- Select a book and set sale price
- Confirm sale to reduce stock and record transaction
- Sales are automatically timestamped

### Sales Report
- Select any date using the date picker
- View all sales for that date
- See summary statistics (total sales, books sold, revenue)
- Use "Today" button for quick access to today's report

## Troubleshooting

### Database Connection Issues
- Verify PostgreSQL is running
- Check connection string credentials
- Ensure database "bookshop" exists

### Build Issues
- Ensure .NET 8.0 SDK is installed
- Run `dotnet restore` to download dependencies
- Check all NuGet packages are properly installed

### Sample Data Not Loading
- Use "Initialize Database" menu option
- Check database permissions
- Verify Entity Framework migrations are applied

## Technology Stack

- **Frontend**: WPF (Windows Presentation Foundation)
- **Backend**: C# .NET 8
- **Database**: PostgreSQL
- **ORM**: Entity Framework Core 8.0
- **Database Provider**: Npgsql (PostgreSQL driver)
- **Dependency Injection**: Microsoft.Extensions.DependencyInjection
- **Hosting**: Microsoft.Extensions.Hosting

## License

This is a sample application for demonstration purposes.