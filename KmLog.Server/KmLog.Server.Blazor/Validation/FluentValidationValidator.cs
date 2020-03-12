using System;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace KmLog.Server.Blazor.Validation
{
    public class FluentValidationValidator : ComponentBase
    {
        [CascadingParameter]
        private EditContext EditContext { get; set; }

        [Parameter]
        public Type ValidatorType { get; set; }

        private IValidator Validator;
        private ValidationMessageStore ValidationMessageStore;

        [Inject]
        private IServiceProvider ServiceProvider { get; set; }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            // Keep a reference to the original values so we can check if they have changed
            EditContext previousEditContext = EditContext;
            Type previousValidatorType = ValidatorType;
            await base.SetParametersAsync(parameters);
            if (EditContext == null)
                throw new NullReferenceException($"{nameof(FluentValidationValidator)} must be placed within an {nameof(EditForm)}");

            if (ValidatorType == null)
                throw new NullReferenceException($"{nameof(ValidatorType)} must be specified.");

            if (!typeof(IValidator).IsAssignableFrom(ValidatorType))
                throw new ArgumentException($"{ValidatorType.Name} must implement {typeof(IValidator).FullName}");

            if (ValidatorType != previousValidatorType)
                ValidatorTypeChanged();

            if (EditContext != previousEditContext)
                EditContextChanged();
        }

        private void ValidatorTypeChanged()
        {
            Validator = (IValidator)ServiceProvider.GetService(ValidatorType);
        }

        private void EditContextChanged()
        {
            ValidationMessageStore = new ValidationMessageStore(EditContext);
            HookUpEditContextEvents();
        }

        private void HookUpEditContextEvents()
        {
            EditContext.OnValidationRequested += ValidationRequested;
            EditContext.OnFieldChanged += FieldChanged;
        }

        private async void ValidationRequested(object sender, ValidationRequestedEventArgs args)
        {
            ValidationMessageStore.Clear();
            var result = await Validator.ValidateAsync(EditContext.Model);
            AddValidationResult(EditContext.Model, result);
        }

        private void AddValidationResult(object model, ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                var fieldIdentifier = new FieldIdentifier(model, error.PropertyName);
                ValidationMessageStore.Add(fieldIdentifier, error.ErrorMessage);
            }
            EditContext.NotifyValidationStateChanged();
        }

        private async void FieldChanged(object sender, FieldChangedEventArgs args)
        {
            var fieldIdentifier = args.FieldIdentifier;
            ValidationMessageStore.Clear(fieldIdentifier);

            var propertiesToValidate = new string[] { fieldIdentifier.FieldName };
            var fluentValidationContext = new ValidationContext(
                fieldIdentifier.Model,
                new PropertyChain(),
                new MemberNameValidatorSelector(propertiesToValidate)
            );

            var result = await Validator.ValidateAsync(fluentValidationContext);
            AddValidationResult(fieldIdentifier.Model, result);
        }
    }
}
