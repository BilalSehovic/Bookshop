# Bookshop

A modern .NET WPF bookshop management application built with Entity Framework Core and PostgreSQL database.

## Features

- MVVM architecture with dependency injection
- Entity Framework Core with PostgreSQL
- Entity Framework migrations
- Book inventory management
- Sales tracking and reporting

## Architecture

- **WpfApp**: Main WPF application (.NET 9.0)
- **DataAccessLayer**: Entity Framework Core data layer
- **MVVM Pattern**: ViewModels with RelayCommand and data binding
- **Dependency Injection**: Microsoft.Extensions.Hosting

## Quick Start

```bash
# Build the application
dotnet build

# Run the application
dotnet run --project WpfApp
```

## Database Setup

Configure PostgreSQL connection in `appsettings.json` or user secrets, then run:

```bash
# Add new migration
dotnet ef migrations add MigrationName --project DataAccessLayer

# Update database
dotnet ef database update --project DataAccessLayer
```

## Requirements

- .NET 9.0
- PostgreSQL database