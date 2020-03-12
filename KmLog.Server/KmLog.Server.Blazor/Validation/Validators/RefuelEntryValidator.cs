using FluentValidation;
using KmLog.Server.Blazor.Validation.Models;

namespace KmLog.Server.Blazor.Validation.Validators
{
    public class RefuelEntryValidator : AbstractValidator<RefuelEntryModel>
    {
        public RefuelEntryValidator()
        {
            RuleFor(re => re.CarId).NotEmpty();
            RuleFor(re => re.Date).NotEmpty();
            RuleFor(re => re.Distance).GreaterThan(0);
            When(re => re.LatestTotalDistance.HasValue, () =>
                RuleFor(re => re.TotalDistance)
                    .GreaterThan(re => re.LatestTotalDistance)
                    .WithMessage(re => $"Total Distance has to be greater than last Total Distance ({re.LatestTotalDistance} km)")
            ).Otherwise(() =>
                RuleFor(re => re.TotalDistance).GreaterThan(0)
            );
            RuleFor(re => re.Amount).GreaterThan(0);
            RuleFor(re => re.Cost).GreaterThan(0);
            RuleFor(re => re.PricePerLiter).GreaterThan(0);
        }
    }
}
