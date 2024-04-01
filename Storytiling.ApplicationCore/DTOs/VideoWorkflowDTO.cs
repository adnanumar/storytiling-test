namespace Storytiling.ApplicationCore.DTOs
{
    public class VideoWorkflowDTO
    {
        public VideoWorkflowDTO()
        {
            Contributions = new List<VideoWorkflowContributionDTO>();
        }
        public Guid VideoWorkflowId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int EmployeeId { get; set; }
        public int ContributionsPending { get; set; }
        public int ContributionsMade { get; set; }
        public VideoWorkflowStatus Status { get; set; }
        public DateTime SubmissionDate { get; set; }
        public List<VideoWorkflowContributionDTO> Contributions { get; set; }
    }
}
