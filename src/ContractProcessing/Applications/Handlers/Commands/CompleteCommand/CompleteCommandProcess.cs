using Warehouse.ContractProcessing.Applications.AppServices.Abstract;
using Warehouse.ContractProcessing.Applications.AppServices.Models;
using Warehouse.ContractProcessing.Applications.Handlers.Abstracts;

namespace Warehouse.ContractProcessing.Applications.Handlers.Commands.CompleteCommand
{
    /// <summary>
    /// Обработчик команды <see cref="CompleteCommand"/>.
    /// </summary>
    public sealed class CompleteCommandProcess(
        IUnloadingContractService service
    ) : ICommandProcess<CompleteCommand>
    {
        private readonly IUnloadingContractService _service = service;

        /// <inheritdoc/>
        public async Task Handle(CompleteCommand command, CancellationToken cancellationToken)
        {
            var request = new IdOnlyModel(command.ContractId);
            await _service.CompleteAsync(request, cancellationToken);
        }
    }
}
