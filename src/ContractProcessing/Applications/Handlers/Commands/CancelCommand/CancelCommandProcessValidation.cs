using FluentValidation;

namespace Warehouse.ContractProcessing.Applications.Handlers.Commands.CancelCommand
{
    /// <summary>
    /// Валидация команды <see cref="CancelCommand"/>.
    /// </summary>
    public sealed class CancelCommandProcessValidation
        : AbstractValidator<CancelCommand>
    {
        /// <summary>
        /// Конструктор валидации команды <see cref="CancelCommand"/>.
        /// </summary>
        public CancelCommandProcessValidation()
        {
            RuleFor(x => x.ContractId)
                .NotEmpty().WithMessage("ContractId не может быть пустым.");

            RuleFor(x => x.CorrelationId)
                .NotEmpty().WithMessage("CorrelationId не может быть пустым.");
        }
    }
}
