using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UserService.Domain.Data;
using UserService.Domain.Entities;
using UserService.Domain.Factories;
using UserService.Domain.Services;

namespace UserService.Services
{
    public class ImportService : IImportService
    {
        private readonly IMediaMapperFactory _mediaMapperFactory;
        private readonly IImportRepository _importRepository;

        public ImportService(IMediaMapperFactory mediaMapperFactory,
            IImportRepository importRepository)
        {
            _mediaMapperFactory = mediaMapperFactory;
            _importRepository = importRepository;
        }
        
        public Import PreviousImport(IFormFile file)
        {
            var import = new Import();
            var mediaMapper = _mediaMapperFactory.GetMediaMapper(file.ContentType);
            var itemsToImport = mediaMapper.Map(file.OpenReadStream(), import.Id);

            import.InitializeImport(itemsToImport);

            Task.Run(() => { _importRepository.PreviousImportCreate(import); });

            return import;
        }

        public IEnumerable<Import> GetPreviousImportNotImported()
        {
            return _importRepository.GetPreviousImportNotImported();
        }

        public void ApproveImport()
        {
            throw new System.NotImplementedException();
        }
    }
}