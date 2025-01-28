using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace DotForge;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private static Mutex? mutex = null;
    private readonly IServiceProvider _serviceProvider;

    public App()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();
    }

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

        string exeDirectory = AppContext.BaseDirectory;
        if (exeDirectory == null)
        {
            throw new ArgumentNullException(nameof(exeDirectory)); // ここは念の為残す。BaseDirectoryがnullを返す可能性は低いが。
        }
        var mainWindow = _serviceProvider.GetRequiredService<Views.MainWindow>();
        mainWindow.Show();
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

    private void ConfigureServices(ServiceCollection services)
    {
        // 実行ファイルのディレクトリを取得
        string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
        // settings.toml のパスを組み立て
        string settingsPath = System.IO.Path.Combine(exeDirectory, "settings.toml");


        // ファクトリの登録
        services.AddSingleton<IWindowFactory, WindowFactory>();

        // 各データサービスの登録
        // 設定サービスを登録
        services.AddSingleton<Services.ISettingsService>(provider => new Services.SettingsService(settingsPath));

        // 各ウィンドウの登録
        services.AddTransient<Views.MainWindow>();
        services.AddTransient<ViewModels.MainWindowViewModel>();
        services.AddTransient<Views.HelpWindow>();
        services.AddTransient<ViewModels.HelpWindowViewModel>();

        // 他のサービスやウィンドウもここに登録
    }

}

