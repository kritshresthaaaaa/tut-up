using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;
using Tutor.Application.Common.Interfaces;

namespace Tutor.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TutorDbContext _dbContext;
        private IDbContextTransaction _transaction;

        public UnitOfWork(TutorDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreateTransaction()
        {
            _transaction = _dbContext.Database.BeginTransaction();
        }


        public void Commit()
        {
            try
            {
                _dbContext.SaveChanges();
                _transaction?.Commit();
            }
            catch
            {
                Rollback();
                throw;
            }
        }

        public void Rollback()
        {
            _transaction?.Rollback();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _dbContext.Dispose();
        }
    }
}