using Booking.Domain.Entities;

namespace Booking.Application.Interfaces.Repositories;

public interface IServiceRepository
{
    Task<List<ServiceEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ServiceEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(ServiceEntity entity, CancellationToken cancellationToken = default);
    void Update(ServiceEntity entity);
    void Remove(ServiceEntity entity);
}
