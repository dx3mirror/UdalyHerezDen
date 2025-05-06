using Warehouse.ContractProcessing.Applications.AppServices.Abstract;
using Warehouse.ContractProcessing.Applications.AppServices.Models;
using Warehouse.ContractProcessing.Applications.Handlers.Abstracts;

namespace Warehouse.ContractProcessing.Applications.Handlers.Commands.CancelCommand
{
    /// <summary>
    /// Обработчик команды <see cref="CancelCommand"/>.
    /// </summary>
    public sealed class CancelCommandProcess(
        IUnloadingContractService service
    ) : ICommandProcess<CancelCommand>
    {
        private readonly IUnloadingContractService _service = service;

        /// <inheritdoc/>
        public async Task Handle(CancelCommand command, CancellationToken cancellationToken)
        {
            var request = new IdOnlyModel(command.ContractId);
            await _service.CancelAsync(request, cancellationToken);
        }
    }
}
