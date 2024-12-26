using System;
using System.IO;

namespace DotForge;
public static class DirectoryCopyHelper
{
    /// <summary>
    /// sourceDirの内容（ファイル・サブディレクトリ）をdestDirにコピーする。
    /// サブディレクトリも含めて再帰的にコピーします。
    /// </summary>
    /// <param name="sourceDir">コピー元ディレクトリ</param>
    /// <param name="destDir">コピー先ディレクトリ</param>
    /// <param name="overwrite">ファイルが存在した場合に上書きするかどうか</param>
    public static void CopyDirectory(string sourceDir, string destDir, bool overwrite = false)
    {
        if (!Directory.Exists(sourceDir))
        {
            throw new DirectoryNotFoundException($"コピー元ディレクトリが見つかりません: {sourceDir}");
        }

        // コピー先ディレクトリを作成 (既に存在していてもOK)
        Directory.CreateDirectory(destDir);

        // コピー元ディレクトリにある全てのファイルをコピー
        foreach (var filePath in Directory.GetFiles(sourceDir))
        {
            string fileName = Path.GetFileName(filePath);
            string destFilePath = Path.Combine(destDir, fileName);

            // ファイルコピー (既存ファイルを上書きする場合は 'overwrite = true' )
            File.Copy(filePath, destFilePath, overwrite);
        }

        // サブディレクトリを再帰的にコピー
        foreach (var subDirPath in Directory.GetDirectories(sourceDir))
        {
            string subDirName = Path.GetFileName(subDirPath);
            string destSubDirPath = Path.Combine(destDir, subDirName);

            // 再帰呼び出し
            CopyDirectory(subDirPath, destSubDirPath, overwrite);
        }
    }
}
