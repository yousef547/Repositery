using HandMadeStore.Data;
using HandMadeStore.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HandMadeStore.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void ClearChangeTrackin()
        {
            _context.ChangeTracker.Clear();
        }

        public IEnumerable<T> GetAll(string[] inCludeProperties = null)
        {
            IQueryable<T> query = _dbSet.AsQueryable();
            if(inCludeProperties != null)
            {
                foreach (var prop in inCludeProperties)
                {
                    query = query.Include(prop);
                }
            }
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string inCludeProperties = null)
        {
            IQueryable<T> query = _dbSet.AsQueryable();
            query = query.Where(filter);
            if (inCludeProperties != null)
            {
                foreach (var prop in inCludeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(prop);
                }
            }
            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRang(IEnumerable<T> entity)
        {
            _dbSet.RemoveRange(entity);

        }
    }
    
}
