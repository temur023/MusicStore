using Microsoft.AspNetCore.Mvc;
using MusicStore.Dtos;
using MusicStore.Services;

namespace MusicStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GeneratorController(IGeneratorService service):ControllerBase
{
    [HttpGet("get-songs")]
    public IActionResult GetSongs([FromQuery] GetSongDto dto)
    {
        var response = service.GetSongs(dto);
        return Ok(response);
    }
}