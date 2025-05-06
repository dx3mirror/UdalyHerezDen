namespace Warehouse.ContractProcessing.Сontract.Request
{
    /// <summary>
    /// Запрос на изменение даты контракта.
    /// </summary>
    /// <param name="NewDate"></param>
    public record RescheduleRequest(DateTime NewDate);
}
