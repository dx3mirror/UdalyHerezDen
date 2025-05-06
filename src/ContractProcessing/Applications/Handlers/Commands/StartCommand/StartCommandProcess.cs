using Warehouse.ContractProcessing.Applications.AppServices.Abstract;
using Warehouse.ContractProcessing.Applications.AppServices.Models;
using Warehouse.ContractProcessing.Applications.Handlers.Abstracts;

namespace Warehouse.ContractProcessing.Applications.Handlers.Commands.StartCommand
{
    /// <summary>
    /// Обработчик команды <see cref="StartCommand"/>.
    /// </summary>
    public sealed class StartCommandProcess(
        IUnloadingContractService service
    ) : ICommandProcess<StartCommand>
    {
        private readonly IUnloadingContractService _service = service;

        /// <inheritdoc/>
        public async Task Handle(StartCommand command, CancellationToken cancellationToken)
        {
            var request = new IdOnlyModel(command.ContractId);
            await _service.StartAsync(request, cancellationToken);
        }
    }
}
