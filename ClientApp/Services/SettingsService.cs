using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Avalonia.Media;

namespace ClientApp.Services
{
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

            // Load settings at startup

        }

        public T GetSetting<T>(string key, T defaultValue)
        {
            if (_settings.TryGetValue(key, out var value))
            {
                // If it's already deserialized, return it directly
                if (value is T typedValue)
                {
                    Console.WriteLine($"Trying to get setting: {key} | {typedValue}");
                    return typedValue;
                }

                // If it's a JsonElement, deserialize it
                if (value is JsonElement element)
                {
                    try
                    {
                        var deserializedValue = JsonSerializer.Deserialize<T>(element.GetRawText()) ?? defaultValue;
                        _settings[key] = deserializedValue; // Cache the deserialized value for future fast access
                        Console.WriteLine($"Trying to get setting: {key} | {deserializedValue}");
                        return deserializedValue;
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Error deserializing setting '{key}': {ex.Message}");
                        return defaultValue;
                    }
                }
            }

            Console.WriteLine($"Trying to get setting: {key} | {defaultValue}");
            return defaultValue;
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
            try
            {
                Console.WriteLine("Saving settings...");
                var json = JsonSerializer.Serialize(_settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_settingsFilePath, json);
                Console.WriteLine("Settings saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving settings: {ex.Message}");
            }
        }

        public void LoadSettings()
        {
            try
            {
                if (File.Exists(_settingsFilePath))
                {
                    var json = File.ReadAllText(_settingsFilePath);
                    var loadedSettings = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

                    if (loadedSettings != null)
                    {
                        foreach (var kvp in loadedSettings)
                        {
                            _settings[kvp.Key] = kvp.Value;
                            Console.WriteLine($"Loaded setting: {kvp.Key} = {kvp.Value}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading settings: {ex.Message}");
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
}
