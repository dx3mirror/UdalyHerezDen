using FluentValidation;

namespace Warehouse.ContractProcessing.Applications.Handlers.Commands.CompleteCommand
{
    /// <summary>
    /// Валидация команды <see cref="CompleteCommand"/>.
    /// </summary>
    public sealed class CompleteCommandProcessValidation
        : AbstractValidator<CompleteCommand>
    {
        /// <summary>
        /// Конструктор валидации команды <see cref="CompleteCommand"/>.
        /// </summary>
        public CompleteCommandProcessValidation()
        {
            RuleFor(x => x.ContractId)
                .NotEmpty().WithMessage("ContractId не может быть пустым.");

            RuleFor(x => x.CorrelationId)
                .NotEmpty().WithMessage("CorrelationId не может быть пустым.");
        }
    }
}
