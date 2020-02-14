using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Domain.Helpers;
using UserService.Domain.Services;
using UserService.ViewModels;

namespace UserService.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private readonly ISupportedMediaHelper _supportedMediaHelper;
        private readonly IImportService _importService;

        public UserController(ISupportedMediaHelper supportedMediaHelper,
            IImportService importService)
        {
            _supportedMediaHelper = supportedMediaHelper;
            _importService = importService;
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
        public IActionResult GetImports([FromQuery] bool imported, [FromQuery] bool? approved)
        {
            if (!imported)
            {
                var previousImportNotImported = _importService.GetPreviousImportNotImported();

                var result = previousImportNotImported.Select(x => new PreviousImportResult
                {
                    CreateDate = x.CreateDate.ToLocalTime(),
                    ImportId = x.Id,
                    AmountRows = x.AmountRows
                });
            
                return Json(result);
            }
            
            if (approved.HasValue)
                return approved.Value ? Json("1") : Json("2");

            return Json("3");
        }
        
        [HttpGet]
        [Route("import/{id}")]
        public IActionResult GetImportResult([FromRoute] Guid id)
        {
            var result = new ImportResult
            {
                Failed = 5,
                Ignored = 10,
                Inserted = 150,
                Updated = 20
            };
            
            return Json(result);
        }

        [HttpPost]
        [Route("import/{id}/approve")]
        public IActionResult ApproveImport([FromRoute] Guid id)
        {
            return Accepted();
        }
    }
}