using Warehouse.ContractProcessing.Applications.Handlers.Abstracts;

namespace Warehouse.ContractProcessing.Applications.Handlers.Commands.RescheduleCommand
{
    /// <summary>
    /// Команда на перенос даты выгрузки.
    /// </summary>
    public sealed record RescheduleCommand(
        Guid ContractId,
        DateTime NewDate
    ) : Command;
}
