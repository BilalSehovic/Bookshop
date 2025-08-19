using System.Threading.Tasks;
using System.Windows;

namespace WpfApp.Services;

public class DialogService : IDialogService
{
    public async Task ShowInformationAsync(string message, string title = "Information")
    {
        await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            var dialog = new Views.Dialogs.MessageDialog(message, title, MessageDialogType.Information);
            dialog.ShowDialog();
        });
    }

    public async Task ShowWarningAsync(string message, string title = "Warning")
    {
        await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            var dialog = new Views.Dialogs.MessageDialog(message, title, MessageDialogType.Warning);
            dialog.ShowDialog();
        });
    }

    public async Task ShowErrorAsync(string message, string title = "Error")
    {
        await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            var dialog = new Views.Dialogs.MessageDialog(message, title, MessageDialogType.Error);
            dialog.ShowDialog();
        });
    }

    public async Task<bool> ShowConfirmationAsync(string message, string title = "Confirmation")
    {
        return await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            var dialog = new Views.Dialogs.ConfirmationDialog(message, title);
            return dialog.ShowDialog() == true;
        });
    }
}

public enum MessageDialogType
{
    Information,
    Warning,
    Error
}