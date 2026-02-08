using MusicStore.Dtos;

namespace MusicStore.Services;

public interface IGeneratorService
{
    List<SongDto> GetSongs(GetSongDto dto);
}