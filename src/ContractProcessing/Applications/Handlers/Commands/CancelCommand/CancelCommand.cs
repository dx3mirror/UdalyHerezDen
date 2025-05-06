using Warehouse.ContractProcessing.Applications.Handlers.Abstracts;

namespace Warehouse.ContractProcessing.Applications.Handlers.Commands.CancelCommand
{
    /// <summary>
    /// Команда на отмену выгрузки (перевод контракта в Cancelled).
    /// </summary>
    public sealed record CancelCommand(
        Guid ContractId
    ) : Command;
}
