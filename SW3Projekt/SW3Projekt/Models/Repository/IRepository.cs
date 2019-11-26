using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.Models.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        List<TEntity> GetAll();
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");
        List<TEntity> FindById(object id);
        void Add(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);
        void Save();
    }
}
