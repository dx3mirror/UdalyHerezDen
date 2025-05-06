using Warehouse.ContractProcessing.Applications.Handlers.Abstracts;

namespace Warehouse.ContractProcessing.Applications.Handlers.Commands.StartCommand
{
    /// <summary>
    /// Команда на старт выгрузки (перевод контракта в InProgress).
    /// </summary>
    public sealed record StartCommand(
        Guid ContractId
    ) : Command;
}
