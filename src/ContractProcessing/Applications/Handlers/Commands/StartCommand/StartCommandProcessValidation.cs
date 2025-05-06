using FluentValidation;

namespace Warehouse.ContractProcessing.Applications.Handlers.Commands.StartCommand
{
    /// <summary>
    /// Валидация команды <see cref="StartCommand"/>.
    /// </summary>
    public sealed class StartCommandProcessValidation
        : AbstractValidator<StartCommand>
    {
        /// <summary>
        /// Конструктор валидации команды <see cref="StartCommand"/>.
        /// </summary>
        public StartCommandProcessValidation()
        {
            RuleFor(x => x.ContractId)
                .NotEmpty().WithMessage("ContractId не может быть пустым.");

            RuleFor(x => x.CorrelationId)
                .NotEmpty().WithMessage("CorrelationId не может быть пустым.");
        }
    }
}
