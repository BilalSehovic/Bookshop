using WpfApp.ViewModels;

namespace WpfApp.Tests.ViewModels;

public class BaseViewModelTests
{
    private class TestViewModel : ViewModelBase
    {
        private string _testProperty = "";
        private int _testNumber = 0;

        public string TestProperty
        {
            get => _testProperty;
            set => SetProperty(ref _testProperty, value);
        }

        public int TestNumber
        {
            get => _testNumber;
            set => SetProperty(ref _testNumber, value);
        }

        // Method to manually trigger property changed
        public void TriggerPropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }
    }

    [Fact]
    public void PropertyChanged_ShouldBeRaisedWhenPropertyChanges()
    {
        // Arrange
        var viewModel = new TestViewModel();
        var propertyChangedEvents = new List<string>();

        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName != null)
                propertyChangedEvents.Add(args.PropertyName);
        };

        // Act
        viewModel.TestProperty = "New Value";

        // Assert
        propertyChangedEvents.Should().Contain(nameof(viewModel.TestProperty));
    }

    [Fact]
    public void PropertyChanged_ShouldNotBeRaisedWhenValueIsTheSame()
    {
        // Arrange
        var viewModel = new TestViewModel();
        viewModel.TestProperty = "Initial Value";

        var propertyChangedRaised = false;
        viewModel.PropertyChanged += (sender, args) => propertyChangedRaised = true;

        // Act
        viewModel.TestProperty = "Initial Value"; // Same value

        // Assert
        propertyChangedRaised.Should().BeFalse();
    }

    [Fact]
    public void SetProperty_ShouldReturnTrueWhenValueChanges()
    {
        // Arrange
        var viewModel = new TestViewModel();
        var initialValue = viewModel.TestProperty;

        // Act
        viewModel.TestProperty = "New Value";

        // Assert
        viewModel.TestProperty.Should().NotBe(initialValue);
    }

    [Fact]
    public void PropertyChanged_ShouldWorkWithDifferentDataTypes()
    {
        // Arrange
        var viewModel = new TestViewModel();
        var propertyChangedEvents = new List<string>();

        viewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName != null)
                propertyChangedEvents.Add(args.PropertyName);
        };

        // Act
        viewModel.TestProperty = "String Value";
        viewModel.TestNumber = 42;

        // Assert
        propertyChangedEvents.Should().Contain(nameof(viewModel.TestProperty));
        propertyChangedEvents.Should().Contain(nameof(viewModel.TestNumber));
        propertyChangedEvents.Should().HaveCount(2);
    }

    [Fact]
    public void OnPropertyChanged_WithExplicitPropertyName_ShouldRaiseEvent()
    {
        // Arrange
        var viewModel = new TestViewModel();
        var raisedPropertyName = "";

        viewModel.PropertyChanged += (sender, args) =>
        {
            raisedPropertyName = args.PropertyName ?? "";
        };

        // Act
        viewModel.TriggerPropertyChanged("CustomProperty");

        // Assert
        raisedPropertyName.Should().Be("CustomProperty");
    }

    [Fact]
    public void PropertyChanged_WithNoSubscribers_ShouldNotThrow()
    {
        // Arrange
        var viewModel = new TestViewModel();

        // Act & Assert
        var exception = Record.Exception(() => viewModel.TestProperty = "New Value");
        exception.Should().BeNull();
    }

    [Fact]
    public void PropertyChanged_WithMultipleSubscribers_ShouldNotifyAll()
    {
        // Arrange
        var viewModel = new TestViewModel();
        var subscriber1Notified = false;
        var subscriber2Notified = false;

        viewModel.PropertyChanged += (sender, args) => subscriber1Notified = true;
        viewModel.PropertyChanged += (sender, args) => subscriber2Notified = true;

        // Act
        viewModel.TestProperty = "New Value";

        // Assert
        subscriber1Notified.Should().BeTrue();
        subscriber2Notified.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("Test")]
    [InlineData("LongPropertyNameWithSpecialCharacters!@#")]
    [InlineData(null)]
    public void OnPropertyChanged_WithVariousPropertyNames_ShouldRaiseEventCorrectly(
        string propertyName
    )
    {
        // Arrange
        var viewModel = new TestViewModel();
        var capturedPropertyName = "";

        viewModel.PropertyChanged += (sender, args) =>
        {
            capturedPropertyName = args.PropertyName ?? "NULL";
        };

        // Act
        viewModel.TriggerPropertyChanged(propertyName!);

        // Assert
        if (propertyName == null)
            capturedPropertyName.Should().Be("NULL");
        else
            capturedPropertyName.Should().Be(propertyName);
    }

    [Fact]
    public void SetProperty_WithNullValues_ShouldWorkCorrectly()
    {
        // Arrange
        var viewModel = new TestViewModel();
        viewModel.TestProperty = "Initial";

        var propertyChanged = false;
        viewModel.PropertyChanged += (sender, args) => propertyChanged = true;

        // Act
        viewModel.TestProperty = null!;

        // Assert
        propertyChanged.Should().BeTrue();
        viewModel.TestProperty.Should().BeNull();
    }
}
