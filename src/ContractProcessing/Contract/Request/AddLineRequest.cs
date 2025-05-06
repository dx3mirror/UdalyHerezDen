namespace Warehouse.ContractProcessing.Сontract.Request
{
    /// <summary>
    /// Добавление единицы товара в контракт.
    /// </summary>
    /// <param name="ProductId"></param>
    /// <param name="Quantity"></param>
    public record AddLineRequest(Guid ProductId, int Quantity);
}
