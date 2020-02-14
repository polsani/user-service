using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TinyCsvParser;
using UserService.Domain.Entities;
using UserService.Domain.Enums;
using UserService.Domain.Mappers;

namespace UserService.Mappers
{
    public class CsvMediaMapper : IMediaMapper
    {
        public bool CanMap(string contentType)
        {
            return contentType.Equals("text/csv");
        }

        public IEnumerable<PreviousImportItem> Map(Stream file, Guid importId)
        {
            var csvParser = new CsvParser<PreviousImportItem>(new CsvParserOptions(false, ','), 
                new CsvPreviousImportItemMapping());

            var result = csvParser.ReadFromStream(file, Encoding.UTF8).ToList();
            
            return result.Select(x => new PreviousImportItem
            {
                Id = Guid.NewGuid(),
                Email = x.Result.Email,
                Gender = x.Result.Gender,
                Name = x.Result.Name,
                BirthDate = x.Result.BirthDate,
                Status = (int)PreviousImportItemStatus.WaitingForApproval,
                ImportId = importId
            });
        }
    }
}