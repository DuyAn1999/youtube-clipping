using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using YoutubeClipping.Services;

namespace YoutubeClipping.Pages
{
    public class ClipModel : PageModel
    {
        private readonly IClipService _clipService;

        public ClipModel(IClipService clipService)
        {
            _clipService = clipService;
        }

        public ClipInfo? Clip { get; set; }
        public string EmbedUrl { get; set; } = string.Empty;

        public IActionResult OnGet(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            Clip = _clipService.GetClip(id);
            if (Clip == null)
                return NotFound();

            // Get embed URL from service
            EmbedUrl = _clipService.GetEmbedUrl(Clip);

            return Page();
        }
    }
}
