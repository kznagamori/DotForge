using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ___PROJECTNAME___.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly Core.Factory.IWindowFactory? _windowFactory;
    public MainWindowViewModel()
    {
    }
    public MainWindowViewModel(Core.Factory.IWindowFactory windowFactory)
    {
        _windowFactory = windowFactory;
    }
}
