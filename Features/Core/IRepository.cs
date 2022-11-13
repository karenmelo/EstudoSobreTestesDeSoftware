using System.Linq.Expressions;

namespace Features.Core
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        void Add(TEntity entity);
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
        void Update(TEntity entity);
        void Remove(Guid id);
        IEnumerable<TEntity> Search(Expression<Func<TEntity, bool>> predicate);
        int Commit();
    }
}
