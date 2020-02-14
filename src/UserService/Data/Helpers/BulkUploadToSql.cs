using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace UserService.Data.Helpers
{
    public class BulkUploadToSql<T>
    {
        public IEnumerable<T> InternalStore { get; set; }
        public string TableName { get; set; }
        public int CommitBatchSize { get; set; } = 1000;
        public SqlConnection Connection { get; set; }
        public SqlTransaction Transaction { get; set; }

        public void BulkInsert()
        {
            if (!InternalStore.Any())
                return;

            var numberOfPages = (InternalStore.Count() / CommitBatchSize) + 
                                (InternalStore.Count() % CommitBatchSize == 0 ? 0 : 1);
            
            for (var pageIndex = 0; pageIndex < numberOfPages; pageIndex++)
            {
                var dt = InternalStore.Skip(pageIndex * CommitBatchSize).Take(CommitBatchSize).ToDataTable();
                BulkInsert(dt);
            }
        }

        private void BulkInsert(DataTable dt)
        {
            var bulkCopy = new SqlBulkCopy(Connection, 
                SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers, 
                Transaction)
            {
                DestinationTableName = TableName
            };

            bulkCopy.WriteToServer(dt);
        }
    }
}