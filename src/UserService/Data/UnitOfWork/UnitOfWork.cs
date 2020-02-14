using UserService.Data.Contexts;
using UserService.Domain.Data.UnitOfWork;

namespace UserService.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public DefaultContext Context;

        public void InitializeContext()
        {
            if (Context == null)
                Context = new DefaultContext();
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}