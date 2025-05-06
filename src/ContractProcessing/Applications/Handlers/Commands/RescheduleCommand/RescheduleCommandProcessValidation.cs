using FluentValidation;

namespace Warehouse.ContractProcessing.Applications.Handlers.Commands.RescheduleCommand
{
    /// <summary>
    /// Валидация команды <see cref="RescheduleCommand"/>.
    /// </summary>
    public sealed class RescheduleCommandProcessValidation
        : AbstractValidator<RescheduleCommand>
    {
        /// <summary>
        /// Конструктор валидации команды <see cref="RescheduleCommand"/>.
        /// </summary>
        public RescheduleCommandProcessValidation()
        {
            RuleFor(x => x.ContractId)
                .NotEmpty().WithMessage("ContractId не может быть пустым.");

            RuleFor(x => x.NewDate)
                .NotEmpty().WithMessage("NewDate не может быть пустым.")
                .Must(d => d != default).WithMessage("NewDate должен быть валидной датой.");

            RuleFor(x => x.CorrelationId)
                .NotEmpty().WithMessage("CorrelationId не может быть пустым.");
        }
    }
}
