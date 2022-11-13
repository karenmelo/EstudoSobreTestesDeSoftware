namespace Features.Core
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
    }
}
