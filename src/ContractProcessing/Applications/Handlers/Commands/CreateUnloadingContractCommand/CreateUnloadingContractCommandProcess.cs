using Warehouse.ContractProcessing.Applications.AppServices.Abstract;
using Warehouse.ContractProcessing.Applications.AppServices.Models;
using Warehouse.ContractProcessing.Applications.Handlers.Abstracts;

namespace Warehouse.ContractProcessing.Applications.Handlers.Commands.CreateUnloadingContractCommand
{
    /// <summary>
    /// Обработчик команды <see cref="CreateUnloadingContractCommand"/>.
    /// </summary>
    public sealed class CreateUnloadingContractCommandProcess(
        IUnloadingContractService service
    ) : ICommandProcess<CreateUnloadingContractCommand>
    {
        private readonly IUnloadingContractService _service = service
            ?? throw new ArgumentNullException(nameof(service));

        /// <inheritdoc/>
        public async Task Handle(CreateUnloadingContractCommand command, CancellationToken cancellationToken)
        {
            var request = new CreateUnloadingContractModel(command.ContractId, command.WarehouseId, command.ManagerId, command.ScheduledFor);
            
            await _service.CreateAsync(request, cancellationToken);
        }
    }
}
