using System.Windows.Controls;

namespace WpfApp.Services;

public interface INavigationService
{
    void Navigate<TView>()
        where TView : UserControl;
}
