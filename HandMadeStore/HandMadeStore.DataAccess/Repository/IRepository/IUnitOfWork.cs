using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandMadeStore.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepositery Category { get; }
        IBrandRepositery Brand { get; }
        IProductRepositery Product { get; }


        void Save();
    }
}
