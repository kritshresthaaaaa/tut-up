﻿namespace Tutor.Application.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task CreateTransaction();
        Task Commit();
        Task RollbackAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
