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
        IEnumerable<T> GetAll();
        T GetFirstOrDefault(Expression<Func<T,bool>> filter);
        //void Update(T entity);  
        void Remove(T entity);
        void RemoveRang(IEnumerable<T> entity);

        void Add(T entity);

    }
}
