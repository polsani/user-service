using System.Collections.Generic;
using System.Linq;
using UserService.Domain.Factories;
using UserService.Domain.Mappers;

namespace UserService.Factories
{
    public class MediaMapperFactory : IMediaMapperFactory
    {
        private readonly IEnumerable<IMediaMapper> _mappers;

        public MediaMapperFactory(IEnumerable<IMediaMapper> mappers)
        {
            _mappers = mappers;
        }
        public IMediaMapper GetMediaMapper(string contentType)
        {
            return _mappers.FirstOrDefault(mapper => mapper.CanMap(contentType));
        }
    }
}