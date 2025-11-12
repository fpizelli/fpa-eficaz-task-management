namespace EficazAPI.Application.Ports.Shared
{
    public interface ICommandRepository<TEntity, TId> where TEntity : class
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TId id);
        void Delete(TEntity entity);
    }
}
