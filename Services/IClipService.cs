namespace YoutubeClipping.Services
{
    public interface IClipService
    {
        // Store a clip and return an id
        string StoreClip(ClipInfo clip);

        // Generate a shareable URL for a stored clip id
        string GenerateShareUrl(string clipId);

        // Optionally retrieve a stored clip
        ClipInfo? GetClip(string clipId);

        // Generate embed URL for a clip
        string GetEmbedUrl(ClipInfo clip);
    }

    public record ClipInfo
    {
        public string VideoId { get; init; } = string.Empty;
        public double StartSeconds { get; init; }
        public double EndSeconds { get; init; }
        public string? Title { get; init; }
        public DateTime CreatedUtc { get; init; } = DateTime.UtcNow;
    }
}
