using System;
using System.Data.Entity;
using System.Linq;
using FleetManagement.DataAccess.DbContext;

namespace FleetManagement.DataAccess.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private ApplicationDbContext _context;
    
        public Repository()
        {
            _context = ContexSingleton.GetDbContext();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public IQueryable<TEntity> Find(Func<TEntity, bool> predicate)
        {
            return _context.Set<TEntity>().Where(predicate).AsQueryable();
        }

        public TEntity GetById(int id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public void AddNew(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            _context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
           _context.Entry(entity).State = EntityState.Modified;

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            TEntity entity = _context.Set<TEntity>().Find(id);

            _context.Set<TEntity>().Remove(entity);
            _context.SaveChanges();
        }
    }
}
