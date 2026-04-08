using Booking.Domain.Entities;

namespace Booking.Application.Interfaces.Repositories;

public interface IStylistRepository
{
    Task<List<StylistEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<StylistEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(StylistEntity entity, CancellationToken cancellationToken = default);
    void Update(StylistEntity entity);
    void Remove(StylistEntity entity);
}
