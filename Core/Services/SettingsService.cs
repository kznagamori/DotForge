using System;
using System.IO;
using Tomlyn;
using Tomlyn.Model;
using DotForge;
using DotForge.Models;

namespace DotForge.Services;
public class SettingsService : ISettingsService
{
    public Settings Settings { get; private set; } = new Settings();

    public SettingsService()
    {

    }
    public SettingsService(string settingsFilePath)
    {
        if (!File.Exists(settingsFilePath))
        {
            throw new FileNotFoundException("設定ファイルが見つかりません。", settingsFilePath);
        }
        // TomlModel を AppCommonData に変換
        Settings.Toml = Utilities.TOMLParser.LoadSettings(settingsFilePath);
    }
}
