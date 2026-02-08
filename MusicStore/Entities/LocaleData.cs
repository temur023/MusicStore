namespace MusicStore.Entities;

public class LocaleData
{
    public List<string> Genres { get; set; } = new();
    public List<string> Adjectives { get; set; } = new();
    public List<string> Nouns { get; set; } = new();
    public List<string> ArtistPrefixes { get; set; } = new();
    public List<string> Names { get; set; } = new();
    public List<string> Reviews { get; set; }
}