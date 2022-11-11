using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HandMadeStore.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(string[] inCludeProperties = null);
        T GetFirstOrDefault(Expression<Func<T,bool>> filter, string inCludeProperties = null);
        //void Update(T entity);  
        void Remove(T entity);
        void RemoveRang(IEnumerable<T> entity);

        void Add(T entity);
        void ClearChangeTrackin();

    }
}
