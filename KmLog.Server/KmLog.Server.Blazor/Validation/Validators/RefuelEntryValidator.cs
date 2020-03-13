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
            RuleFor(re => re.Amount).GreaterThan(0);
            RuleFor(re => re.Cost).GreaterThan(0);
            RuleFor(re => re.PricePerLiter).GreaterThan(0);

            RuleFor(re => re.Distance)
                .GreaterThan(0)
                .When(re => re.TotalDistance == 0)
                .WithMessage("Distance or Total Distance has to be set.");

            When(re => re.LatestTotalDistance.HasValue, () =>
            {
                // previous total distance exists
                RuleFor(re => re.TotalDistance)
                    .GreaterThan(re => re.LatestTotalDistance)
                    .When(re => re.Distance == 0)
                    .WithMessage(re => $"Total Distance has to be greater than previous Total Distance ({re.LatestTotalDistance} km)");

                When(re => re.TotalDistance > 0 && re.Distance > 0, () =>
                {
                    // difference to previous total distance has to be same as distance
                    RuleFor(re => re.TotalDistance)
                        .Equal(re => re.LatestTotalDistance.Value + re.Distance)
                        .WithMessage(re => $"Mismatch with Distance ({re.LatestTotalDistance.Value + re.Distance} Difference)");
                    RuleFor(re => re.Distance)
                        .Equal(re => re.TotalDistance - re.LatestTotalDistance.Value)
                        .WithMessage(re => $"Mismatch with Total Distance ({re.TotalDistance - re.LatestTotalDistance.Value} Difference)");
                });
            }).Otherwise(() => {
                RuleFor(re => re.TotalDistance)
                    .GreaterThan(0)
                    .When(re => re.Distance == 0)
                    .WithMessage("Total Distance or Distance has to be set.");
            });
        }
    }
}
