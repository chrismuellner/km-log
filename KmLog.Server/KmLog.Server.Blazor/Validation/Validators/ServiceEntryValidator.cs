using FluentValidation;
using KmLog.Server.Blazor.Validation.Models;

namespace KmLog.Server.Blazor.Validation.Validators
{
    public class ServiceEntryValidator : AbstractValidator<ServiceEntryModel>
    {
        public ServiceEntryValidator()
        {
            RuleFor(se => se.CarId).NotEmpty();
            RuleFor(se => se.Date).NotEmpty();
            RuleFor(se => se.Cost).NotEmpty();
            RuleFor(se => se.TotalDistance).NotEmpty();
        }
    }
}
