using FluentValidation;
using KmLog.Server.Blazor.Validation.Models;

namespace KmLog.Server.Blazor.Validation.Validators
{
    public class JoinGroupValidator : AbstractValidator<GroupModel>
    {
        public JoinGroupValidator()
        {
            RuleFor(g => g.Id)
                .NotEmpty().WithMessage("Select a Group to join!");
        }
    }
}
