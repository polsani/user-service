using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Domain.Entities;

namespace UserService.Domain.Data
{
    public interface IImportRepository
    {
        Task PreviousImportCreate(Import import);
        IEnumerable<Import> GetPreviousImportNotImported();
    }
}