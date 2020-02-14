using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Polly;
using UserService.Domain.Data.Repositories;
using UserService.Domain.Data.UnitOfWork;
using UserService.Domain.Factories;
using UserService.Domain.Mappers;
using UserService.Domain.Models;
using UserService.Domain.Services;
using Import = UserService.Domain.Entities.Import;

namespace UserService.Services
{
    public class ImportService : IImportService
    {
        private readonly IMediaMapperFactory _mediaMapperFactory;
        private readonly IImportRepository _importRepository;
        private readonly MessagingService _messagingService;
        private readonly IUserToImportMapper _userToImportMapper;
        private readonly IUnitOfWork _unitOfWork;

        public ImportService(IMediaMapperFactory mediaMapperFactory,
            IImportRepository importRepository,
            IUserToImportMapper userToImportMapper,
            IUnitOfWork unitOfWork)
        {
            _messagingService = MessagingServiceSingleton.Instance;
            _mediaMapperFactory = mediaMapperFactory;
            _importRepository = importRepository;
            _userToImportMapper = userToImportMapper;
            _unitOfWork = unitOfWork;
        }
        
        public Import PreviousImport(IFormFile file)
        {
            
            var import = new Import();
            var mediaMapper = _mediaMapperFactory.GetMediaMapper(file.ContentType);
            var itemsToImport = mediaMapper.Map(file.OpenReadStream(), import.Id);

            import.InitializeImport(itemsToImport);
            
            var policy =
                Policy
                    .Handle<Exception>()
                    .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt * 10));

            Task.Run(() =>
            {
                policy.Execute(() => _importRepository.PreviousImportCreate(import));
            });

            return import;
        }

        public IEnumerable<Import> GetPreviousImportNotImported(int page, int pageSize)
        {
            _unitOfWork.InitializeContext();
            
            return _importRepository.GetPreviousImportNotImported(page, pageSize);
        }

        public bool ApproveImport(Guid importId)
        {
            _unitOfWork.InitializeContext();
            
            var import = _importRepository.GetImport(importId);
            if (import == null)
                return false;

            var itemsToImport = _importRepository.GetPreviousImportItems(importId);

            var serializedUsersToImport = itemsToImport.Select(item => 
                JsonConvert.SerializeObject(_userToImportMapper.ConvertToUserImportRequest(item))).ToList();

            _messagingService.SendUsersImports(serializedUsersToImport);

            import.Approved = true;
            import.ImportDate = DateTime.UtcNow;
            _importRepository.UpdateImport(import);
            
            _unitOfWork.Commit();
            return true;
        }

        public bool ReproveImport(Guid importId)
        {
            _unitOfWork.InitializeContext();
            
            var import = _importRepository.GetImport(importId);
            if (import == null)
                return false;

            import.Approved = false;
            _importRepository.UpdateImport(import);
            
            _unitOfWork.Commit();
            
            return true;
        }

        public IEnumerable<Import> GetImports(bool? approved, int page, int pageSize)
        {
            _unitOfWork.InitializeContext();
            return _importRepository.GetImports(approved, page, pageSize);
        }

        public ImportResult GetImportResult(Guid importId)
        {
            _unitOfWork.InitializeContext();
            return _importRepository.GetImportResult(importId);
        }
    }
}