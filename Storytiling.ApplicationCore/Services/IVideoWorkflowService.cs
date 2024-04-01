using Storytiling.ApplicationCore.DTOs;
using Storytiling.ApplicationCore.Requests;

namespace Storytiling.ApplicationCore.Services
{
    public interface IVideoWorkflowService
    {
        Task<VideoWorkflowDTO?> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<VideoWorkflowDTO>> GetAllAsync(CancellationToken cancellationToken);
        Task<VideoWorkflowDTO?> GetVideoWorkflowByEmployeeIdAsync(int id, CancellationToken cancellationToken);
        Task<VideoWorkflowDTO> CreateVideoWorkflowAsync(CreateVideoWorkflowRequest videoWorkflowDTO, CancellationToken cancellationToken);
        Task<VideoWorkflowDTO> InviteContributerAsync(Guid videoWorkflowId, int ContributerId, CancellationToken cancellationToken);
        Task<VideoWorkflowDTO> AcceptInvitationAsync(Guid videoWorkflowId, int ContributerId, CancellationToken cancellationToken);
        Task<VideoWorkflowDTO> SubmitContributionAsync(Guid videoWorkflowId, SubmitContributionRequest request, CancellationToken cancellationToken);
    }
}
