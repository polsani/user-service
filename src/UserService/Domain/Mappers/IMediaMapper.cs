using System;
using System.Collections.Generic;
using System.IO;
using UserService.Domain.Entities;

namespace UserService.Domain.Mappers
{
    public interface IMediaMapper
    {
        bool CanMap(string contentType);
        IEnumerable<PreviousImportItem> Map(Stream file, Guid importId);
    }
}