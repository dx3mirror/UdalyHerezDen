using FluentValidation;

namespace Warehouse.ContractProcessing.Applications.Handlers.Commands.AddLineCommand
{
    /// <summary>
    /// Валидация команды <see cref="AddLineCommand"/>.
    /// </summary>
    public sealed class AddLineCommandProcessValidation
        : AbstractValidator<AddLineCommand>
    {
        /// <summary>
        /// Конструктор валидации команды <see cref="AddLineCommand"/>.
        /// </summary>
        public AddLineCommandProcessValidation()
        {
            RuleFor(x => x.ContractId)
                .NotEmpty().WithMessage("ContractId не может быть пустым.");

            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("ProductId не может быть пустым.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity должен быть больше нуля.");

            RuleFor(x => x.CorrelationId)
                .NotEmpty().WithMessage("CorrelationId не может быть пустым.");
        }
    }
}
