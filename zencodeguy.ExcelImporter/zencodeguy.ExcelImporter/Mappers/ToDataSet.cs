using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using zencodeguy.ExcelImporter;

namespace zencodeguy.ExcelImporter.Mappers
{
    public class ToDataSet: MapperBase
    {
        public ToDataSet(ImportDefinition ImportDefinition)
        {
            this.importDefinition = ImportDefinition ?? throw new ArgumentNullException("ImportDefinition");
        }

        public DataTable Map(ImportResult ImportResult,
            Dictionary<string,string> Substitutions = null)
        {
            this.importResult = ImportResult ?? throw new ArgumentNullException("ImportResult");
            this.substitutions = Substitutions;

            this.CanMap();

            if (this.importResult.Rows.Count == 0)
            {
                throw new ArgumentOutOfRangeException("ImportResult", "No rows to map.");
            }

            var dt = CreateDataTable(this.importDefinition);

            var rows = PopulateDataTable(dt, this.importResult, this.importDefinition);

            return dt;
        }

        private DataTable CreateDataTable(ImportDefinition id)
        {
            if(id == null)
            {
                throw new InvalidOperationException("ImportDefinition is null.");
            }

            DataTable dt = new DataTable();

            foreach(var c in id.Columns)
            {
                dt.Columns.Add(CreateColumn(c));
            }

            foreach(var dp in id.DestinationProperties)
            {
                dt.Columns.Add(CreateColumn(dp));
            }

            return dt;
        }

        private DataColumn CreateColumn(ColumnDefinition c)
        {
            return new DataColumn()
            {
                ColumnName = c.PropertyName,
                AllowDBNull = true,
                DataType = GetColumnType(c.DataType)
            };
        }

        private DataColumn CreateColumn(DestinationPropertyDefinition dp)
        {
            return new DataColumn()
            {
                ColumnName = dp.PropertyName,
                AllowDBNull = true,
                DataType = GetColumnType(dp.DataType)
            };
        }

        private Type GetColumnType(Enums.DataTypes dt)
        {
            switch(dt)
            {
                case Enums.DataTypes.BOOLEAN:
                    return typeof(bool);
                case Enums.DataTypes.BYTE:
                    return typeof(byte);
                case Enums.DataTypes.CHAR:
                    return typeof(char);
                case Enums.DataTypes.DATETIME:
                    return typeof(DateTime);
                case Enums.DataTypes.DECIMAL:
                    return typeof(Decimal);
                case Enums.DataTypes.DOUBLE:
                    return typeof(Double);
                case Enums.DataTypes.GUID:
                    return typeof(Guid);
                case Enums.DataTypes.INT16:
                    return typeof(Int16);
                case Enums.DataTypes.INT32:
                    return typeof(Int32);
                case Enums.DataTypes.INT64:
                    return typeof(Int64);
                case Enums.DataTypes.STRING:
                    return typeof(string);
                default:
                    throw new ArgumentException("Data type of " + dt.ToString() + " is not mapped to a data type.");
            }
        }

        private int PopulateDataTable(DataTable dt, ImportResult ir, ImportDefinition id)
        {
            int rowCount = 0;

            foreach(var r in ir.Rows)
            {
                rowCount++;
                dt.Rows.Add(CreateRow(dt, r, id));
            }

            return rowCount;
        }

        private DataRow CreateRow(DataTable dt, ImportedRow ir, ImportDefinition id)
        {
            var dr = dt.NewRow();

            foreach(var c in ir.Columns)
            {
                CastValueToType(c.Key, c.Value, id.Columns.Single(x => x.PropertyName == c.Key).DataType, dr);
            }

            foreach(var dp in id.DestinationProperties)
            {
                if (dp.Substitute)
                {
                    CastValueToType(dp.PropertyName,
                        this.substitutions[dp.SubstitutionName],
                        dp.DataType, dr);
                }
                else
                {
                    CastValueToType(dp.PropertyName, dp.GetValueToSet(), dp.DataType, dr);
                }
            }


            return dr;
        }

        private void CastValueToType(string Key, string Value, Enums.DataTypes dataType, DataRow dr)
        {
            if(string.IsNullOrWhiteSpace(Value))
            {
                dr[Key] = DBNull.Value;
                return;
            }

            switch (dataType)
            {
                case Enums.DataTypes.BOOLEAN:
                    dr[Key] = bool.Parse(Value);
                    break;
                case Enums.DataTypes.BYTE:
                    dr[Key] = byte.Parse(Value);
                    break;
                case Enums.DataTypes.CHAR:
                    dr[Key] = char.Parse(Value);
                    break;
                case Enums.DataTypes.DATETIME:
                    dr[Key] = DateTime.Parse(Value).ToUniversalTime();
                    break;
                case Enums.DataTypes.DECIMAL:
                    dr[Key] = Decimal.Parse(Value);
                    break;
                case Enums.DataTypes.DOUBLE:
                    dr[Key] = Double.Parse(Value);
                    break;
                case Enums.DataTypes.GUID:
                    dr[Key] = Guid.Parse(Value);
                    break;
                case Enums.DataTypes.INT16:
                    dr[Key] = Int16.Parse(Value);
                    break;
                case Enums.DataTypes.INT32:
                    dr[Key] = Int32.Parse(Value);
                    break;
                case Enums.DataTypes.INT64:
                    dr[Key] = Int64.Parse(Value);
                    break;
                case Enums.DataTypes.STRING:
                    dr[Key] = Value;
                    break;
                default:
                    throw new ArgumentException("Data type of " + dataType.ToString() + " is not mapped to a data type.");
            }

        }
    }
}
