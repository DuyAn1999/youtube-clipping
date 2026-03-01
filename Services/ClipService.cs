using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

namespace YoutubeClipping.Services
{
    public class ClipService : IClipService
    {
        // In-memory store for clips. Replace with DB for production.
        private readonly ConcurrentDictionary<string, ClipInfo> _store = new();

        public string StoreClip(ClipInfo clip)
        {
            // generate a short id
            var id = GenerateId(8);
            _store[id] = clip with { CreatedUtc = DateTime.UtcNow };
            return id;
        }

        public ClipInfo? GetClip(string clipId)
        {
            if (string.IsNullOrWhiteSpace(clipId)) return null;
            _store.TryGetValue(clipId, out var clip);
            return clip;
        }

        public string GenerateShareUrl(string clipId)
        {
            if (string.IsNullOrWhiteSpace(clipId)) return string.Empty;
            // In real app, build absolute url based on request. Here return a relative url.
            return $"/clip/{clipId}";
        }

        public string GetEmbedUrl(ClipInfo clip)
        {
            if (string.IsNullOrWhiteSpace(clip?.VideoId)) return string.Empty;
            // Build YouTube embed URL with start, end, and player options
            var baseUrl = $"https://www.youtube.com/embed/{clip.VideoId}";
            var start = Math.Floor(clip.StartSeconds);
            var end = Math.Ceiling(clip.EndSeconds);
            return $"{baseUrl}?start={start}&end={end}&controls=0&rel=0&modestbranding=1";
        }

        private static string GenerateId(int length)
        {
            // URL-safe base64
            var bytes = new byte[length];
            RandomNumberGenerator.Fill(bytes);
            return Convert.ToBase64String(bytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .Substring(0, length);
        }
    }
}
