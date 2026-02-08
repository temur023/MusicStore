using System.Runtime.InteropServices.JavaScript;
using MusicStore.Dtos;

namespace MusicStore.Services;

public interface IMediaService
{
    byte[] GenerateCover(MediaDto dto);
    byte[] GenerateAudio(long seed);
}