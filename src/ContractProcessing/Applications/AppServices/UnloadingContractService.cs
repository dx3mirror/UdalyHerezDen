using Microsoft.EntityFrameworkCore;
using Stateless;
using Utilities.DbContextSettings.Abstracts;
using Warehouse.ContractProcessing.Applications.AppServices.Abstract;
using Warehouse.ContractProcessing.Applications.AppServices.Models;
using Warehouse.ContractProcessing.Applications.AppServices.StateMachines.Enum;
using Warehouse.ContractProcessing.Domain.Aggregates;
using Warehouse.ContractProcessing.Domain.Enums;
using Warehouse.ContractProcessing.Domain.Exceptions;
using Warehouse.ContractProcessing.Domain.ValueObjects;

namespace Warehouse.ContractProcessing.Applications.AppServices
{
    /// <summary>
    /// Сервис управления контрактами выгрузки, с машиной состояний на уровне Application.
    /// </summary>
    /// <param name="repository"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public class UnloadingContractService(IRepository<UnloadingContract> repository) : IUnloadingContractService
    {
        private readonly IRepository<UnloadingContract> _repository = repository
                ?? throw new ArgumentNullException(nameof(repository));

        /// <inheritdoc/>
        public async Task<Guid> CreateAsync(CreateUnloadingContractModel request, CancellationToken cancellationToken)
        {
            var contract = UnloadingContract.Create(
                request.ContractId,
                request.WarehouseId,
                request.ManagerId,
                request.ScheduledFor);

            await _repository.AddAsync(contract, cancellationToken);
            return contract.Id.Value;
        }

        /// <inheritdoc/>
        public async Task AddLineAsync(AddLineModel request, CancellationToken cancellationToken)
        {
            var contract = await LoadContract(request.ContractId, cancellationToken);
            contract.AddLine(request.ProductId, request.Quantity);
            await _repository.UpdateAsync(contract, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task RescheduleAsync(RescheduleModel request, CancellationToken cancellationToken)
        {
            var contract = await LoadContract(request.ContractId, cancellationToken);
            contract.Reschedule(request.NewDate);
            await _repository.UpdateAsync(contract, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task StartAsync(IdOnlyModel request, CancellationToken cancellationToken)
        {
            var contract = await LoadContract(request.ContractId, cancellationToken);

            var machine = new StateMachine<ContractStatus, UnloadingTrigger>(contract.Status);

            machine.Configure(ContractStatus.Pending)
                   .Permit(UnloadingTrigger.Start, ContractStatus.InProgress)
                   .Permit(UnloadingTrigger.Cancel, ContractStatus.Cancelled);

            machine.Configure(ContractStatus.InProgress)
                   .Permit(UnloadingTrigger.Complete, ContractStatus.Completed)
                   .Permit(UnloadingTrigger.Cancel, ContractStatus.Cancelled);

            machine.OnTransitioned(t =>
            {
                if (t.Trigger == UnloadingTrigger.Start)
                    contract.Start();
            });

            try
            {
                machine.Fire(UnloadingTrigger.Start);
            }
            catch (InvalidOperationException)
            {
                throw new UnloadingContractException(
                    $"Невозможно перевести контракт из состояния {contract.Status} в InProgress.");
            }

            await _repository.UpdateAsync(contract, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task CompleteAsync(IdOnlyModel request, CancellationToken cancellationToken)
        {
            var contract = await LoadContract(request.ContractId, cancellationToken);

            var machine = new StateMachine<ContractStatus, UnloadingTrigger>(contract.Status);
            machine.Configure(ContractStatus.Pending)
                   .Permit(UnloadingTrigger.Start, ContractStatus.InProgress)
                   .Permit(UnloadingTrigger.Cancel, ContractStatus.Cancelled);
            machine.Configure(ContractStatus.InProgress)
                   .Permit(UnloadingTrigger.Complete, ContractStatus.Completed)
                   .Permit(UnloadingTrigger.Cancel, ContractStatus.Cancelled);

            machine.OnTransitioned(t =>
            {
                if (t.Trigger == UnloadingTrigger.Complete)
                    contract.Complete();
            });

            try
            {
                machine.Fire(UnloadingTrigger.Complete);
            }
            catch (InvalidOperationException)
            {
                throw new UnloadingContractException(
                    $"Невозможно перевести контракт из состояния {contract.Status} в Completed.");
            }

            await _repository.UpdateAsync(contract, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task CancelAsync(IdOnlyModel request, CancellationToken cancellationToken)
        {
            var contract = await LoadContract(request.ContractId, cancellationToken);

            var machine = new StateMachine<ContractStatus, UnloadingTrigger>(contract.Status);
            machine.Configure(ContractStatus.Pending)
                   .Permit(UnloadingTrigger.Start, ContractStatus.InProgress)
                   .Permit(UnloadingTrigger.Cancel, ContractStatus.Cancelled);
            machine.Configure(ContractStatus.InProgress)
                   .Permit(UnloadingTrigger.Complete, ContractStatus.Completed)
                   .Permit(UnloadingTrigger.Cancel, ContractStatus.Cancelled);

            machine.OnTransitioned(t =>
            {
                if (t.Trigger == UnloadingTrigger.Cancel)
                    contract.Cancel();
            });

            try
            {
                machine.Fire(UnloadingTrigger.Cancel);
            }
            catch (InvalidOperationException)
            {
                throw new UnloadingContractException(
                    $"Невозможно отменить контракт из состояния {contract.Status}.");
            }

            await _repository.UpdateAsync(contract, cancellationToken);
        }

        private async Task<UnloadingContract> LoadContract(Guid id, CancellationToken ct)
        {
            var cid = ContractId.Of(id);
            var contract = await _repository
                .Where(c => c.Id == cid)
                .SingleOrDefaultAsync(ct);

            if (contract is null)
                throw new InvalidOperationException(nameof(UnloadingContract));

            return contract;
        }
    }
}