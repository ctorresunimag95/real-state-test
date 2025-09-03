using Microsoft.EntityFrameworkCore;
using RealState.Test.Domain.Property;

namespace RealState.Test.Infrastructure.Persistence.Repositories;

public class PropertyRepository : IPropertyRepository
{
    private readonly RealStateContext _context;

    public PropertyRepository(RealStateContext context)
    {
        _context = context;
    }

    public async Task<Property?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context
            .Properties
            .Include(p => p.Owner )
            .Include(p => p.PropertyImages )
            .Include(p => p.PropertyTraces )
            .SingleOrDefaultAsync(x => x.IdProperty == id, cancellationToken: cancellationToken);
    }

    public async Task AddAsync(Property property, CancellationToken cancellationToken = default)
    {
        _context.Properties.Add(property);
    }

    public async Task<IReadOnlyCollection<Property>> GetAsync(PropertyFilters filters
        , CancellationToken cancellationToken = default)
    {
        var properties = _context.Properties.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filters.Name))
        {
            properties = properties.Where(p => p.Name.Contains(filters.Name));
        }
        
        if (!string.IsNullOrWhiteSpace(filters.Address))
        {
            properties = properties.Where(p => p.Name.Contains(filters.Address));
        }
        
        if (!string.IsNullOrWhiteSpace(filters.CodeInternal))
        {
            properties = properties.Where(p => p.Name.Equals(filters.CodeInternal));
        }
        
        if (filters.Price is not null)
        {
            properties = properties.Where(p => p.Price == filters.Price );
        }
        
        if (filters.Year is not null)
        {
            properties = properties.Where(p => p.Price == filters.Year );
        }
        
        if (filters.IdOwner is not null)
        {
            properties = properties.Where(p => p.IdOwner == filters.IdOwner );
        }
        
        return await properties
            .Include(p => p.Owner )
            .Include(p => p.PropertyImages )
            .Include(p => p.PropertyTraces ).ToListAsync(cancellationToken);
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}