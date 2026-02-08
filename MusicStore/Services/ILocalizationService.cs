using System.Runtime.InteropServices.JavaScript;
using MusicStore.Entities;

namespace MusicStore.Services;

public interface ILocalizationService
{
    LocaleData GetLocale(string region);
    string GetValue(long seed, List<string> list);
}