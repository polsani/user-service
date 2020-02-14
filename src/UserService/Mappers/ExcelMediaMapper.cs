using System;
using System.Collections.Generic;
using System.IO;
using UserService.Domain.Entities;
using UserService.Domain.Mappers;

namespace UserService.Mappers
{
    public class ExcelMediaMapper : IMediaMapper
    {
        public bool CanMap(string contentType)
        {
            return contentType.Equals("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public IEnumerable<PreviousImportItem> Map(Stream file, Guid importId)
        {
            throw new System.NotImplementedException();
        }
    }
}