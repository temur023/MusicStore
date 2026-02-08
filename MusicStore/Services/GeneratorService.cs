using Bogus;
using MusicStore.Dtos;
using MusicStore.Entities;

namespace MusicStore.Services;

public class GeneratorService(ILocalizationService _loc) : IGeneratorService
{
    public List<SongDto> GetSongs(GetSongDto dto)
    {
        var songs = new List<SongDto>();
        var locale = _loc.GetLocale(dto.Region);
        int startRow = (dto.Page - 1) * dto.Limit;

        for (int i = 0; i < dto.Limit; i++)
        {
            int currentIndex = startRow + i + 1;

            long songSeed = dto.Seed + currentIndex;
            
            var genre  = _loc.GetValue(songSeed,     locale.Genres);
            var adj    = _loc.GetValue(songSeed + 10, locale.Adjectives); 
            var noun   = _loc.GetValue(songSeed + 20, locale.Nouns);      
            var prefix = _loc.GetValue(songSeed + 30, locale.ArtistPrefixes);
            var name   = _loc.GetValue(songSeed + 40, locale.Names);
            var review   = _loc.GetValue(songSeed + 50, locale.Reviews);
            var song = new SongDto
            {
                Index = currentIndex,
                Genre = genre,
                Title = $"{adj} {noun}",
                Review = review,
                Artist = (songSeed % 2 == 0) ? $"{prefix} {name}" : name,
                Album = (songSeed % 5 == 0) ? "Single" : $"{noun} Evolution",
                
                CoverUrl = $"/api/media/cover?seed={songSeed}&title={Uri.EscapeDataString($"{adj} {noun}")}",
                AudioUrl = $"/api/media/audio?seed={songSeed}"
            };


            song.Likes = CalculateLikes(songSeed, dto.Likes);

            songs.Add(song);
        }
        return songs;
    }

    private int CalculateLikes(long seed, double average)
    {
        int baseLikes = (int)Math.Floor(average);
        double fraction = average - baseLikes;
        
        var rand = new Random((int)(seed % int.MaxValue));
        return rand.NextDouble() < fraction ? baseLikes + 1 : baseLikes;
    }
}