using System;
using System.Windows;

namespace ___PROJECTNAME___;
public interface IWindowFactory
{
    T CreateWindow<T>() where T : Window;
}
