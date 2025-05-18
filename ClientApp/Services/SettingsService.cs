using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Avalonia.Media;

namespace ClientApp.Services;

public class SettingsService
{
    private readonly string _settingsFilePath;
    private readonly Dictionary<string, object> _settings;

    public event Action<string, object>? SettingChanged;

    public SettingsService(string appName = "CampusPay")
    {
        var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), appName);
        Directory.CreateDirectory(appDataPath); // Ensure the directory exists
        _settingsFilePath = Path.Combine(appDataPath, "settings.json");
        _settings = new Dictionary<string, object>();
    }

    public T GetSetting<T>(string key, T defaultValue)
    {
        var setting =  _settings.TryGetValue(key, out object? value) && value is JsonElement element
            ? JsonSerializer.Deserialize<T>(element.GetRawText()) ?? defaultValue
            : defaultValue;
        Console.WriteLine($"Trying to get setting : {key} |{ setting}");
        return setting;
    }

    public void SetSetting<T>(string key, T value)
    {
        if (!_settings.ContainsKey(key) || !_settings[key]!.Equals(value))
        {
            _settings[key] = value!;
            SettingChanged?.Invoke(key, value!);
            SaveSettings();
        }
    }

    public void SaveSettings()
    {
        Console.WriteLine("Saving settings....");
        var json = JsonSerializer.Serialize(_settings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_settingsFilePath, json);
    }

    private Dictionary<string, JsonElement> _loadedSettings = new();

    public void LoadSettings()
    {
        if (File.Exists(_settingsFilePath))
        {
            var json = File.ReadAllText(_settingsFilePath);
            var loadedSettings = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

            if (loadedSettings != null)
            {
                _loadedSettings = loadedSettings;


                foreach (var kvp in _loadedSettings)
                {
                    _settings[kvp.Key] = kvp.Value;
                    Console.WriteLine($"Loaded {kvp.Key} , {kvp.Value}");
                }
            }
        }
    }


    public IEnumerable<string> GetAllKeys()
    {
        return _settings.Keys;
    }

    public Color GetColorSetting(string key, Color defaultColor)
    {
        var colorString = GetSetting(key, defaultColor.ToString());
        return Color.TryParse(colorString, out var color) ? color : defaultColor;
    }

    public void SetColorSetting(string key, Color color)
    {
        SetSetting(key, color.ToString());
    }

}