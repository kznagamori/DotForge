using System.Configuration;
using System.Data;
using System.Windows;

namespace DotForge;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private static Mutex? mutex = null;

    protected override void OnStartup(StartupEventArgs e)
    {
        // 多重起動の防止
        /*         const string appName = "DotForge"; // アプリケーション名
                mutex = new Mutex(true, appName, out bool createdNew);

                if (!createdNew)
                {
                    // すでに起動している
                    MessageBox.Show("アプリケーションがすでに立ち上がっています。", "多重起動防止", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Application.Current.Shutdown();
                    return;
                } */

        // 未処理例外のキャッチ
        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            Exception? exception = args.ExceptionObject as Exception;
            if (exception != null)
            {
                MessageBox.Show(exception.ToString(), "未処理例外発生", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        };

        DispatcherUnhandledException += (sender, args) =>
        {
            MessageBox.Show(args.Exception.ToString(), "Dispatcher未処理例外発生", MessageBoxButton.OK, MessageBoxImage.Error);
            args.Handled = true; // 例外処理済みとする
            Application.Current.Shutdown();

        };

        base.OnStartup(e);

    }

    protected override void OnExit(ExitEventArgs e)
    {
        if (mutex != null)
        {
            mutex.ReleaseMutex();
            mutex.Dispose();
        }
        base.OnExit(e);
    }
}

