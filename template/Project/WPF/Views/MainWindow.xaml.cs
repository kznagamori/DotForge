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

namespace ___PROJECTNAME___.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly Core.Factory.IWindowFactory? _windowFactory;
    public MainWindow()
    {
        InitializeComponent();
    }
    public MainWindow(Core.Factory.IWindowFactory windowFactory, ViewModels.MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        _windowFactory = windowFactory;
    }
}