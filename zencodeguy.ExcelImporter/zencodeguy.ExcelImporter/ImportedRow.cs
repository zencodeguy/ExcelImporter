using System;
using System.Collections.Generic;

using zencodeguy.ExcelImporter.Rules;
namespace zencodeguy.ExcelImporter
{
    public class ImportedRow
    {
        private int rowNumber;
        private Dictionary<string, string> columns;
        private List<string> errorMessages;

        public ImportedRow()
        {
            this.columns = new Dictionary<string, string>();
            this.errorMessages = new List<string>();
        }

        public ImportedRow(int RowNumber, string[] values, ImportDefinition ImportDefinition)
        {
            this.rowNumber = RowNumber;
            this.columns = new Dictionary<string, string>(values.Length);
            this.errorMessages = new List<string>();
            this.ParseColumnValues(values, ImportDefinition);
            this.ExecuteRules(ImportDefinition);
        }
        public int RowNumber
        {
            get
            {
                return this.rowNumber;
            }
        }

        public Dictionary<string, string> Columns
        {
            get
            {
                return this.columns;
            }
        }

        public List<string> ErrorMessages
        {
            get
            {
                return this.errorMessages;
            }
        }

        private void ParseColumnValues(string[] values, ImportDefinition ImportDefinition)
        {
            foreach(ColumnDefinition c in ImportDefinition.Columns)
            {
                if(c.ColumnNumber >= values.Length)
                {
                    this.errorMessages.Add("Column Number " + c.ColumnNumber.ToString() +
                        ", '" + c.PropertyName + "' is not present in read data.");
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(values[c.ColumnNumber]))
                    {
                        this.columns.Add(c.PropertyName, null);
                        continue;
                    }

                    if (IsDataType(c.DataType, values[c.ColumnNumber]))
                    {
                        this.columns.Add(c.PropertyName, values[c.ColumnNumber]);
                    }
                    else
                    {
                        this.errorMessages.Add("Column Number " + c.ColumnNumber.ToString() +
                            ", '" + c.PropertyName + "' contains a value that cannot be converted to " +
                            c.DataType.ToString() + ": " + values[c.ColumnNumber].ToString());
                    }
                }
            }
        }

        private bool IsDataType(Enums.DataTypes dataType, string value)
        {
            switch(dataType)
            {
                case Enums.DataTypes.BOOLEAN:
                    bool b;
                    return bool.TryParse(value, out b);
                case Enums.DataTypes.BYTE:
                    byte by;
                    return byte.TryParse(value, out by);
                case Enums.DataTypes.CHAR:
                    char c;
                    return char.TryParse(value, out c);
                case Enums.DataTypes.DATETIME:
                    DateTime d;
                    return DateTime.TryParse(value, out d);
                case Enums.DataTypes.DECIMAL:
                    Decimal de;
                    return Decimal.TryParse(value, out de);
                case Enums.DataTypes.DOUBLE:
                    Double dbl;
                    return Double.TryParse(value, out dbl);
                case Enums.DataTypes.GUID:
                    Guid g;
                    return Guid.TryParse(value, out g);
                case Enums.DataTypes.INT16:
                    Int16 i16;
                    return Int16.TryParse(value, out i16);
                case Enums.DataTypes.INT32:
                    Int32 i32;
                    return Int32.TryParse(value, out i32);
                case Enums.DataTypes.INT64:
                    Int64 i64;
                    return Int64.TryParse(value, out i64);
                case Enums.DataTypes.STRING:
                    return true;
                default:
                    return false;
            }
        }

        private void ExecuteRules(ImportDefinition id)
        {
            foreach(var rule in id.Rules)
            {
                if (!rule.IsValid(this.Columns))
                {
                    this.ErrorMessages.Add("Row " + this.rowNumber +
                        " failed rule " + rule.RuleName + ": " + rule.ErrorMessage);
                }
            }
        }
    }
}
