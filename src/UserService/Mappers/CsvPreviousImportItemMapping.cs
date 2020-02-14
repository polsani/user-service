using TinyCsvParser.Mapping;
using UserService.Domain.Entities;

namespace UserService.Mappers
{
    public class CsvPreviousImportItemMapping : CsvMapping<PreviousImportItem>
    {
        public CsvPreviousImportItemMapping()
        {
            MapProperty(0, x => x.Name);
            MapProperty(1, x => x.Email);
            MapProperty(2, x => x.BirthDate);
            MapProperty(3, x => x.Gender);
        }
    }
}