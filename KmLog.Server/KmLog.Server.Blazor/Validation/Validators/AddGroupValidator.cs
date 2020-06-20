using FluentValidation;
using KmLog.Server.Blazor.Validation.Models;

namespace KmLog.Server.Blazor.Validation.Validators
{
    public class AddGroupValidator : AbstractValidator<GroupModel>
    {
        public AddGroupValidator()
        {
            RuleFor(g => g.Name)
                .NotEmpty().WithMessage("Group must have a Name!");
        }
    }
}
