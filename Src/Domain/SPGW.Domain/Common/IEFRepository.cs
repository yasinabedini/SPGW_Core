using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SPGW.Domain.Common
{
    public interface IEFRepository<T> where T : Entity
    {
        #region CRUD

        void Insert(T item);

        void InsertRange(IEnumerable<T> entities);

        void Update(T entity);

        void Delete(long Id);

        void Delete(T entity);

        void DeleteRange(IEnumerable<T> entity);

        IQueryable<T> Queryable();

        #endregion CRUD

        IDbContextTransaction BeginTransaction();

        T Find(long Id);

        T Find(params object[] keyValues);

        int Count();

        int Count(Expression<Func<T, bool>> where);

        TResult Max<TResult>(Expression<Func<T, TResult>> selector);

        TResult Max<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector);

        IList<T> GetAll();

        //IQueryable<T> Select(
        //    Expression<Func<T, bool>> filter = null,
        //    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        //    List<Expression<Func<T, object>>> includes = null,
        //    int? page = null,
        //    int? pageSize = null);
    }
}
