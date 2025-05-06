using Warehouse.ContractProcessing.Applications.AppServices.Abstract;
using Warehouse.ContractProcessing.Applications.AppServices.Models;
using Warehouse.ContractProcessing.Applications.Handlers.Abstracts;

namespace Warehouse.ContractProcessing.Applications.Handlers.Commands.AddLineCommand
{
    /// <summary>
    /// Обработчик команды <see cref="AddLineCommand"/>.
    /// </summary>
    public sealed class AddLineCommandProcess(
        IUnloadingContractService service
    ) : ICommandProcess<AddLineCommand>
    {
        private readonly IUnloadingContractService _service = service;

        /// <inheritdoc/>
        public async Task Handle(AddLineCommand command, CancellationToken cancellationToken)
        {
            var request = new AddLineModel(command.ContractId, command.ProductId, command.Quantity);
            
            await _service.AddLineAsync(request, cancellationToken);
        }
    }
}
