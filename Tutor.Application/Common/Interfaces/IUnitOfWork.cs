namespace Tutor.Application.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void CreateTransaction();
        void Commit();
        void Rollback();
    }
}
