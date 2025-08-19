using System.Threading.Tasks;

namespace WpfApp.Services;

public interface IDialogService
{
    Task ShowInformationAsync(string message, string title = "Information");
    Task ShowWarningAsync(string message, string title = "Warning");
    Task ShowErrorAsync(string message, string title = "Error");
    Task<bool> ShowConfirmationAsync(string message, string title = "Confirmation");
}