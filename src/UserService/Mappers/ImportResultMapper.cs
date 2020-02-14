using UserService.Domain.Mappers;
using UserService.ViewModels;

namespace UserService.Mappers
{
    public class ImportResultMapper : IImportResultMapper
    {
        public ImportResult ConvertToViewModel(Domain.Models.ImportResult importResult)
        {
            if (importResult == null)
                return null;

            return new ImportResult
            {
                Failed = importResult.Failed,
                Id = importResult.Id,
                Ignored = importResult.Ignored,
                Inserted = importResult.Inserted,
                Updated = importResult.Updated,
                AmountRows = importResult.AmountRows,
                CreateDate = importResult.CreateDate.ToLocalTime()
            };
        }
    }
}