using Microsoft.EntityFrameworkCore;
using Teams.Data;
using Teams.Models;

namespace Teams.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : Entity
    {
        private readonly TeamsContext _context;

        public BaseRepository(TeamsContext context)
        {
            _context = context;
        }
        public virtual async Task<Guid> AddAsync(T entity)
        {
            entity.Id = Guid.NewGuid();
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> ListAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task UpdateAsync(Guid id, T entity)
        {
            if (!EntityExists(id))
                throw new ArgumentException("Entity not exists.",nameof(id));
            _context.Entry(entity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!EntityExists(id))
                    throw new ArgumentException(ex.Message,nameof(id));
                throw;
            }
        }

        private bool EntityExists(Guid id)
        {
            return _context.Set<T>().Any(e => e.Id == id);
        }
    }
}
