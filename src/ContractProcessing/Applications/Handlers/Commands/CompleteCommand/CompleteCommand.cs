using Warehouse.ContractProcessing.Applications.Handlers.Abstracts;

namespace Warehouse.ContractProcessing.Applications.Handlers.Commands.CompleteCommand
{
    /// <summary>
    /// Команда на завершение выгрузки (перевод контракта в Completed).
    /// </summary>
    public sealed record CompleteCommand(
        Guid ContractId
    ) : Command;
}
