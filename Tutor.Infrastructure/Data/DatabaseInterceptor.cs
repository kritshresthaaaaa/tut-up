using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Tutor.Application.Services.CurrentUser;
using Tutor.Domain.Interfaces;

namespace Tutor.Infrastructure.Data
{
    public class DatabaseInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUserService _currentUserService;

        public DatabaseInterceptor(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            if (eventData.Context == null)
            {
                return base.SavingChanges(eventData, result);
            }

            InterceptEntities(eventData);
            return base.SavingChanges(eventData, result);
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            if (eventData.Context == null)
            {
                return await base.SavingChangesAsync(eventData, result, cancellationToken);
            }

            InterceptEntities(eventData);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void InterceptEntities(DbContextEventData eventData)
        {
            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (entry.Entity is IAuditableEntity auditableEntity)
                        {
                            auditableEntity.CreatedBy = Guid.NewGuid();
                            auditableEntity.CreationTime = DateTime.UtcNow;
                        }
                        break;

                    case EntityState.Modified:
                        if (entry.Entity is IAuditableEntity auditableModifiedEntity)
                        {
                            auditableModifiedEntity.LastModifiedBy = Guid.NewGuid();
                            auditableModifiedEntity.LastModifiedTime = DateTime.UtcNow;
                        }
                        break;

                    case EntityState.Deleted:
                        if (entry.Entity is ISoftDeleteEntity softDeleteEntity)
                        {
                            // Soft delete logic
                            softDeleteEntity.DeletedBy = Guid.NewGuid();
                            softDeleteEntity.DeletionTime = DateTime.UtcNow;
                            softDeleteEntity.IsDeleted = true;
                            entry.State = EntityState.Modified; // Change state to Modified
                        }

                        if (entry.Entity is IAuditableEntity auditableEntitys)
                        {
                            // Audit logic for deleted entities
                            auditableEntitys.LastModifiedBy = Guid.NewGuid();
                            auditableEntitys.LastModifiedTime = DateTime.UtcNow;
                            entry.State = EntityState.Modified; // Change state to Modified
                        }
                        break;
                }
            }
        }
    }
}