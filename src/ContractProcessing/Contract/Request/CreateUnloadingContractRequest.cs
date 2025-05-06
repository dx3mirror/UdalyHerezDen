namespace Warehouse.ContractProcessing.Сontract.Request
{
    /// <summary>
    /// Запрос на создание контракта на выгрузку.
    /// </summary>
    /// <param name="WarehouseId"></param>
    /// <param name="ManagerId"></param>
    /// <param name="ScheduledFor"></param>
    public record CreateUnloadingContractRequest(Guid WarehouseId, Guid ManagerId, DateTime ScheduledFor);
}
