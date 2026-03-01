using Microsoft.AspNetCore.Mvc;
using YoutubeClipping.Services;

namespace YoutubeClipping.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClipController : ControllerBase
    {
        private readonly IClipService _clipService;

        public ClipController(IClipService clipService)
        {
            _clipService = clipService;
        }

        public record CreateClipRequest(string VideoId, double Start, double End);
        public record CreateClipResponse(string ClipId, string ShareUrl);

        [HttpPost]
        public IActionResult Create([FromBody] CreateClipRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.VideoId) || req.End <= req.Start) return BadRequest();
            var clip = new ClipInfo { VideoId = req.VideoId, StartSeconds = req.Start, EndSeconds = req.End };
            var id = _clipService.StoreClip(clip);
            var share = _clipService.GenerateShareUrl(id);
            return Ok(new CreateClipResponse(id, share));
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var clip = _clipService.GetClip(id);
            if (clip == null) return NotFound();
            return Ok(clip);
        }
    }
}
