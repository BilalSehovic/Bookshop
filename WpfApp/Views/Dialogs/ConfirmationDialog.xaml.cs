using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WpfApp.Views.Dialogs;

public partial class ConfirmationDialog : Window
{
    public string Message { get; }
    public new string Title { get; }

    public ConfirmationDialog(string message, string title)
    {
        InitializeComponent();
        
        Message = message;
        Title = title;
        DataContext = this;
        
        SetQuestionIcon();
    }

    private void SetQuestionIcon()
    {
        try
        {
            IconImage.Source = new BitmapImage(new System.Uri("pack://application:,,,/WpfApp;component/Resources/question.png"));
        }
        catch
        {
            // If custom icon doesn't exist, use system icon
            IconImage.Source = SystemIcons.Question.ToBitmap().ToImageSource();
        }
    }

    private void YesButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }

    private void NoButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}