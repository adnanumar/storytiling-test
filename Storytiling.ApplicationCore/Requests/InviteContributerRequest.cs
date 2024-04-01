using FluentValidation;

namespace Storytiling.ApplicationCore.Requests
{
    public class InviteContributerRequest
    {
        public int ContributerId { get; set; }
    }
    public class InviteContributerRequestValidator : AbstractValidator<InviteContributerRequest>
    {
        public InviteContributerRequestValidator()
        {
            RuleFor(x => x.ContributerId).NotEmpty();
        }
    }
}
