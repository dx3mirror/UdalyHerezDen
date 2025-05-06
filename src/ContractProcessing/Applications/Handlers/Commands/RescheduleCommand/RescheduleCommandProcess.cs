using Warehouse.ContractProcessing.Applications.AppServices.Abstract;
using Warehouse.ContractProcessing.Applications.AppServices.Models;
using Warehouse.ContractProcessing.Applications.Handlers.Abstracts;

namespace Warehouse.ContractProcessing.Applications.Handlers.Commands.RescheduleCommand
{
    /// <summary>
    /// Обработчик команды <see cref="RescheduleCommand"/>.
    /// </summary>
    public sealed class RescheduleCommandProcess(
        IUnloadingContractService service
    ) : ICommandProcess<RescheduleCommand>
    {
        private readonly IUnloadingContractService _service = service;

        /// <inheritdoc/>
        public async Task Handle(RescheduleCommand command, CancellationToken cancellationToken)
        {
            var request = new RescheduleModel(command.ContractId, command.NewDate);
            
            await _service.RescheduleAsync(request, cancellationToken);
        }
    }
}
