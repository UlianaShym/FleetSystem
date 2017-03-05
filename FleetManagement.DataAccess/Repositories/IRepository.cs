using System.Linq;

namespace FleetManagement.DataAccess.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        TEntity GetById(int id);

        void AddNew(TEntity entity);
        void Update(TEntity entity);
        void Delete(int id);
    }
}
