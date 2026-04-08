using Booking.Application.Interfaces.Repositories;
using Booking.Domain.Entities;
using Booking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Booking.Infrastructure.Repositories;

public class StylistRepository : IStylistRepository
{
    private readonly ApplicationDbContext _context;

    public StylistRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all stylists
    /// </summary>
    /// <remarks>
    /// No OrderBy or pagination is used because it is not written in the technical task,
    /// but it would be reasonable to use them.
    /// </remarks>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of stylists</returns>
    public async Task<List<StylistEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Stylists
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Get stylist by id
    /// </summary>
    /// <param name="id">Stylist id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task<StylistEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Stylists.FindAsync([id], cancellationToken);
    }

    /// <summary>
    /// Add stylist
    /// </summary>
    /// <param name="entity">Stylist entity</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task AddAsync(StylistEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.Stylists.AddAsync(entity, cancellationToken);
    }

    /// <summary>
    /// Update stylist
    /// </summary>
    /// <param name="entity">Stylist entity</param>
    public void Update(StylistEntity entity)
    {
        _context.Stylists.Update(entity);
    }

    /// <summary>
    /// Remove stylist
    /// </summary>
    /// <param name="entity">Stylist entity</param>
    public void Remove(StylistEntity entity)
    {
        _context.Stylists.Remove(entity);
    }
}
