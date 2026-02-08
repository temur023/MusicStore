using Microsoft.AspNetCore.Mvc;
using MusicStore.Dtos;
using MusicStore.Services;

namespace MusicStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MediaController(IMediaService service):ControllerBase
{
        [HttpGet("audio")] 
        public IActionResult GenerateAudio([FromQuery] long seed)
        {
            var bytes = service.GenerateAudio(seed);
            return File(bytes, "audio/wav");
        }
        
        [HttpGet("cover")]
        public IActionResult GenerateCover([FromQuery] MediaDto dto)
        {
            var bytes = service.GenerateCover(dto);
            return File(bytes, "image/png");
        }
    }
