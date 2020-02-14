namespace UserService.Domain.Mappers
{
    public interface IUserMapper
    {
        ViewModels.User ConvertToEntity(Domain.Entities.User user);
        Entities.User ConvertToEntity(ViewModels.UserImportRequest userImportRequest);
    }
}