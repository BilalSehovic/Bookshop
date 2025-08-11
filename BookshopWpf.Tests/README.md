# WpfApp.Tests

Comprehensive unit test suite for the Bookshop WPF application.

## Test Structure

- **ViewModels/**: Tests for all ViewModels
  - `BookManagementViewModelTests` - CRUD operations, validation, commands
  - `SalesViewModelTests` - Sales processing, stock validation, error handling
  - `SalesReportViewModelTests` - Report generation, CSV export, date filtering
  - `MainWindowViewModelTests` - Navigation, database initialization, view management

- **TestHelpers/**: Utility classes for testing
  - `TestData` - Factory methods for creating test data
  - `TestDispatcher` - Helper for WPF Dispatcher operations in tests

## Test Coverage

The test suite covers:

✅ **Command Pattern Testing**
- Command execution and CanExecute logic
- Command parameter handling
- Async command operations

✅ **MVVM Pattern Testing**
- Property change notifications (INotifyPropertyChanged)
- Data binding scenarios
- ViewModel state management

✅ **Business Logic Testing**
- CRUD operations
- Validation rules
- Error handling
- Data calculations

✅ **Service Integration Testing**
- Mock service interactions
- Async operation handling
- Exception scenarios

✅ **UI State Testing**
- Button enable/disable states
- Form population and clearing
- Status message updates

## Running Tests

```bash
dotnet test WpfApp.Tests/WpfApp.Tests.csproj
```

## Test Technologies

- **xUnit** - Test framework
- **Moq** - Mocking framework
- **FluentAssertions** - Assertion library
- **Coverlet** - Code coverage

## Key Test Patterns

1. **Arrange-Act-Assert (AAA)** pattern
2. **Mock dependencies** using Moq
3. **Theory/InlineData** for parametized tests
4. **Async test handling** with Task.Delay for timing
5. **Property change event verification**
6. **Exception handling verification**