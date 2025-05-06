using Warehouse.ContractProcessing.Applications.AppServices.Models;

namespace Warehouse.ContractProcessing.Applications.AppServices.Abstract
{
    /// <summary>
    /// Сервис для работы с контрактами выгрузки (UnloadingContract).
    /// </summary>
    public interface IUnloadingContractService
    {
        /// <summary>
        /// Создаёт новый контракт на выгрузку товаров.
        /// </summary>
        /// <param name="request">Данные для создания контракта (<see cref="CreateUnloadingContractModel"/>).</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>GUID вновь созданного контракта.</returns>
        Task<Guid> CreateAsync(CreateUnloadingContractModel request, CancellationToken cancellationToken);

        /// <summary>
        /// Добавляет товарную строку в существующий контракт.
        /// </summary>
        /// <param name="request">Данные для добавления строки (<see cref="AddLineModel"/>).</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        Task AddLineAsync(AddLineModel request, CancellationToken cancellationToken);

        /// <summary>
        /// Переносит запланированную дату выгрузки для контракта.
        /// </summary>
        /// <param name="request">Данные для переноса даты (<see cref="RescheduleModel"/>).</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        Task RescheduleAsync(RescheduleModel request, CancellationToken cancellationToken);

        /// <summary>
        /// Переводит контракт в состояние «в процессе выгрузки».
        /// </summary>
        /// <param name="request">Данные для операции (ID контракта) — <see cref="IdOnlyModel"/>.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        Task StartAsync(IdOnlyModel request, CancellationToken cancellationToken);

        /// <summary>
        /// Завершает выгрузку по контракту.
        /// </summary>
        /// <param name="request">Данные для операции (ID контракта) — <see cref="IdOnlyModel"/>.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        Task CompleteAsync(IdOnlyModel request, CancellationToken cancellationToken);

        /// <summary>
        /// Отменяет контракт на выгрузку.
        /// </summary>
        /// <param name="request">Данные для операции (ID контракта) — <see cref="IdOnlyModel"/>.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        Task CancelAsync(IdOnlyModel request, CancellationToken cancellationToken);
    }
}
