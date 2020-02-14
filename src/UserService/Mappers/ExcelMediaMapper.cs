using System;
using System.Collections.Generic;
using System.IO;
using ExcelDataReader;
using UserService.Domain.Entities;
using UserService.Domain.Enums;
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
            var rows = new List<PreviousImportItem>();
            
            using (var reader = ExcelReaderFactory.CreateReader(file))
            {
                do
                {
                    while (reader.Read())
                    {
                        var name = reader.GetValue(0).ToString();
                        var email = reader.GetValue(1).ToString();
                        var birthDate = reader.GetValue(2).ToString();
                        var gender = reader.GetValue(3).ToString();

                        rows.Add(new PreviousImportItem
                        {
                            Email = email,
                            Gender = gender,
                            Id = Guid.NewGuid(),
                            Name = name,
                            Status = (int) PreviousImportItemStatus.WaitingForApproval,
                            BirthDate = birthDate,
                            ImportId = importId
                        });
                    }
                } while (reader.NextResult()); //Move to NEXT SHEET
            }

            return rows;
        }
    }
}