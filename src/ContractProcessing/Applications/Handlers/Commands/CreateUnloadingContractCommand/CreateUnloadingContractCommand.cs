using Warehouse.ContractProcessing.Applications.Handlers.Abstracts;

namespace Warehouse.ContractProcessing.Applications.Handlers.Commands.CreateUnloadingContractCommand
{
    /// <summary>
    /// Команда на создание контракта выгрузки.
    /// </summary>
    public sealed record CreateUnloadingContractCommand(
        Guid ContractId,
        Guid WarehouseId,
        Guid ManagerId,
        DateTime ScheduledFor
    ) : Command;
}
