using FluentValidation;

namespace Storytiling.ApplicationCore.Requests
{
    public class SubmitContributionRequest
    {
        public int VideoId { get; set; }
        public string VideoUrl { get; set; } = string.Empty;
        public int ContributerId { get; set; }
    }
    public class SubmitContributionRequestValidator : AbstractValidator<SubmitContributionRequest>
    {
        public SubmitContributionRequestValidator()
        {
            RuleFor(x => x.VideoId).NotEmpty();
            RuleFor(x => x.VideoUrl).NotEmpty();
            RuleFor(x => x.ContributerId).NotEmpty();
        }
    }
}
