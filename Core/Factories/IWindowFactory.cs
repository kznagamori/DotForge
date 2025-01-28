using System;
using System.Windows;

namespace DotForge;
public interface IWindowFactory
{
    T CreateWindow<T>() where T : Window;
}
