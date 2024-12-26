
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DotForge;
public class TemplateInfoItem
{
    public string DirectoryName { get; set; } = string.Empty; // ディレクトリ名
    public string TemplateName { get; set; } = string.Empty;// テンプレート名

    public bool IsProjectTemplate => DirectoryName.StartsWith("Project.");
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

                // "Project." を削除
                if (originalName.StartsWith("Project."))
                {
                    modifiedName = originalName.Substring("Project.".Length);
                }

                // リストに追加
                directoryList.Add(new TemplateInfoItem
                {
                    DirectoryName = originalName,
                    TemplateName = modifiedName
                });
            }
        }

        return directoryList;
    }

    public static string GetTemplateDirectory()
    {
        string? exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (exeDirectory == null)
        {
            throw new ArgumentNullException(nameof(exeDirectory));
        }
        return Path.Combine(exeDirectory, "template");
    }
}
