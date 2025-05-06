using Warehouse.ContractProcessing.Applications.Handlers.Abstracts;

namespace Warehouse.ContractProcessing.Applications.Handlers.Commands.AddLineCommand
{
    /// <summary>
    /// Команда на добавление строки товара в контракт.
    /// </summary>
    public sealed record AddLineCommand(
        Guid ContractId,
        Guid ProductId,
        int Quantity
    ) : Command;
}
