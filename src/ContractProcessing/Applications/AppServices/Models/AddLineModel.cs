namespace Warehouse.ContractProcessing.Applications.AppServices.Models
{
    /// <summary>
    /// Данные для добавления товарной строки в контракт.
    /// </summary>
    public sealed record AddLineModel(
        Guid ContractId,
        Guid ProductId,
        int Quantity
    );
}
