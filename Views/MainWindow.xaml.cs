using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DotForge.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly IWindowFactory? _windowFactory;
    public MainWindow()
    {
        InitializeComponent();
    }
    public MainWindow(ViewModels.MainWindowViewModel viewModel, IWindowFactory windowFactory)
    {
        InitializeComponent();
        DataContext = viewModel;
        _windowFactory = windowFactory;
    }
    // 終了メニューのクリックイベント
    private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
    private void HelpMenu_Click(object sender, RoutedEventArgs e)
    {
        if (_windowFactory == null)
        {
            return;
        }
        var helpWindow = _windowFactory.CreateWindow<HelpWindow>();
        helpWindow.ShowDialog();
    }
}
