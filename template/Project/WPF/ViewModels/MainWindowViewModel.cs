using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ___PROJECTNAME___.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly IWindowFactory? _windowFactory;
    public MainWindowViewModel()
    {
    }
    public MainWindowViewModel(IWindowFactory windowFactory)
    {
        _windowFactory = windowFactory;
    }
}
