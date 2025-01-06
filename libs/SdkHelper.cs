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
                            sdkVersions.Add(productVersion);
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
                                    sdkVersions.Add(productVersion);
                                }
                            }
                        }
                    }
                }
            }
        }

        return sdkVersions.OrderBy(v => v).ToList(); // Sort and return as List
    }
}
