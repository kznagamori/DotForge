using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Windows.Forms.Integration;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Text;

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

        [ObservableProperty]
        private bool isOutputEnabled = false;

        // ListBox 用データ
        [ObservableProperty]
        private ObservableCollection<string> templates = new();

        // ComboBox の選択アイテム
        [ObservableProperty]
        private string selectedTemplate = string.Empty;

        DotForge.TemplateInfoItem? _SelectedItem;
        partial void OnSelectedTemplateChanged(string? oldValue, string newValue)
        {
            _SelectedItem = _TemplateInfoList.Find(x => x.TemplateName == newValue);
            if (_SelectedItem != null)
            {
                if (_SelectedItem.IsProjectTemplate)
                {
                    IsProjectFileRowEnabled = false;
                    IsProjectNameRowEnabled = true;
                    IsClassNameRowEnabled = false;
                    IsOutputDirRowEnabled = true;
                }
                else
                {
                    IsProjectFileRowEnabled = true;
                    IsProjectNameRowEnabled = false;
                    IsClassNameRowEnabled = true;
                    IsOutputDirRowEnabled = true;
                }
                IsOutputEnabled = true;
            }
            else
            {
                throw new System.Exception("Template not found");
            }

        }

        // プロジェクトファイルパス
        [ObservableProperty]
        private string statusText = string.Empty;

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
            Microsoft.Win32.OpenFileDialog dialog = new();
            dialog.Filter = "プロジェクトファイル(*.csproj)|*.csproj";
            dialog.Title = "プロジェクトファイルを選択してください";
            // マイドキュメントのパスを取得
            dialog.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (dialog.ShowDialog() == true)
            {
                ProjectFilePath = dialog.FileName;
                // ProjectFilePathのベースファイル名を取得
                ProjectName = Path.GetFileNameWithoutExtension(ProjectFilePath);
            }
        }

        // コマンド: 出力先ディレクトリ選択
        [RelayCommand]
        private void OpenOutputDir()
        {
            //CommonOpenFileDialogでフォルダー選択ダイアログを表示
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;
                dialog.Title = "出力先ディレクトリを選択してください";
                // ProjectFilePathが設定されている場合は、そのディレクトリを初期ディレクトリとする
                if (!string.IsNullOrEmpty(ProjectFilePath))
                {
                    dialog.InitialDirectory = Path.GetDirectoryName(ProjectFilePath);
                }
                else
                {
                    // マイドキュメントのパスを取得
                    dialog.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    OutputDirectory = dialog.FileName;
                }
            }
        }

        // コマンド: 出力処理
        [RelayCommand]
        private void GenerateOutput()
        {
            if (_SelectedItem == null)
            {
                throw new System.Exception("Template not found");
            }
            if (_SelectedItem.IsProjectTemplate)
            {
                if (ProjectName == string.Empty)
                {
                    StatusText = "プロジェクト名が未入力です";
                    return;
                }
            }
            else
            {
                if (ProjectName == string.Empty)
                {
                    StatusText = "プロジェクト名が未入力です";
                    return;
                }
                if (ClassName == string.Empty)
                {
                    StatusText = "クラス名が未入力です";
                    return;
                }
            }
            StatusText = "出力中...";
            if (OutputDirectory == string.Empty)
            {
                StatusText = "出力先ディレクトリが未入力です";
                return;
            }
            if (!TryHelper.TryExec(() => new FileInfo(OutputDirectory)))
            {
                StatusText = "出力先ディレクトリが無効です";
                return;
            }
            // 出力先ディレクトリが存在しない場合は作成
            if (!Directory.Exists(OutputDirectory))
            {
                //途中がない場合も作成する
                Directory.CreateDirectory(OutputDirectory);
            }
            if (_SelectedItem.IsProjectTemplate)
            {
                // 出力先+プロジェクト名のディレクトリを作成
                OutputDirectory = Path.Combine(OutputDirectory, ProjectName);
                Directory.CreateDirectory(OutputDirectory);
            }
            // 選択したテンプレートディレクトリを取得
            string templateDirectory = _SelectedItem.DirectoryName;
            // 出力先ディレクトリにテンプレートをコピー
            DirectoryCopyHelper.CopyDirectory(templateDirectory, OutputDirectory, true);

            // 出力先ディレクトリをサブディレクトリを含めて再帰的に取得
            // ファイル名の一部にに___PROJECTNAME___が含まれるファイルを検索して置換
            // ___PROJECTNAME___はProjectNameに置き換える
            // ファイル名の一部にに___CLASSNAME___が含まれるファイルを検索して置換
            // ___CLASSNAME___はClassNameに置き換える
            // 再帰的にサブディレクトリを含めて検索
            var allFiles = Directory.GetFiles(OutputDirectory, "*", SearchOption.AllDirectories);

            foreach (var filePath in allFiles)
            {
                string fileName = Path.GetFileName(filePath); // ファイル名のみ取得
                string directory = Path.GetDirectoryName(filePath)!; // 親ディレクトリ取得
                string newFileName = fileName;
                // ファイル名の置換処理
                if (fileName.Contains("___PROJECTNAME___"))
                {
                    newFileName = newFileName.Replace("___PROJECTNAME___", ProjectName);
                }
                if (!_SelectedItem.IsProjectTemplate)
                {
                    if (fileName.Contains("___CLASSNAME___"))
                    {
                        newFileName = newFileName.Replace("___CLASSNAME___", ClassName);
                    }
                }
                // 変更があった場合、新しい名前でリネーム
                if (newFileName != fileName)
                {
                    string newFilePath = Path.Combine(directory, newFileName);

                    // ファイルをリネーム
                    File.Move(filePath, newFilePath);

                    StatusText = $"リネーム: {filePath} -> {newFilePath}";
                }
            }

            // 出力先ディレクトリをサブディレクトリを含めて再帰的に取得
            // ファイルを読み込み、___PROJECTNAME___をProjectNameに置換
            // ファイルを読み込み、___CLASSNAME___をClassNameに置換
            // 再帰的にファイルを取得
            foreach (var filePath in Directory.GetFiles(OutputDirectory, "*", SearchOption.AllDirectories))
            {
                // ファイルを読み込む
                byte[] fileBytes = File.ReadAllBytes(filePath);

                // UTF-8 BOM があるか確認
                bool hasBOM = fileBytes.Length >= 3 &&
                              fileBytes[0] == 0xEF &&
                              fileBytes[1] == 0xBB &&
                              fileBytes[2] == 0xBF;

                string fileContent;
                if (hasBOM)
                {
                    // BOM をスキップして文字列を読み取る
                    fileContent = Encoding.UTF8.GetString(fileBytes, 3, fileBytes.Length - 3);
                }
                else
                {
                    // BOM がない場合はそのまま読み取る
                    fileContent = Encoding.UTF8.GetString(fileBytes);
                }

                // 置換処理
                string updatedContent = fileContent
                    .Replace("___PROJECTNAME___", ProjectName)
                    .Replace("___CLASSNAME___", ClassName);

                // ファイルに変更がある場合のみ保存
                if (updatedContent != fileContent)
                {
                    StatusText = $"更新: {filePath}";

                    // UTF-8 BOM を付与して保存
                    byte[] updatedBytes = hasBOM
                        ? Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(updatedContent)).ToArray()
                        : Encoding.UTF8.GetBytes(updatedContent);

                    File.WriteAllBytes(filePath, updatedBytes);
                }
            }
            StatusText = "出力が完了しました";
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
