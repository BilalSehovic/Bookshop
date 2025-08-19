using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;
using WpfApp.Services;

namespace WpfApp.Views.Dialogs;

public partial class MessageDialog : Window
{
    public string Message { get; }
    public new string Title { get; }

    public MessageDialog(string message, string title, MessageDialogType dialogType)
    {
        InitializeComponent();
        
        Message = message;
        Title = title;
        DataContext = this;
        
        SetIcon(dialogType);
    }

    private void SetIcon(MessageDialogType dialogType)
    {
        string iconPath = dialogType switch
        {
            MessageDialogType.Information => "pack://application:,,,/WpfApp;component/Resources/info.png",
            MessageDialogType.Warning => "pack://application:,,,/WpfApp;component/Resources/warning.png",
            MessageDialogType.Error => "pack://application:,,,/WpfApp;component/Resources/error.png",
            _ => "pack://application:,,,/WpfApp;component/Resources/info.png"
        };

        try
        {
            IconImage.Source = new BitmapImage(new System.Uri(iconPath));
        }
        catch
        {
            // If custom icons don't exist, use system icons
            IconImage.Source = dialogType switch
            {
                MessageDialogType.Error => SystemIcons.Error.ToBitmap().ToImageSource(),
                MessageDialogType.Warning => SystemIcons.Warning.ToBitmap().ToImageSource(),
                _ => SystemIcons.Information.ToBitmap().ToImageSource()
            };
        }
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }
}

public static class IconExtensions
{
    public static System.Windows.Media.ImageSource ToImageSource(this System.Drawing.Bitmap bitmap)
    {
        var handle = bitmap.GetHbitmap();
        try
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                handle, 
                System.IntPtr.Zero, 
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }
        finally
        {
            System.Runtime.InteropServices.Marshal.FreeHGlobal(handle);
        }
    }
}