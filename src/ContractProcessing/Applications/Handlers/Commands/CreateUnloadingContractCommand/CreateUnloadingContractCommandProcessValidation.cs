using FluentValidation;

namespace Warehouse.ContractProcessing.Applications.Handlers.Commands.CreateUnloadingContractCommand
{
    /// <summary>
    /// Валидация команды <see cref="CreateUnloadingContractCommand"/>.
    /// </summary>
    public sealed class CreateUnloadingContractCommandProcessValidation
        : AbstractValidator<CreateUnloadingContractCommand>
    {
        /// <summary>
        /// Конструктор валидации команды <see cref="CreateUnloadingContractCommand"/>.
        /// </summary>
        public CreateUnloadingContractCommandProcessValidation()
        {
            RuleFor(x => x.ContractId)
                .NotEmpty().WithMessage("ContractId не может быть пустым.");

            RuleFor(x => x.WarehouseId)
                .NotEmpty().WithMessage("WarehouseId не может быть пустым.");

            RuleFor(x => x.ManagerId)
                .NotEmpty().WithMessage("ManagerId не может быть пустым.");

            RuleFor(x => x.ScheduledFor)
                .NotEmpty().WithMessage("ScheduledFor не может быть пустым.")
                .Must(d => d != default).WithMessage("ScheduledFor должен быть валидной датой.");

            RuleFor(x => x.CorrelationId)
                .NotEmpty().WithMessage("CorrelationId не может быть пустым.");
        }
    }
}
