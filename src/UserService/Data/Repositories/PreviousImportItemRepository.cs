using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Data.Repositories;
using UserService.Domain.Data.UnitOfWork;
using UserService.Domain.Entities;

namespace UserService.Data.Repositories
{
    public class PreviousImportItemRepository : RepositoryBase, IPreviousImportItemRepository
    {
        public PreviousImportItemRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public PreviousImportItem GetPreviousImportItem(Guid id)
        {
            return UnitOfWork.Context.PreviousImportItem.FirstOrDefault(x => x.Id == id);
        }

        public void UpdatePreviousImportItem(PreviousImportItem previousImportItem)
        {
            UnitOfWork.Context.Attach(previousImportItem);
            UnitOfWork.Context.Entry(previousImportItem).State = EntityState.Modified;
        }
    }
}