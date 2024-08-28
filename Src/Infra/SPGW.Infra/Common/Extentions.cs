using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SPGW.Infra.Common
{
    public static class Extentions
    {
        public static IQueryable<T> ApplyInclude<T>(this IQueryable<T> query, params Expression<Func<T, object>>[] includes) where T : class
        {
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return query;
        }

        public static IQueryable<T> ApplySort<T>(this IQueryable<T> query) where T : class
        {
            return query;
        }
    }
}
