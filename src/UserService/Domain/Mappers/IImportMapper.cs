namespace UserService.Domain.Mappers
{
    public interface IImportMapper
    {
        ViewModels.Import ConvertToViewModel(Entities.Import item);
    }
}