namespace Storytiling.ApplicationCore.DTOs
{
    public class VideoWorkflowContributionDTO
    {
        public int ContributerId { get; set; }
        public int VideoId { get; set; }
        public string VideoUrl { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public VideoWorkflowContributionStatus Status { get; set; }
    }
}
