using FluentValidation;

namespace Storytiling.ApplicationCore.Requests
{
    public class AcceptContributionRequest
    {
        public int ContributerId { get; set; }
    }
    public class AcceptContributionRequestValidator : AbstractValidator<AcceptContributionRequest>
    {
        public AcceptContributionRequestValidator()
        {
            RuleFor(x => x.ContributerId).NotEmpty();
        }
    }
}
