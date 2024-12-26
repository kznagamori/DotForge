using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;

namespace DotForge.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        // 行ごとの有効／無効を管理するプロパティ
        [ObservableProperty]
        private bool isTemplateRowEnabled = true;

        [ObservableProperty]
        private bool isProjectFileRowEnabled = true;

        [ObservableProperty]
        private bool isProjectNameRowEnabled = true;

        [ObservableProperty]
        private bool isClassNameRowEnabled = true;

        [ObservableProperty]
        private bool isOutputDirRowEnabled = true;

        // ListBox 用データ
        [ObservableProperty]
        private ObservableCollection<string> templates = new();

        // ComboBox の選択アイテム
        [ObservableProperty]
        private string selectedTemplate = string.Empty;

        // プロジェクトファイルパス
        [ObservableProperty]
        private string projectFilePath = string.Empty;

        // プロジェクト名
        [ObservableProperty]
        private string projectName = string.Empty;

        // クラス名
        [ObservableProperty]
        private string className = string.Empty;

        // 出力先ディレクトリ
        [ObservableProperty]
        private string outputDirectory = string.Empty;

        // コマンド: プロジェクトファイル選択
        [RelayCommand]
        private void OpenProjectFile()
        {
            // ここでファイルダイアログを開くなどの実装を行う想定
            // 例: Microsoft.Win32.OpenFileDialog を使ってパスを取得
            // ProjectFilePath = 取得したパス;
        }

        // コマンド: 出力先ディレクトリ選択
        [RelayCommand]
        private void OpenOutputDir()
        {
            // ここでフォルダブラウズダイアログを開くなどの実装を行う想定
            // OutputDirectory = 取得したディレクトリ;
        }

        // コマンド: 出力処理
        [RelayCommand]
        private void GenerateOutput()
        {
            // 実際の出力処理を行う想定
            // 例: ファイル生成・コンパイルなど
        }


        private List<TemplateInfoItem> _TemplateInfoList = new();
        public MainViewModel()
        {
            // コンストラクタで初期値等を設定
            // 実行中のアセンブリの場所を取得
            string templateDirectory = TemplateHelper.GetTemplateDirectory();
            _TemplateInfoList = TemplateHelper.GetTempleteInfoList(templateDirectory);
            templates.Clear();
            foreach (var item in _TemplateInfoList)
            {
                templates.Add(item.TemplateName);
            }
        }
    }
}
