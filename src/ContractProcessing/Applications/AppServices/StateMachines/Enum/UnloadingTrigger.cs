using Warehouse.ContractProcessing.Domain.Enums;

namespace Warehouse.ContractProcessing.Applications.AppServices.StateMachines.Enum
{
    /// <summary>
    /// Триггеры для переходов состояний контракта.
    /// </summary>
    public enum UnloadingTrigger
    {
        /// <summary>
        /// Запустить выгрузку (перевод из <see cref="ContractStatus.Pending"/> 
        /// в <see cref="ContractStatus.InProgress"/>).
        /// </summary>
        Start,

        /// <summary>
        /// Завершить выгрузку (перевод из <see cref="ContractStatus.InProgress"/> 
        /// в <see cref="ContractStatus.Completed"/>).
        /// </summary>
        Complete,

        /// <summary>
        /// Отменить контракт (из <see cref="ContractStatus.Pending"/> 
        /// или <see cref="ContractStatus.InProgress"/> 
        /// в <see cref="ContractStatus.Cancelled"/>).
        /// </summary>
        Cancel
    }
}
