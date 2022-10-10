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
    public class ProductRepository : Repository<Product>, IProductRepositery
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public ApplicationDbContext Context { get; }

        public void Update(Product prdouct)
        {
            var ProductFromDb = _context.Products.Find(prdouct.Id);
            if(ProductFromDb != null)
            {
                ProductFromDb.Name = prdouct.Name;
                ProductFromDb.Description = prdouct.Description;
                ProductFromDb.Price = prdouct.Price;
                ProductFromDb.Price30Plus = prdouct.Price30Plus;
                ProductFromDb.Price10Plus = prdouct.Price10Plus;
                ProductFromDb.CreatedDate = prdouct.CreatedDate;
                ProductFromDb.CategoryId = prdouct.CategoryId;
                ProductFromDb.BrandId = prdouct.BrandId;
                if(prdouct.ImageUrl != null)
                {
                    
                    ProductFromDb.ImageUrl = prdouct.ImageUrl;
                }
            }
        }
    }
}
