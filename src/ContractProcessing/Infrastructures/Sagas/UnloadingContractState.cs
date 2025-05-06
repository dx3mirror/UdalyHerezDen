using MassTransit;

namespace Warehouse.ContractProcessing.Infrastructures.Sagas
{
    /// <summary>
    /// Состояние саги контракта выгрузки.
    /// </summary>
    public class UnloadingContractState : SagaStateMachineInstance
    {
        /// <inheritdoc />
        public Guid CorrelationId { get; set; }

        /// <summary>Текущее состояние саги.</summary>
        public string CurrentState { get; set; } = string.Empty;

        /// <summary>Идентификатор задания таймаута.</summary>
        public Guid? TimeoutTokenId { get; set; }

        /// <summary>
        /// Идентификатор склада, на который планируется выгрузка.
        /// </summary>
        public Guid WarehouseId { get; set; }

        /// <summary>
        /// Идентификатор менеджера, ответственного за организацию выгрузки.
        /// </summary>
        public Guid ManagerId { get; set; }

        /// <summary>
        /// Дата и время создания контракта на выгрузку (UTC).
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Запланированная дата и время начала выгрузки (UTC).
        /// </summary>
        public DateTime ScheduledFor { get; set; }

        /// <summary>
        /// Количество товарных позиций, добавленных в контракт.
        /// </summary>
        public int LinesCount { get; set; }

        /// <summary>
        /// Дата и время фактического запуска выгрузки (перевод контракта в InProgress), или null, если не запущена.
        /// </summary>
        public DateTime? StartedAt { get; set; }

        /// <summary>
        /// Дата и время завершения выгрузки (перевод контракта в Completed), или null, если не завершена.
        /// </summary>
        public DateTime? CompletedAt { get; set; }
    }
}
