using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SPGW.Domain.Common;
using SPGW.Infra.Contexts;

namespace SPGW.Infra.Common
{
    public class EFRepository<T> : IEFRepository<T> where T : Entity
    {
        #region Private Fields

        private readonly IEFUnitOfWorkScope _unitofworkscope;
        private readonly EFUnitOfWork _unitOfWork;

        //tihs fields are protected becuase needed in repository for example for execute stored procedure we need cotext object.
        protected readonly SPGWContext _context;
        protected readonly DbSet<T> _dbSet;

        #endregion Private Fields

        public EFRepository(IEFUnitOfWorkScope unitofworkscope)
        {
            _unitofworkscope = unitofworkscope;
            _unitOfWork = unitofworkscope.CurrentUnitOfWork as EFUnitOfWork;
            _context = _unitOfWork.CPH4Context;
            _dbSet = _context.Set<T>();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public int Count()
        {
            return _dbSet.Count();
        }

        public int Count(Expression<Func<T, bool>> where)
        {
            return _dbSet.Count(where);
        }

        public void Delete(long Id)
        {
            var item = this.Find(Id);
            if (item != null)
            {
                _dbSet.Remove(item);
                _unitofworkscope.Commit();
            }
        }

        public void Delete(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
                _unitofworkscope.Commit();
            }
            catch (Exception)
            {

            }
        }

        //public void DeleteRange(IEnumerable<long> Ids)
        //{
        //    var items = this._dbSet.Select(d=>Ids.Contains(d.Id);
        //    if (item != null)
        //    {
        //        _dbSet.RemoveRange(item);
        //        _unitofworkscope.Commit();
        //    }
        //}

        public void DeleteRange(IEnumerable<T> entity)
        {
            try
            {
                _dbSet.RemoveRange(entity);
                _unitofworkscope.Commit();
            }
            catch (Exception)
            {

            }
        }

        public T Find(object Id)
        {
            return _dbSet.Find(Id);
        }

        public T Find(params object[] keyValues)
        {
            return _dbSet.Find(keyValues);
        }

        public T Find(long Id)
        {
            return _dbSet.Find(Id);
        }

        public IList<T> GetAll()
        {
            return _dbSet.AsQueryable().ToList();
        }

        public void Insert(T item)
        {
            _dbSet.Add(item);
            _unitofworkscope.Commit();
        }

        public void InsertRange(IEnumerable<T> items)
        {
            _dbSet.AddRange(items);
            _unitofworkscope.Commit();
        }

        public TResult Max<TResult>(Expression<Func<T, TResult>> selector)
        {
            return _dbSet.Max(selector);
        }

        public TResult Max<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> selector)
        {
            return _dbSet.Where(where).Max(selector);
        }

        public IQueryable<T> Queryable()
        {
            return _dbSet.AsQueryable<T>();
        }

        //public IQueryable<T> Select(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<Expression<Func<T, object>>> includes = null, int? page = default(int?), int? pageSize = default(int?))
        //{
        //    var data=_dbSet.
        //    throw new NotImplementedException();
        //}

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _unitofworkscope.Commit();
        }
    }
}
