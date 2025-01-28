using Tomlyn;
using Tomlyn.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace DotForge;

static class TomHelper
{
    /// <summary>
    /// オブジェクトを TOML 文字列に変換します。
    /// </summary>
    public static string ObjectToToml(object obj)
    {
        return Toml.FromModel(obj);
    }

    /// <summary>
    /// TOML 文字列を Dictionary<string, object> に変換します。
    /// </summary>
    public static Dictionary<string, object> TomlToDictionary(string toml)
    {
        var parseResult = Toml.Parse(toml);
        if (parseResult.HasErrors)
        {
            throw new FormatException("TOMLのパースに失敗しました。エラー: " + string.Join(", ", parseResult.Diagnostics));
        }

        var tomlTable = parseResult.ToModel() as TomlTable;
        if (tomlTable == null)
        {
            throw new InvalidDataException("TOMLデータがテーブル形式ではありません。");
        }

        return TomlTableToDictionary(tomlTable);
    }

    /// <summary>
    /// TOML ファイルから Dictionary<string, object> を読み込みます。
    /// </summary>
    public static Dictionary<string, object> LoadSettings(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("設定ファイルが見つかりません", filePath);
        }

        string tomlString = File.ReadAllText(filePath);
        return TomlToDictionary(tomlString);
    }

    /// <summary>
    /// Dictionary<string, object> の内容を再帰的に表示します（デバッグ用）。
    /// </summary>
    public static void PrintDictionary(Dictionary<string, object> dictionary, int indent = 0)
    {
        foreach (var kvp in dictionary)
        {
            PrintIndent(indent);
            if (kvp.Value is Dictionary<string, object> innerDict)
            {
                Console.WriteLine($"{kvp.Key}:");
                PrintDictionary(innerDict, indent + 1);
            }
            else if (kvp.Value is List<object> innerList)
            {
                Console.WriteLine($"{kvp.Key}:");
                PrintList(innerList, indent + 1);
            }
            else
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
        }
    }

    /// <summary>
    /// TomlTable を Dictionary<string, object> に変換します。
    /// </summary>
    private static Dictionary<string, object> TomlTableToDictionary(TomlTable tomlTable)
    {
        var dictionary = new Dictionary<string, object>();
        foreach (var item in tomlTable)
        {
            dictionary.Add(item.Key, ConvertTomlObject(item.Value));
        }
        return dictionary;
    }

    /// <summary>
    /// TomlArray の内容を再帰的に表示します（デバッグ用）。
    /// </summary>
    private static void PrintList(List<object> list, int indent = 0)
    {
        for (int i = 0; i < list.Count; i++)
        {
            PrintIndent(indent);
            if (list[i] is Dictionary<string, object> innerDict)
            {
                Console.WriteLine($"[{i}]:");
                PrintDictionary(innerDict, indent + 1);
            }
            else if (list[i] is List<object> innerList)
            {
                Console.WriteLine($"[{i}]:");
                PrintList(innerList, indent + 1);
            }
            else
            {
                Console.WriteLine($"[{i}]: {list[i]}");
            }
        }
    }

    /// <summary>
    /// インデントを表示します。
    /// </summary>
    private static void PrintIndent(int indent)
    {
        for (int i = 0; i < indent; i++)
        {
            Console.Write("  ");
        }
    }

    /// <summary>
    /// Toml オブジェクトを適切な型に変換します。
    /// </summary>
    private static object ConvertTomlObject(object tomlObject)
    {
        if (tomlObject is TomlTable tomlTable)
        {
            return TomlTableToDictionary(tomlTable);
        }
        else if (tomlObject is TomlArray tomlArray)
        {
            var list = new List<object>();
            foreach (var item in tomlArray)
            {
                if (item is not null)
                {
                    list.Add(ConvertTomlObject(item));
                }
            }
            return list;
        }
        else
        {
            return tomlObject;
        }
    }
}
