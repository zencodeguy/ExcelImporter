using System;
using System.Data.SqlClient;

namespace zencodeguy.ExcelImporter.Mappers
{
    public class ToSqlBulkCopy
    {
        private ImportDefinition importDefinition;

        public ToSqlBulkCopy(ImportDefinition ImportDefinition)
        {
            this.importDefinition = ImportDefinition ?? throw new ArgumentNullException("ImportDefinition");
        }

        public SqlBulkCopy Map(string ConnectionString)
        {
            var sbc = new SqlBulkCopy(ConnectionString)
            {
                DestinationTableName = this.importDefinition.TableName
            };

            foreach (var c in this.importDefinition.Columns)
            {
                sbc.ColumnMappings.Add(c.PropertyName, c.PropertyName);
            }

            foreach (var dp in this.importDefinition.DestinationProperties)
            {
                sbc.ColumnMappings.Add(dp.PropertyName, dp.PropertyName);
            }

            return sbc;
        }
    }
}
