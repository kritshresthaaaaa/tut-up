using Tutor.Application.Common.Interfaces;

namespace Tutor.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TutorDbContext _dbContext;
        private bool _transactionStarted = false;
        private bool _disposed = false;
        public UnitOfWork(TutorDbContext context)
        {
            _dbContext = context;
        }
        public async Task Commit()
        {
            if (_transactionStarted)
            {
                await _dbContext.SaveChangesAsync();
                await _dbContext.Database.CommitTransactionAsync();
                _transactionStarted = false;
            }

        }
        public async Task CreateTransaction()
        {
            if (!_transactionStarted)
            {
                await _dbContext.Database.BeginTransactionAsync();
                _transactionStarted = true;
            }

        }

        public async Task RollbackAsync()
        {
            await _dbContext.Database.RollbackTransactionAsync();
            _transactionStarted = false;
        }
        public void Dispose()
        {
            if (!_disposed)
            {
                _dbContext.Dispose();
                _disposed = true;
            }
        }
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
