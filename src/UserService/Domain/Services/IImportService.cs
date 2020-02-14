using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using UserService.Domain.Entities;
using UserService.Domain.Models;

namespace UserService.Domain.Services
{
    public interface IImportService
    {
        Import PreviousImport(IFormFile file);
        IEnumerable<Import> GetPreviousImportNotImported(int page, int pageSize);
        bool ApproveImport(Guid importId);
        bool ReproveImport(Guid importId);
        IEnumerable<Import> GetImports(bool? approved, int page, int pageSize);
        ImportResult GetImportResult(Guid importId);
    }
}