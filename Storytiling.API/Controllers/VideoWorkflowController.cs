using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Storytiling.ApplicationCore.Requests;
using Storytiling.ApplicationCore.Services;

namespace Storytiling.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VideoWorkflowController : ControllerBase
    {
        #region Fields
        private readonly ILogger<VideoWorkflowController> _logger;
        private readonly IVideoWorkflowService _videoWorkflowService;
        private readonly IValidator<CreateVideoWorkflowRequest> _createVideoWorkflowValidator;
        private readonly IValidator<SubmitContributionRequest> _addContributionRequestValidator;
        private readonly IValidator<InviteContributerRequest> _inviteContributerRequestValidator;
        private readonly IValidator<AcceptContributionRequest> _acceptContributionRequestValidator;

        #endregion
        #region Constructors
        public VideoWorkflowController(ILogger<VideoWorkflowController> logger,
            IVideoWorkflowService videoWorkflowService,
            IValidator<CreateVideoWorkflowRequest> createVideoWorkflowRequestValidator,
            IValidator<SubmitContributionRequest> addContributionRequestValidator,
            IValidator<InviteContributerRequest> inviteContributerRequestValidator,
            IValidator<AcceptContributionRequest> acceptContributionRequestValidator)
        {
            this._logger = logger;
            this._videoWorkflowService = videoWorkflowService;
            this._createVideoWorkflowValidator = createVideoWorkflowRequestValidator;
            this._addContributionRequestValidator = addContributionRequestValidator;
            this._inviteContributerRequestValidator = inviteContributerRequestValidator;
            _acceptContributionRequestValidator = acceptContributionRequestValidator;
        } 
        #endregion
        #region Video Workflow actions
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            throw new Exception();
            var response = await _videoWorkflowService.GetAllAsync(cancellationToken);
            return Ok(response);
        }
        [HttpGet("Get")]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
        {
            var response = await _videoWorkflowService.GetAsync(id, cancellationToken);
            if (response is null) return NotFound();

            return Ok(response);
        }
        [HttpGet("GetByEmployeeId")]
        public async Task<IActionResult> GetByEmployeeId(int id, CancellationToken cancellationToken)
        {
            var response = await _videoWorkflowService.GetVideoWorkflowByEmployeeIdAsync(id, cancellationToken);
            if (response is null) return NotFound();

            return Ok(response);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateVideoWorkflowRequest request, CancellationToken cancellationToken)
        {
            //Check validation
            var validationResult = _createVideoWorkflowValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(this.ModelState);
                return BadRequest(this.ModelState);
            }
            //Create Video Workflow
            var response = await _videoWorkflowService.CreateVideoWorkflowAsync(request, cancellationToken);
            return Ok(response);
        } 
        #endregion
        #region Video Workflow Contribution actions
        [HttpPost("{videoWorkflowId}/InviteContributer")]
        public async Task<IActionResult> InviteContributer(Guid videoWorkflowId, InviteContributerRequest request, CancellationToken cancellationToken)
        {
            //Check validation
            var validationResult = _inviteContributerRequestValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(this.ModelState);
                return BadRequest(this.ModelState);
            }
            //Send invitation to contributer
            var response = await _videoWorkflowService.InviteContributerAsync(videoWorkflowId, request.ContributerId, cancellationToken);
            return Ok(response);
        }
        [HttpPut("{videoWorkflowId}/AcceptInvitation")]
        public async Task<IActionResult> AcceptInvitation(Guid videoWorkflowId, AcceptContributionRequest request, CancellationToken cancellationToken)
        {
            //Check validation
            var validationResult = _acceptContributionRequestValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(this.ModelState);
                return BadRequest(this.ModelState);
            }
            //Accept invitation for Video contribution
            var response = await _videoWorkflowService.AcceptInvitationAsync(videoWorkflowId, request.ContributerId, cancellationToken);
            return Ok(response);
        }
        [HttpPut("{videoWorkflowId}/SubmitContribution")]
        public async Task<IActionResult> SubmitContribution(Guid videoWorkflowId, SubmitContributionRequest request, CancellationToken cancellationToken)
        {
            //Check validation
            var validationResult = _addContributionRequestValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(this.ModelState);
                return BadRequest(this.ModelState);
            }
            //Submit contribution for Video Workflow
            var response = await _videoWorkflowService.SubmitContributionAsync(videoWorkflowId, request, cancellationToken);
            return Ok(response);
        } 
        #endregion
    }
}
