using System.Runtime.InteropServices.JavaScript;

namespace MusicStore.Dtos;

public class MediaDto
{
    public long Seed { get; set; }
    public string Title { get; set; } = "";
    public string Artist { get; set; } = "";
}