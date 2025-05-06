namespace Warehouse.ContractProcessing.Applications.AppServices.Models
{
    /// <summary>
    /// Данные для переноса даты выгрузки.
    /// </summary>
    public sealed record RescheduleModel(
        Guid ContractId,
        DateTime NewDate
    );
}
