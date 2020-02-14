using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using UserService.Data.Annotations;

namespace UserService.Data.Helpers
{
    public static class BulkUploadToSqlHelperExtensions
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> data)
        {
            var properties = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();

            foreach (PropertyDescriptor prop in properties)
            {
                var type = typeof(T);
                var property = type.GetProperty(prop.Name);
                var attributes = (BulkInsertNotMapped[]) property?.GetCustomAttributes(typeof(BulkInsertNotMapped), false);

                if (attributes?.Length == 0)
                {
                    var attributeValues = (BulkInsertColumnName[]) property?.GetCustomAttributes(typeof(BulkInsertColumnName), false);
                    
                    table.Columns.Add(attributeValues.Length > 0 ? attributeValues[0].ColumnName : prop.Name, 
                        Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }
            }
                

            foreach (var item in data)
            {
                var row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    var type = typeof(T);
                    var property = type.GetProperty(prop.Name);
                    
                    var attributeValues = (BulkInsertColumnName[]) property?.GetCustomAttributes(typeof(BulkInsertColumnName), false);

                    var columnName = attributeValues?.Length > 0 ? attributeValues[0].ColumnName : prop.Name;
                    
                    if(table.Columns.Contains(columnName))
                        row[columnName] = prop.GetValue(item) ?? DBNull.Value;
                }
                    

                table.Rows.Add(row);
            }

            return table;
        }
    }
}