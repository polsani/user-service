using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Domain.Helpers;
using UserService.Domain.Mappers;
using UserService.Domain.Services;
using UserService.ViewModels;

namespace UserService.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private readonly ISupportedMediaHelper _supportedMediaHelper;
        private readonly IImportService _importService;
        private readonly IImportMapper _importMapper;
        private readonly IImportResultMapper _importResultMapper;

        public UserController(ISupportedMediaHelper supportedMediaHelper,
            IImportService importService,
            IImportMapper importMapper,
            IImportResultMapper importResultMapper)
        {
            _supportedMediaHelper = supportedMediaHelper;
            _importService = importService;
            _importMapper = importMapper;
            _importResultMapper = importResultMapper;
        }
        
        [HttpPost]
        [Route("import")]
        public IActionResult PreviousImport(IFormFile file)
        {
            if (file == null || !_supportedMediaHelper.IsMediaSupported(file.ContentType))
                return StatusCode(415);

            var importRequest = _importService.PreviousImport(file);
            
            var importResult = new PreviousImportResult
            {
                CreateDate = importRequest.CreateDate.ToLocalTime(),
                ImportId = importRequest.Id,
                AmountRows = importRequest.PreviousImportItems.Count()
            };

            //TODO Need definition to correct use of status code
            return StatusCode(202, importResult);
        }

        [HttpGet]
        [Route("import")]
        public IActionResult GetImports([FromQuery] bool imported, 
            [FromQuery] bool? approved, 
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            if (page <= 0 || pageSize <= 0)
                return NoContent();
            
            if (!imported)
            {
                var previousImportNotImported = _importService.GetPreviousImportNotImported(page, pageSize);

                var result = previousImportNotImported.Select(x => new PreviousImportResult
                {
                    CreateDate = x.CreateDate.ToLocalTime(),
                    ImportId = x.Id,
                    AmountRows = x.AmountRows
                });
            
                return Json(result);
            }

            var imports = _importService.GetImports(approved, page, pageSize);
            var importsResult = imports.Select(item => _importMapper.ConvertToViewModel(item)).ToList();

            return Json(importsResult);
        }
        
        [HttpGet]
        [Route("import/{id}")]
        public IActionResult GetImportResult([FromRoute] Guid id)
        {
            var result = _importResultMapper.ConvertToViewModel(_importService.GetImportResult(id));

            if (result == null)
                return NotFound(id);
            
            return Json(result);
        }

        [HttpPost]
        [Route("import/{id}/approve")]
        public IActionResult ApproveImport([FromRoute] Guid id)
        {
            if(_importService.ApproveImport(id))
                return Accepted();

            return NotFound(id);
        }
        
        [HttpPost]
        [Route("import/{id}/reprove")]
        public IActionResult ReproveImport([FromRoute] Guid id)
        {
            if(_importService.ReproveImport(id))
                return Accepted();

            return NotFound(id);
        }
    }
}