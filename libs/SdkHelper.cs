using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace DotForge;
public static class SdkHelper
{
    public static List<string> GetInstalledSdkVersions()
    {
        HashSet<string> sdkVersions = new HashSet<string>(); // Use HashSet to avoid duplicates

        // Registry paths to check
        string[] registryKeyPaths = {
            @"SOFTWARE\Microsoft\Microsoft SDKs\Windows\v10.0",
            @"SOFTWARE\Wow6432Node\Microsoft\Microsoft SDKs\Windows\v10.0"
        };

        foreach (string registryKeyPath in registryKeyPaths)
        {
            using (RegistryKey? key = Registry.LocalMachine.OpenSubKey(registryKeyPath))
            {
                if (key != null)
                {
                    // Get InstallationFolder (if exists)
                    string? installationFolders = key.GetValue("InstallationFolder", "").ToString();
                    if (string.IsNullOrEmpty(installationFolders))
                    {
                        continue;
                    }
                    foreach (string installationFolder in installationFolders.Split(';'))
                    {
                        string? productVersion = key.GetValue("ProductVersion", "").ToString();
                        if (!string.IsNullOrEmpty(productVersion))
                        {
                            sdkVersions.Add(NormalizeVersion(productVersion));
                        }
                    }

                    // Get ProductVersion from subkeys
                    foreach (string subkeyName in key.GetSubKeyNames())
                    {
                        using (RegistryKey? subkey = key.OpenSubKey(subkeyName))
                        {
                            if (subkey != null)
                            {
                                string? productVersion = subkey.GetValue("ProductVersion", "").ToString();
                                if (!string.IsNullOrEmpty(productVersion))
                                {
                                    sdkVersions.Add(NormalizeVersion(productVersion));
                                }
                            }
                        }
                    }
                }
            }
        }

        return sdkVersions.OrderBy(v => v).ToList(); // Sort and return as List
    }
    public static List<string> GetInstalledSdkVersionsFromInstalledRoots()
    {
        HashSet<string> sdkVersions = new HashSet<string>();
        string registryKeyPath = @"SOFTWARE\WOW6432Node\Microsoft\Windows Kits\Installed Roots";

        using (RegistryKey? key = Registry.LocalMachine.OpenSubKey(registryKeyPath))
        {
            if (key != null)
            {
                foreach (string subkeyName in key.GetSubKeyNames())
                {
                    if (subkeyName.StartsWith("10.")) // Check if the subkey name is a version number
                    {
                        using (RegistryKey? subkey = key.OpenSubKey(subkeyName))
                        {
                            if (subkey != null)
                            {
                                sdkVersions.Add(NormalizeVersion(subkeyName)); // サブキー名をバージョンとして追加
                            }
                        }
                    }
                }
            }
        }
        return sdkVersions.OrderBy(v => v).ToList();
    }
    public static List<string> GetInstalledSdkVersionsCombined()
    {
        HashSet<string> sdkVersions = new HashSet<string>();

        // Method 1: Get versions from SOFTWARE\Microsoft\Microsoft SDKs\Windows\v10.0
        foreach (string version in GetInstalledSdkVersions()) // Previous method for v10.0
        {
            sdkVersions.Add(version);
        }

        // Method 2: Get versions from Installed Roots
        foreach (string version in GetInstalledSdkVersionsFromInstalledRoots())
        {
            sdkVersions.Add(version);
        }

        // Method 3: Scan Program Files\WindowsApps for UWP SDKs
        // ... (Implementation for scanning WindowsApps) ...

        return sdkVersions.OrderBy(v => v).ToList();
    }
    // バージョン番号の末尾の .0 を補完して正規化するヘルパーメソッド
    private static string NormalizeVersion(string version)
    {
        string[] parts = version.Split('.');
        if (parts.Length == 3)
        {
            return version + ".0";
        }
        return version;
    }
}
