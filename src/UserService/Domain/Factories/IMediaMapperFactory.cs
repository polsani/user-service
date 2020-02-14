using UserService.Domain.Mappers;

namespace UserService.Domain.Factories
{
    public interface IMediaMapperFactory
    {
        IMediaMapper GetMediaMapper(string contentType);
    }
}