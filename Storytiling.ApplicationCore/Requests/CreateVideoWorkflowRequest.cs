using FluentValidation;

namespace Storytiling.ApplicationCore.Requests
{
    public class CreateVideoWorkflowRequest
    {
        public string Title { get; set; } = string.Empty;
        public int EmployeeId { get; set; }
        public DateTime SumissionDate { get; set; }
    }
    public class CreateVideoWorkflowRequestValidator : AbstractValidator<CreateVideoWorkflowRequest>
    {
        public CreateVideoWorkflowRequestValidator()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
            RuleFor(x => x.Title).NotEmpty().Length(5, 50);
            RuleFor(x => x.SumissionDate).NotEmpty().Must(x => x > DateTime.UtcNow).WithMessage("Submission date must be in future date");
        }
    }
}
