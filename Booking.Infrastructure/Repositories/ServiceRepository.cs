using Booking.Application.Interfaces.Repositories;
using Booking.Domain.Entities;
using Booking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Booking.Infrastructure.Repositories;

public class ServiceRepository : IServiceRepository
{
    private readonly ApplicationDbContext _context;

    public ServiceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ServiceEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Services
            .AsNoTracking()
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<ServiceEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Services.FindAsync([id], cancellationToken);
    }

    public async Task AddAsync(ServiceEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.Services.AddAsync(entity, cancellationToken);
    }

    public void Update(ServiceEntity entity)
    {
        _context.Services.Update(entity);
    }

    public void Remove(ServiceEntity entity)
    {
        _context.Services.Remove(entity);
    }
}
