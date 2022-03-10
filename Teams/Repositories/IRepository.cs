namespace Teams.Repositories
{
    public interface IRepository<T>
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> ListAsync();
        Task<Guid> AddAsync(T t);
        Task UpdateAsync(Guid id, T t);
    }
}
