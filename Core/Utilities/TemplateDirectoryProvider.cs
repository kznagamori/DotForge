
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DotForge.Utilities;
public class DirectoryInfoItem
{
    public string FullPath { get; set; } = string.Empty; // ディレクトリ名
    public string DirectoryName { get; set; } = string.Empty;// テンプレート名
}

public class TemplateDirectoryProvider
{
    public static List<DirectoryInfoItem> GetDirectoryInfoList(string templateDirectory)
    {
        // ディレクトリリストを作成するリスト
        var directoryList = new List<DirectoryInfoItem>();

        if (Directory.Exists(templateDirectory))
        {
            // サブディレクトリを取得
            var directories = Directory.GetDirectories(templateDirectory);

            foreach (var directory in directories)
            {
                string originalName = Path.GetFileName(directory); // 元のディレクトリ名
                string modifiedName = originalName;
                string fullPath = Path.Combine(templateDirectory, originalName);
                // リストに追加
                directoryList.Add(new DirectoryInfoItem
                {
                    FullPath = fullPath,
                    DirectoryName = modifiedName,
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
