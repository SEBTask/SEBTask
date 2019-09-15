﻿using Domain.Contracts.DataAccess;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SEBTaskDbContext context;
        private readonly Dictionary<Type, object> repositories;

        public UnitOfWork(SEBTaskDbContext context)
        {
            this.context = context;

            this.repositories = new Dictionary<Type, object>();
        }

        public async Task<int> CommitAsync()
        {
            return await this.context.SaveChangesAsync();
        }

        public void Dispose()
        {
            this.context.Dispose();
        }

        public IRepository<T> GetRepository<T>() where T : class, BaseEntity
        {
            var targetType = typeof(T);

            if (!this.repositories.ContainsKey(targetType))
            {
                repositories[targetType] = new Repository<T>(this.context);
            }

            return (IRepository<T>)this.repositories[targetType];
        }
    }
}
