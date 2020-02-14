namespace UserService.Domain.Mappers
{
    public interface IImportResultMapper
    {
        ViewModels.ImportResult ConvertToViewModel(Models.ImportResult importResult);
    }
}