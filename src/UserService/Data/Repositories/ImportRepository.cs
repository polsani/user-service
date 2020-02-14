using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using UserService.Configurations;
using UserService.Data.Helpers;
using UserService.Domain.Data.Repositories;
using UserService.Domain.Data.UnitOfWork;
using UserService.Domain.Entities;
using UserService.Domain.Enums;
using UserService.Domain.Models;

namespace UserService.Data.Repositories
{
    public class ImportRepository : RepositoryBase, IImportRepository
    {

        public ImportRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public Task PreviousImportCreate(Import import)
        {
            using (var sqlConnection = new SqlConnection(UserServiceConfiguration.ConnectionString))
            {
                sqlConnection.Open();

                using (var sqlTransaction = sqlConnection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    const string query = 
                        @"INSERT INTO Import (id, create_date, approved, amount_rows) 
                        VALUES (@id, @create_date, @approved, @amount_rows);";
                    
                    var importCreateCommand = new SqlCommand(query, sqlConnection, sqlTransaction);
                    
                    importCreateCommand.Parameters.Add(new SqlParameter("@id", import.Id));
                    importCreateCommand.Parameters.Add(new SqlParameter("@create_date", import.CreateDate));
                    importCreateCommand.Parameters.Add(new SqlParameter("@approved", import.Approved));
                    importCreateCommand.Parameters.Add(new SqlParameter("@amount_rows", import.PreviousImportItems.Count()));

                    importCreateCommand.ExecuteNonQuery();
                    
                    var objBulk = new BulkUploadToSql<PreviousImportItem>
                    {
                        InternalStore = import.PreviousImportItems,
                        TableName = "PreviousImportItem",
                        CommitBatchSize = 1000,
                        Connection = sqlConnection,
                        Transaction = sqlTransaction
                    };
                    
                    try
                    {
                        objBulk.BulkInsert();
                        sqlTransaction.Commit();
                    }
                    catch
                    {
                        sqlTransaction.Rollback();
                        sqlConnection.Close();
                        throw;
                    }
                }
            }
            
            return Task.CompletedTask;
        }

        public IEnumerable<Import> GetPreviousImportNotImported(int page, int pageSize)
        {
            return UnitOfWork.Context.Import
                .Where(x => x.ImportDate == null)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public Import GetImport(Guid id)
        {
            return UnitOfWork.Context.Import.FirstOrDefault(x => x.Id == id);
        }

        public void UpdateImport(Import import)
        {
            UnitOfWork.Context.Attach(import);
            UnitOfWork.Context.SaveChanges();
        }

        public IEnumerable<PreviousImportItem> GetPreviousImportItems(Guid importId)
        {
            return UnitOfWork.Context.PreviousImportItem.Where(x => x.ImportId == importId).ToList();
        }

        public IEnumerable<Import> GetImports(bool? approved, int page, int pageSize)
        {
            var query = UnitOfWork.Context.Import.Where(x=>x.ImportDate != null);

            if (approved.HasValue)
                query = query
                    .Where(x => x.Approved == approved.Value);

            return query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public ImportResult GetImportResult(Guid importId)
        {
            var import = UnitOfWork.Context.Import.FirstOrDefault(x => x.Id == importId);
            
            if(import != null)
                return new ImportResult
                {
                    Id = import.Id,
                    CreateDate = import.CreateDate,
                    AmountRows = import.AmountRows,
                    Failed = UnitOfWork.Context.PreviousImportItem.Count(x => 
                        x.Status == (int) PreviousImportItemStatus.Failed && x.ImportId == importId),
                    
                    Ignored = UnitOfWork.Context.PreviousImportItem.Count(x => 
                        x.Status == (int) PreviousImportItemStatus.Ignored && x.ImportId == importId),
                    
                    Inserted = UnitOfWork.Context.PreviousImportItem.Count(x => 
                        x.Status == (int) PreviousImportItemStatus.Inserted && x.ImportId == importId),
                    
                    Updated = UnitOfWork.Context.PreviousImportItem.Count(x => 
                        x.Status == (int) PreviousImportItemStatus.Updated && x.ImportId == importId)
                };

            return null;
        }
    }
}