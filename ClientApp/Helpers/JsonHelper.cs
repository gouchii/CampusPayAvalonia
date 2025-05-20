using System.Text.Json;
using ClientApp.Services;

namespace ClientApp.Helpers;

public static class JsonHelper
{
    public static void SetJson<T>(this SettingsService settings, string key, T value)
    {
        var json = JsonSerializer.Serialize(value);
        settings.SetSetting(key, json);
    }

    public static T? GetJson<T>(this SettingsService settings, string key)
    {
        var json = settings.GetSetting(key, string.Empty);
        return string.IsNullOrWhiteSpace(json) ? default : JsonSerializer.Deserialize<T>(json);
    }
}