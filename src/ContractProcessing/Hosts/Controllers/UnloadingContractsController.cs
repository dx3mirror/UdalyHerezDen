using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics.CodeAnalysis;
using Warehouse.ContractProcessing.Applications.Handlers.Commands.AddLineCommand;
using Warehouse.ContractProcessing.Applications.Handlers.Commands.CancelCommand;
using Warehouse.ContractProcessing.Applications.Handlers.Commands.CompleteCommand;
using Warehouse.ContractProcessing.Applications.Handlers.Commands.CreateUnloadingContractCommand;
using Warehouse.ContractProcessing.Applications.Handlers.Commands.RescheduleCommand;
using Warehouse.ContractProcessing.Applications.Handlers.Commands.StartCommand;
using Warehouse.ContractProcessing.Сontract.Request;
using Wolverine;

namespace Warehouse.ContractProcessing.Host.Public.Controllers
{
    /// <summary>
    /// Контроллер управления контрактами выгрузки.
    /// </summary>
    [ApiController]
    [Route("unloading-contracts")]
    public class UnloadingContractsController(IMessageBus bus) : ControllerBase
    {
        /// <summary>
        /// Шина сообщений для отправки команд.
        /// </summary>
        private readonly IMessageBus _bus = bus;

        /// <summary>
        /// Создаёт контракт на выгрузку.
        /// </summary>
        /// <param name="contractId">Идентификатор контракта.</param>
        /// <param name="dto">Запрос с данными для создания контракта.</param>
        /// <returns>Ответ с кодом 202 (Accepted).</returns>
        [HttpPost("{contractId:guid}/create")]
        public async Task<IActionResult> Create(
            [FromRoute]
            [BindRequired]
            [NotNull]Guid contractId,
            [FromBody] CreateUnloadingContractRequest dto)
        {
            var command = new CreateUnloadingContractCommand(
                contractId,
                dto.WarehouseId,
                dto.ManagerId,
                dto.ScheduledFor
            );
            await _bus.SendAsync(command);
            return Accepted();
        }

        /// <summary>
        /// Добавляет строку товара в контракт выгрузки.
        /// </summary>
        /// <param name="contractId">Идентификатор контракта.</param>
        /// <param name="dto">Данные строки товара.</param>
        /// <returns>Ответ с кодом 202 (Accepted).</returns>
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{contractId:guid}/add-line")]
        public async Task<IActionResult> AddLine(
            [FromRoute]
            [BindRequired]
            [NotNull]Guid contractId, [FromBody] AddLineRequest dto)
        {
            var command = new AddLineCommand(contractId, dto.ProductId, dto.Quantity);
            await _bus.SendAsync(command);
            return Accepted();
        }

        /// <summary>
        /// Переводит контракт в состояние "В процессе" (InProgress).
        /// </summary>
        /// <param name="contractId">Идентификатор контракта.</param>
        /// <returns>Ответ с кодом 202 (Accepted).</returns>
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{contractId:guid}/start")]
        public async Task<IActionResult> Start(
            [FromRoute]
            [BindRequired]
            [NotNull]Guid contractId)
        {
            await _bus.SendAsync(new StartCommand(contractId));
            return Accepted();
        }

        /// <summary>
        /// Завершает контракт (переводит в состояние Completed).
        /// </summary>
        /// <param name="contractId">Идентификатор контракта.</param>
        /// <returns>Ответ с кодом 202 (Accepted).</returns>
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{contractId:guid}/complete")]
        public async Task<IActionResult> Complete(
            [FromRoute]
            [BindRequired]
            [NotNull]Guid contractId)
        {
            await _bus.SendAsync(new CompleteCommand(contractId));
            return Accepted();
        }

        /// <summary>
        /// Отменяет контракт (переводит в состояние Cancelled).
        /// </summary>
        /// <param name="contractId">Идентификатор контракта.</param>
        /// <returns>Ответ с кодом 202 (Accepted).</returns>
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{contractId:guid}/cancel")]
        public async Task<IActionResult> Cancel(
            [FromRoute]
            [BindRequired]
            [NotNull]Guid contractId)
        {
            await _bus.SendAsync(new CancelCommand(contractId));
            return Accepted();
        }

        /// <summary>
        /// Переносит дату выполнения контракта.
        /// </summary>
        /// <param name="contractId">Идентификатор контракта.</param>
        /// <param name="dto">Новая дата выполнения.</param>
        /// <returns>Ответ с кодом 202 (Accepted).</returns>
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{contractId:guid}/reschedule")]
        public async Task<IActionResult> Reschedule(
            [FromRoute]
            [BindRequired]
            [NotNull]Guid contractId, 
            [FromBody] RescheduleRequest dto)
        {
            await _bus.SendAsync(new RescheduleCommand(contractId, dto.NewDate));
            return Accepted();
        }
    }
}
