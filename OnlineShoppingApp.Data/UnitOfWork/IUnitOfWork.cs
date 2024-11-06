using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransaction();
        Task RollbackTransaction();
    }
}
