using System;
using System.Windows;

namespace ___PROJECTNAME___.Core.Factory;
public interface IWindowFactory
{
    T CreateWindow<T>() where T : Window;
}
