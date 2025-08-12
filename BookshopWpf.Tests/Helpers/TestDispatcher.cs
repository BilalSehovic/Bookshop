using System.Windows.Threading;

namespace WpfApp.Tests.Helpers;

/// <summary>
/// Test helper for WPF Dispatcher operations in unit tests
/// </summary>
public static class TestDispatcher
{
    /// <summary>
    /// Executes an action on the UI thread for testing
    /// </summary>
    public static void Invoke(Action action)
    {
        if (Dispatcher.CurrentDispatcher != null)
        {
            Dispatcher.CurrentDispatcher.Invoke(action);
        }
        else
        {
            action();
        }
    }

    /// <summary>
    /// Executes an async action on the UI thread for testing
    /// </summary>
    public static async Task InvokeAsync(Func<Task> action)
    {
        if (Dispatcher.CurrentDispatcher != null)
        {
            await Dispatcher.CurrentDispatcher.InvokeAsync(action);
        }
        else
        {
            await action();
        }
    }
}
