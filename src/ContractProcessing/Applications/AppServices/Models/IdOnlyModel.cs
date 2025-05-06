namespace Warehouse.ContractProcessing.Applications.AppServices.Models
{
    /// <summary>
    /// Данные для операций по смене статуса контракта.
    /// </summary>
    public sealed record IdOnlyModel(
        Guid ContractId
    );
}
