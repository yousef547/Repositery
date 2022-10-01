using HandMadeStore.Data;
using HandMadeStore.DataAccess.Repository.IRepository;
using HandMadeStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandMadeStore.DataAccess.Repository
{
    public class CategoryRepositery : Repository<Category>, ICategoryRepositery
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepositery(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public ApplicationDbContext Context { get; }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
        }
    }
}
