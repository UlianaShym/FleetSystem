using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.DataAccess.Entities;
using FleetManagement.DataAccess.DbContext;

namespace FleetManagement.DataAccess.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
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
            var tmpEntity = _context.Set<TEntity>().FirstOrDefault(x => x.Id == entity.Id);
            if (tmpEntity == null)
                throw new ArgumentException("No such item exists");

            //tmpEntity = Mapper.Map<TEntity>(entity);

            //_context.Entry(tmpEntity).State = EntityState.Modified;

            _context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            Delete(entity.Id);
        }

        public void Delete(int id)
        {
            TEntity entity = _context.Set<TEntity>().Find(id);

            _context.Set<TEntity>().Remove(entity);
            _context.SaveChanges();
        }
    }
}
