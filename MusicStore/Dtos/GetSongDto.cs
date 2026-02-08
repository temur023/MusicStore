using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace MusicStore.Dtos;

public class GetSongDto
{
    public string Region { get; set; }
    public long Seed { get; set; }
    
    [Range(0,10)]
    public double Likes { get; set; }
    public int Page { get; set; }
    public int Limit { get; set; }
}