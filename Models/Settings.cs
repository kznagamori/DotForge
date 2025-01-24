using System.Collections.Generic;

namespace DotForge.Models;
public class Settings
{
    public Dictionary<string, object> Toml { get; set; }

    public Settings()
    {
        Toml = new Dictionary<string, object>();
    }
}
