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
/// Interaction logic for ___CLASSNAME___.xaml
/// </summary>
public partial class ___CLASSNAME___ : Window
{
    public ___CLASSNAME___()
    {
        InitializeComponent();
    }
    public ___CLASSNAME___(ViewModels.___CLASSNAME___ViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
