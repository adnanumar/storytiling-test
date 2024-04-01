using Storytiling.ApplicationCore.DTOs;
using Storytiling.ApplicationCore.Repository;
using Storytiling.ApplicationCore.Requests;

namespace Storytiling.ApplicationCore.Services
{
    public class VideoWorkflowService : IVideoWorkflowService
    {
        private readonly IVideoWorkflowRepository _videoWorkflowRepo;
        public VideoWorkflowService(IVideoWorkflowRepository videoWorkflowRepo)
        {
            _videoWorkflowRepo = videoWorkflowRepo;
        }
        /// <summary>
        /// This method returns Video workflow by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<VideoWorkflowDTO?> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var videoWorkflow = await _videoWorkflowRepo.GetAsync(id, cancellationToken);
            return videoWorkflow;
        }
        /// <summary>
        /// This method returns list of all Video workflows
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<VideoWorkflowDTO>> GetAllAsync(CancellationToken cancellationToken)
        {
            var videoWorkflow = await _videoWorkflowRepo.GetAllAsync(cancellationToken);
            return videoWorkflow;
        }
        /// <summary>
        /// This method returns Video workflow by employee id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<VideoWorkflowDTO?> GetVideoWorkflowByEmployeeIdAsync(int id, CancellationToken cancellationToken)
        {
            var videoWorkflow = await _videoWorkflowRepo.GetByEmployeeIdAsync(id, cancellationToken);

            return videoWorkflow;
        }
        /// <summary>
        /// This method create new Video workflow for an employee
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<VideoWorkflowDTO> CreateVideoWorkflowAsync(CreateVideoWorkflowRequest request, CancellationToken cancellationToken)
        {
            var videoWorkflow = new VideoWorkflowDTO
            {
                Title = request.Title,
                SubmissionDate = request.SumissionDate,
                EmployeeId = request.EmployeeId
            };
            videoWorkflow.VideoWorkflowId = Guid.NewGuid();
            videoWorkflow.Status = VideoWorkflowStatus.NotStarted;
            videoWorkflow.ContributionsMade = 0;
            videoWorkflow.ContributionsPending = 0;
            var response = await _videoWorkflowRepo.CreateAsync(videoWorkflow, cancellationToken);

            return response;
        }
        /// <summary>
        /// This method send invitation to the contributor
        /// </summary>
        /// <param name="videoWorkflowId"></param>
        /// <param name="ContributerId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<VideoWorkflowDTO> InviteContributerAsync(Guid videoWorkflowId, int ContributerId, CancellationToken cancellationToken)
        {
            //Get existing contribution
            var videoWorkflow = await _videoWorkflowRepo.GetAsync(videoWorkflowId, cancellationToken);
            if (videoWorkflow is null) throw new ArgumentException("Invalid", "VideoWorkflowId");

            //TODO: Send Invitation Email or Message to Contributor

            //Create contribution with Invited status
            var contribution = new VideoWorkflowContributionDTO
            {
                VideoId = default,
                ContributerId = ContributerId,
                Timestamp = DateTime.UtcNow,
                Status = VideoWorkflowContributionStatus.Invited,
            };
            //Update VideoWorkflow Contributions count
            videoWorkflow.Status = VideoWorkflowStatus.Invited;
            videoWorkflow.Contributions.Add(contribution);
            var response = await _videoWorkflowRepo.UpdateAsync(videoWorkflow, cancellationToken);
            return videoWorkflow;
        }
        /// <summary>
        /// This method accepts invitation for a Video workflwo contribution
        /// </summary>
        /// <param name="videoWorkflowId"></param>
        /// <param name="ContributerId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<VideoWorkflowDTO> AcceptInvitationAsync(Guid videoWorkflowId, int ContributerId, CancellationToken cancellationToken)
        {
            //Get existing contribution
            var videoWorkflow = await _videoWorkflowRepo.GetAsync(videoWorkflowId, cancellationToken);
            if (videoWorkflow is null) throw new ArgumentException("Invalid", "VideoWorkflowId");

            //Create contribution with Invited status
            var contribution = videoWorkflow.Contributions.Single(x => x.ContributerId == ContributerId);
            contribution.Status = VideoWorkflowContributionStatus.InProgress;
            videoWorkflow.ContributionsPending++;
            videoWorkflow.Status = VideoWorkflowStatus.InProgress;
            var response = await _videoWorkflowRepo.UpdateAsync(videoWorkflow, cancellationToken);
            return videoWorkflow;
        }
        /// <summary>
        /// This method submits contribution to Video workflow of contributer
        /// </summary>
        /// <param name="videoWorkflowId"></param>
        /// <param name="ContributerId"></param>
        /// <param name="videoId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<VideoWorkflowDTO> SubmitContributionAsync(Guid videoWorkflowId, SubmitContributionRequest request, CancellationToken cancellationToken)
        {
            //Get existing contribution
            var videoWorkflow = await _videoWorkflowRepo.GetAsync(videoWorkflowId, cancellationToken);
            if (videoWorkflow is null) throw new ArgumentException("Invalid", "VideoWorkflowId");

            //Create contribution with Invited status
            var contribution = videoWorkflow.Contributions.Single(x => x.ContributerId == request.ContributerId);
            contribution.VideoId = request.VideoId;
            contribution.VideoUrl = request.VideoUrl;
            contribution.Status = VideoWorkflowContributionStatus.Completed;
            //Update VideoWorkflow Contributions count
            videoWorkflow.ContributionsPending--;
            videoWorkflow.ContributionsMade++;
            if (videoWorkflow.ContributionsPending == 0)
            {
                videoWorkflow.Status = VideoWorkflowStatus.ReadyToShare;
            }
            var response = await _videoWorkflowRepo.UpdateAsync(videoWorkflow, cancellationToken);
            return videoWorkflow;
        }
    }
}
