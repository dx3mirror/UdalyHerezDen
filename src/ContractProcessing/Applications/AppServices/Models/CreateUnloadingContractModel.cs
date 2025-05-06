namespace Warehouse.ContractProcessing.Applications.AppServices.Models
{
    /// <summary>
    /// Данные для создания контракта на выгрузку.
    /// </summary>
    public sealed record CreateUnloadingContractModel(
        Guid ContractId,
        Guid WarehouseId,
        Guid ManagerId,
        DateTime ScheduledFor
    );
}
