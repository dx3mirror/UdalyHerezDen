namespace Warehouse.ContractProcessing.Applications.Handlers.Abstracts
{
    /// <summary>
    /// Базовый класс для команд.
    /// </summary>
    public abstract record Command
    {
        /// <summary>
        /// Идентификатор корреляции.
        /// </summary>
        public Guid CorrelationId { get; init; }

        /// <summary>
        /// Идентификатор контракта.
        /// </summary>
        protected Command()
        {
            CorrelationId = Guid.NewGuid();
        }

        /// <summary>
        /// Идентификатор контракта.
        /// </summary>
        /// <param name="correlationId"></param>
        protected Command(Guid correlationId)
        {
            CorrelationId = correlationId == Guid.Empty ? Guid.NewGuid() : correlationId;
        }
    }
}
