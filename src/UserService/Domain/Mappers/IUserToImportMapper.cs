namespace UserService.Domain.Mappers
{
    public interface IUserToImportMapper
    {
        ViewModels.UserImportRequest ConvertToUserImportRequest(Entities.PreviousImportItem item);
    }
}