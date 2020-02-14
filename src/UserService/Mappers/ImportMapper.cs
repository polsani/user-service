using UserService.Domain.Mappers;
using UserService.ViewModels;

namespace UserService.Mappers
{
    public class ImportMapper : IImportMapper
    {
        public Import ConvertToViewModel(Domain.Entities.Import item)
        {
            return new Import
            {
                Approved = item.Approved,
                Id = item.Id,
                AmountRows = item.AmountRows,
                CreateDate = item.CreateDate,
                ImportDate = item.ImportDate
            };
        }
    }
}