using FluentValidation;
using KmLog.Server.Blazor.Validation.Models;

namespace KmLog.Server.Blazor.Validation.Validators
{
    public class CarValidator : AbstractValidator<CarModel>
    {
        public CarValidator()
        {
            RuleFor(c => c.LicensePlate)
                .Matches("^[a-zA-Z]{1,2}-[a-zA-Z0-9]{4,6}$")
                .WithMessage("Enter a valid License plate. (#[#]-####)");
            RuleFor(c => c.InitialDistance).GreaterThan(0);
        }
    }
}
