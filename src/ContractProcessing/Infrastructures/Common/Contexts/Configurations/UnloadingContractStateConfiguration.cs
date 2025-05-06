using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Warehouse.ContractProcessing.Infrastructures.Sagas;

namespace Warehouse.ContractProcessing.Infrastructures.Common.Contexts.Configurations
{
    /// <summary>
    /// Конфигурация EF Core для хранения состояния саги <see cref="UnloadingContractState"/>.
    /// </summary>
    public class UnloadingContractStateConfiguration
        : IEntityTypeConfiguration<UnloadingContractState>
    {
        /// <summary>
        /// Конфигурирует модель состояния саги <see cref="UnloadingContractState"/> в контексте базы данных.
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<UnloadingContractState> builder)
        {
            // В таблице будет храниться одно состояние на одна CorrelationId
            builder.ToTable("UnloadingContractSaga", "saga");

            // Первичный ключ — CorrelationId
            builder.HasKey(x => x.CorrelationId);

            builder.Property(x => x.CorrelationId)
                   .ValueGeneratedNever();

            // Текущее машинное состояние как строка
            builder.Property(x => x.CurrentState)
                   .HasMaxLength(64)
                   .IsRequired();

            // Таймер MassTransit (для Schedule)
            builder.Property(x => x.TimeoutTokenId);

            // Остальные поля как обычные столбцы
            builder.Property(x => x.WarehouseId).IsRequired();
            builder.Property(x => x.ManagerId).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.ScheduledFor).IsRequired();
            builder.Property(x => x.LinesCount).IsRequired();
            builder.Property(x => x.StartedAt);
            builder.Property(x => x.CompletedAt);
        }
    }
}
