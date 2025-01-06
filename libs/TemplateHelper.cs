
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DotForge;
public class TemplateInfoItem
{
    public string DirectoryName { get; set; } = string.Empty; // ディレクトリ名
    public string TemplateName { get; set; } = string.Empty;// テンプレート名

    public bool IsProjectTemplate { get; set; } = false; // プロジェクトテンプレートかどうか
}

public class TemplateHelper
{
    public static List<TemplateInfoItem> GetTempleteInfoList(string templateDirectory)
    {
        // ディレクトリリストを作成するリスト
        var directoryList = new List<TemplateInfoItem>();

        if (Directory.Exists(templateDirectory))
        {
            // サブディレクトリを取得
            var directories = Directory.GetDirectories(templateDirectory);

            foreach (var directory in directories)
            {
                string originalName = Path.GetFileName(directory); // 元のディレクトリ名
                string modifiedName = originalName;
                bool isProjectTemplate = false;
                // "Project." を削除
                if (originalName.StartsWith("Project."))
                {
                    modifiedName = originalName.Substring("Project.".Length);
                    isProjectTemplate = true;
                }
                //originalNameからフルパスを作成する
                string fullPath = Path.Combine(templateDirectory, originalName);
                // リストに追加
                directoryList.Add(new TemplateInfoItem
                {
                    DirectoryName = fullPath,
                    TemplateName = modifiedName,
                    IsProjectTemplate = isProjectTemplate
                });
            }
        }

        return directoryList;
    }

    public static string GetTemplateDirectory()
    {
        string exeDirectory = AppContext.BaseDirectory;
        if (exeDirectory == null)
        {
            throw new ArgumentNullException(nameof(exeDirectory)); // ここは念の為残す。BaseDirectoryがnullを返す可能性は低いが。
        }
        return Path.Combine(exeDirectory, "template");
    }
}
