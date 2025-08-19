using System.Windows;
using WpfApp.Interfaces;

namespace WpfApp.Services;

public class DialogService : IDialogService
{
    public void ShowInformation(
        string message,
        string title = "Information",
        MessageBoxButton button = MessageBoxButton.OK,
        MessageBoxImage icon = MessageBoxImage.Information
    )
    {
        MessageBox.Show(message, title, button, icon);
    }

    public void ShowWarning(
        string message,
        string title = "Warning",
        MessageBoxButton button = MessageBoxButton.OK,
        MessageBoxImage icon = MessageBoxImage.Warning
    )
    {
        MessageBox.Show(message, title, button, icon);
    }

    public void ShowError(
        string message,
        string title = "Error",
        MessageBoxButton button = MessageBoxButton.OK,
        MessageBoxImage icon = MessageBoxImage.Error
    )
    {
        MessageBox.Show(message, title, button, icon);
    }

    public bool ShowConfirmation(
        string message,
        string title = "Confirmation",
        MessageBoxButton button = MessageBoxButton.YesNo,
        MessageBoxImage icon = MessageBoxImage.Question
    )
    {
        var result = MessageBox.Show(message, title, button, icon);

        return result == MessageBoxResult.Yes;
    }
}
