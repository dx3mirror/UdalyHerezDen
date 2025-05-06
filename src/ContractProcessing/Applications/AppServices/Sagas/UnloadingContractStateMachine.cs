using MassTransit;
using Warehouse.ContractProcessing.Applications.Handlers.Commands.AddLineCommand;
using Warehouse.ContractProcessing.Applications.Handlers.Commands.CancelCommand;
using Warehouse.ContractProcessing.Applications.Handlers.Commands.CompleteCommand;
using Warehouse.ContractProcessing.Applications.Handlers.Commands.CreateUnloadingContractCommand;
using Warehouse.ContractProcessing.Applications.Handlers.Commands.RescheduleCommand;
using Warehouse.ContractProcessing.Applications.Handlers.Commands.StartCommand;
using Warehouse.ContractProcessing.Infrastructures.Sagas;

namespace Warehouse.ContractProcessing.Applications.AppServices.Sagas
{
    /// <summary>
    /// Сага для управления жизненным циклом контракта выгрузки.
    /// </summary>
    public class UnloadingContractStateMachine : MassTransitStateMachine<UnloadingContractState>
    {
        /// <summary>
        /// Состояние после создания контракта (ожидает первой товарной строки или переноса даты).
        /// </summary>
        public MassTransit.State Created { get; private set; } = null!;

        /// <summary>
        /// Состояние после добавления хотя бы одной товарной строки.
        /// </summary>
        public MassTransit.State LinesAdded { get; private set; } = null!;

        /// <summary>
        /// Состояние после переноса запланированной даты выгрузки.
        /// </summary>
        public MassTransit.State Rescheduled { get; private set; } = null!;

        /// <summary>
        /// Состояние, когда выгрузка по контракту начата.
        /// </summary>
        public MassTransit.State InProgress { get; private set; } = null!;

        /// <summary>
        /// Состояние после успешного завершения выгрузки.
        /// </summary>
        public MassTransit.State Completed { get; private set; } = null!;

        /// <summary>
        /// Состояние после отмены контракта (в результате команды Cancel или таймаута).
        /// </summary>
        public MassTransit.State Cancelled { get; private set; } = null!;

        /// <summary>
        /// Событие создания нового контракта выгрузки.
        /// </summary>
        public Event<CreateUnloadingContractCommand> CreateContract { get; private set; } = null!;

        /// <summary>
        /// Событие добавления товарной строки в контракт.
        /// </summary>
        public Event<AddLineCommand> AddLine { get; private set; } = null!;

        /// <summary>
        /// Событие переноса запланированной даты выгрузки.
        /// </summary>
        public Event<RescheduleCommand> Reschedule { get; private set; } = null!;

        /// <summary>
        /// Событие начала выгрузки (перевод контракта в InProgress).
        /// </summary>
        public Event<StartCommand> Start { get; private set; } = null!;

        /// <summary>
        /// Событие успешного завершения выгрузки (перевод контракта в Completed).
        /// </summary>
        public Event<CompleteCommand> Complete { get; private set; } = null!;

        /// <summary>
        /// Событие отмены контракта выгрузки.
        /// </summary>
        public Event<CancelCommand> Cancel { get; private set; } = null!;

        /// <summary>
        /// Планировщик события таймаута; срабатывает, если нет активности в течение часа.
        /// </summary>
        public Schedule<UnloadingContractState, UnloadingContractTimeoutExpired> TimeoutExpired { get; private set; } = null!;

        /// <summary>
        /// Конструктор состояния саги контракта выгрузки.
        /// </summary>
        public UnloadingContractStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => CreateContract, x => x.CorrelateById(c => c.Message.CorrelationId));
            Event(() => AddLine, x => x.CorrelateById(c => c.Message.CorrelationId));
            Event(() => Reschedule, x => x.CorrelateById(c => c.Message.CorrelationId));
            Event(() => Start, x => x.CorrelateById(c => c.Message.CorrelationId));
            Event(() => Complete, x => x.CorrelateById(c => c.Message.CorrelationId));
            Event(() => Cancel, x => x.CorrelateById(c => c.Message.CorrelationId));

            Schedule(() => TimeoutExpired, x => x.TimeoutTokenId, cfg =>
            {
                cfg.Delay = TimeSpan.FromHours(1);
                cfg.Received = e => e.CorrelateById(c => c.Message.CorrelationId);
            });

            Initially(
                When(CreateContract)
                    .Then(context =>
                    {
                        context.Saga.WarehouseId = context.Message.WarehouseId;
                        context.Saga.ManagerId = context.Message.ManagerId;
                        context.Saga.CreatedAt = DateTime.UtcNow;
                        context.Saga.ScheduledFor = context.Message.ScheduledFor;
                        context.Saga.LinesCount = 0;
                    })
                    .Schedule(TimeoutExpired, ctx => new UnloadingContractTimeoutExpired(ctx.Message.CorrelationId))
                    .TransitionTo(Created)
            );

            During(Created,
                When(AddLine)
                    .Then(ctx => ctx.Saga.LinesCount++)
                    .ThenAsync(ctx => RescheduleTimeout(ctx))
                    .TransitionTo(LinesAdded),

                When(Reschedule)
                    .Then(ctx => ctx.Saga.ScheduledFor = ctx.Message.NewDate)
                    .ThenAsync(ctx => RescheduleTimeout(ctx))
                    .TransitionTo(Rescheduled),

                When(Start)
                    .Then(ctx => ctx.Saga.StartedAt = DateTime.UtcNow)
                    .ThenAsync(ctx => CancelTimeout(ctx))
                    .TransitionTo(InProgress)
            );

            During(LinesAdded, Rescheduled,
                When(Start)
                    .Then(ctx => ctx.Saga.StartedAt = DateTime.UtcNow)
                    .ThenAsync(ctx => CancelTimeout(ctx))
                    .TransitionTo(InProgress)
            );

            During(InProgress,
                When(Complete)
                    .Then(ctx => ctx.Saga.CompletedAt = DateTime.UtcNow)
                    .ThenAsync(ctx => CancelTimeout(ctx))
                    .TransitionTo(Completed)
                    .Finalize(),

                When(Cancel)
                    .ThenAsync(ctx => CancelTimeout(ctx))
                    .TransitionTo(Cancelled)
                    .Finalize()
            );

            DuringAny(
                When(TimeoutExpired.Received)
                    .ThenAsync(ctx => HandleTimeout())
                    .TransitionTo(Cancelled)
                    .Finalize()
            );

            SetCompletedWhenFinalized();
        }

        static async Task RescheduleTimeout(BehaviorContext<UnloadingContractState> context)
        {
            // Отменяем старый таймер, если есть
            if (context.Saga.TimeoutTokenId.HasValue)
            {
                var scheduler = context.GetPayload<IMessageScheduler>();
                var address = new Uri("queue:unloading-contract-timeout");
                await scheduler.CancelScheduledSend(address, context.Saga.TimeoutTokenId.Value);
            }

            // Запланировать новое событие по истечении 1 часа
            var scheduledMessage = await context.SchedulePublish(
                TimeSpan.FromHours(1),
                new UnloadingContractTimeoutExpired(context.Saga.CorrelationId)
            );

            // Сохранить идентификатор таймаута для возможности отмены
            context.Saga.TimeoutTokenId = scheduledMessage.TokenId;
        }

        static async Task CancelTimeout(BehaviorContext<UnloadingContractState> context)
        {
            if (context.Saga.TimeoutTokenId.HasValue)
            {
                var scheduler = context.GetPayload<IMessageScheduler>();
                var address = new Uri("queue:unloading-contract-timeout");
                await scheduler.CancelScheduledSend(address, context.Saga.TimeoutTokenId.Value);
            }
        }

        static Task HandleTimeout()
        {
            // Логирование или уведомление об истечении срока
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// Событие для срабатывания расписания при отсутствии активности.
    /// </summary>
    public sealed record UnloadingContractTimeoutExpired(Guid CorrelationId);
}

