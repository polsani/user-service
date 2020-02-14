namespace UserService.Domain.Mappers
{
    public interface IUserMapper
    {
        ViewModels.User ConvertToViewModel(Entities.User user);
        Entities.User ConvertToEntity(ViewModels.User user);
    }
}