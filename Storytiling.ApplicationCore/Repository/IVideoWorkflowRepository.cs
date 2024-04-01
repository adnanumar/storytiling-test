using Storytiling.ApplicationCore.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storytiling.ApplicationCore.Repository
{
    public interface IVideoWorkflowRepository
    {
        Task<IEnumerable<VideoWorkflowDTO>> GetAllAsync(CancellationToken cancellationToken);
        Task<VideoWorkflowDTO?> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<VideoWorkflowDTO?> GetByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken);
        Task<VideoWorkflowDTO> CreateAsync(VideoWorkflowDTO videoWorkflowDTO, CancellationToken cancellationToken);
        Task<VideoWorkflowDTO?> UpdateAsync(VideoWorkflowDTO videoWorkflowDTO, CancellationToken cancellationToken);
    }
}
