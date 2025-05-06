﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using Warehouse.ContractProcessing.Domain.Aggregates;
using Warehouse.ContractProcessing.Domain.Entity;
using Warehouse.ContractProcessing.Infrastructures.Common.Contexts.Configurations;

namespace Warehouse.ContractProcessing.Infrastructures.Common.Contexts
{
    /// <summary>
    /// DbContext для хранения агрегатов UnloadingContract и связанных сущностей.
    /// </summary>
    public class ContractProcessingDbContext : DbContext
    {
        /// <summary>
        /// Контракты разгрузки.
        /// </summary>
        public DbSet<UnloadingContract> UnloadingContracts { get; set; }

        /// <summary>
        /// Строки контрактов разгрузки.
        /// </summary>
        public DbSet<UnloadingLine> UnloadingLines { get; set; }

        /// <summary>
        /// Конструктор DbContext для микросервиса обработки контрактов разгрузки.
        /// </summary>
        /// <param name="options"></param>
        public ContractProcessingDbContext(DbContextOptions<ContractProcessingDbContext> options)
            : base(options)
        {
        }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UnloadingContractConfiguration());
            modelBuilder.ApplyConfiguration(new UnloadingLineConfiguration());
            CustomModelBuilder.OnModelCreating(modelBuilder);
        }
    }
}
