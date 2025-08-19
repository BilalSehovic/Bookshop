using System.Windows;

namespace WpfApp.Interfaces;

public interface IDialogService
{
    void ShowInformation(
        string message,
        string title = "Information",
        MessageBoxButton button = MessageBoxButton.OK,
        MessageBoxImage icon = MessageBoxImage.Information
    );
    void ShowWarning(
        string message,
        string title = "Warning",
        MessageBoxButton button = MessageBoxButton.OK,
        MessageBoxImage icon = MessageBoxImage.Warning
    );
    void ShowError(
        string message,
        string title = "Error",
        MessageBoxButton button = MessageBoxButton.OK,
        MessageBoxImage icon = MessageBoxImage.Error
    );
    bool ShowConfirmation(
        string message,
        string title = "Confirmation",
        MessageBoxButton button = MessageBoxButton.YesNo,
        MessageBoxImage icon = MessageBoxImage.Question
    );
}
