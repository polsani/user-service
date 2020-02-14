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

        public IEnumerable<Import> GetPreviousImportNotImported()
        {
            return _dbContext.Import.Where(x => x.ImportDate == null).ToList();
        }
    }
}