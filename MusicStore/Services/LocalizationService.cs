using System.Text.Json;
using MusicStore.Entities;

namespace MusicStore.Services;

public class LocalizationService : ILocalizationService
{
    private readonly Dictionary<string, LocaleData> _locales = new();

    public LocalizationService()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Resources");
        if (!Directory.Exists(path)) return;

        foreach (var file in Directory.GetFiles(path, "*.json"))
        {
            var filename = Path.GetFileNameWithoutExtension(file);
            var json = File.ReadAllText(file);
            var data = JsonSerializer.Deserialize<LocaleData>(json, new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            });

            if (data != null) _locales[filename] = data;
        }
    }

    public LocaleData GetLocale(string region) => 
        _locales.TryGetValue(region, out var data) ? data : _locales.Values.First();
    
    public string GetValue(long seed, List<string> list)
    {
        if (list == null || list.Count == 0) return "Unknown";
        int index = (int)(Math.Abs(seed) % list.Count);
        return list[index];
    }
}