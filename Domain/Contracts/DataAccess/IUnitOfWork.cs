using Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Domain.Contracts.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : class, BaseEntity;
        Task<int> CommitAsync();
    }
}
