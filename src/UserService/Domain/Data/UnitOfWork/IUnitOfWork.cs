using System;

namespace UserService.Domain.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        void InitializeContext();
    }
}