using FluentValidation;
using FluentValidation.Validators;
using KmLog.Server.Blazor.Validation.Models;

namespace KmLog.Server.Blazor.Validation.Validators
{
    public class ImportValidator : AbstractValidator<ImportModel>
    {
        public ImportValidator()
        {
            RuleFor(i => i.DateColumn)
                .Must(IsColumnUnique).WithMessage("Column has to be unique.")
                .NotEmpty().WithMessage("Column must have value.");
            RuleFor(i => i.CostColumn)
                .Must(IsColumnUnique).WithMessage("Column has to be unique.")
                .NotEmpty().WithMessage("Column must have value.");
            RuleFor(i => i.AmountColumn)
                .Must(IsColumnUnique).WithMessage("Column has to be unique.")
                .NotEmpty().WithMessage("Column must have value.");
            RuleFor(i => i.PricePerLiterColumn)
                .Must(IsColumnUnique).WithMessage("Column has to be unique.")
                .NotEmpty().WithMessage("Column must have value.");
            RuleFor(i => i.TankStatusColumn)
                .Must(IsColumnUnique).WithMessage("Column has to be unique.")
                .NotEmpty().WithMessage("Column must have value.");
            RuleFor(i => i.TotalDistanceColumn)
                .Must(IsColumnUnique).WithMessage("Column has to be unique.")
                .NotEmpty().WithMessage("Column must have value.");

            RuleFor(i => i.LicensePlate)
                .Matches("^[a-zA-Z]{1,2}-[a-zA-Z0-9]{4,6}$").WithMessage("Enter a valid License plate. (#[#]-####)")
                .NotEmpty().WithMessage("License plate must have value."); ;
        }

        private bool IsColumnUnique(ImportModel importModel, string newValue, PropertyValidatorContext context)
        {
            var properties = typeof(ImportModel).GetProperties();
            foreach (var property in properties)
            {
                if (property.Name == context.PropertyName || property.Name == nameof(ImportModel.LicensePlate))
                {
                    continue;
                }
                var value = (string)property.GetValue(importModel);
                if (value == newValue)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
