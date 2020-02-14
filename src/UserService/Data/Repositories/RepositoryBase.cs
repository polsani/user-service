using UserService.Domain.Data.UnitOfWork;

namespace UserService.Data.Repositories
{
    public abstract class RepositoryBase
    {
        protected readonly UnitOfWork.UnitOfWork UnitOfWork;

        protected RepositoryBase(IUnitOfWork unitOfWork) =>
            UnitOfWork = (UnitOfWork.UnitOfWork)unitOfWork;
    }
}