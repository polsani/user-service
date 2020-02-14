using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using UserService.Configurations;
using UserService.Data.Contexts;
using UserService.Data.Helpers;
using UserService.Domain.Data;
using UserService.Domain.Entities;

namespace UserService.Data.Repositories
{
    public class ImportRepository : IImportRepository
    {
        private readonly DefaultContext _dbContext;

        public ImportRepository(DefaultContext dbContext)
        {
            _dbContext = dbContext;
        }
        
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
            return _dbContext.Import
                .Where(x => x.ImportDate == null)
                .Take(pageSize)
                .Skip((page - 1) * pageSize)
                .ToList();
        }

        public Import GetImport(Guid id)
        {
            return _dbContext.Import.FirstOrDefault(x => x.Id == id);
        }

        public void UpdateImport(Import import)
        {
            _dbContext.Attach(import);
            _dbContext.SaveChanges();
        }

        public IEnumerable<PreviousImportItem> GetPreviousImportItems(Guid importId)
        {
            return _dbContext.PreviousImportItem.Where(x => x.ImportId == importId).ToList();
        }

        public IEnumerable<Import> GetImports(bool? approved, int page, int pageSize)
        {
            var query = _dbContext.Import.Where(x=>x.ImportDate != null);

            if (approved.HasValue)
                query = query
                    .Where(x => x.Approved == approved.Value);

            return query
                .Take(pageSize)
                .Skip((page - 1) * pageSize)
                .ToList();
        }
    }
}