using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zencodeguy.ExcelImporter.Parsers
{
    public static class Helpers
    {
        public static int ParseNextTokenAsInteger(string[] Tokens, int CurrentToken)
        {
            if (CurrentToken < Tokens.Length)
            {
                var value = Tokens[CurrentToken + 1];

                if (Int32.TryParse(value, out int valueAsInt))
                {
                    return valueAsInt;
                }
                else
                {
                    throw new InvalidCastException("Invalid Token: Cannot cast value " + Tokens[CurrentToken + 1] + " to an integer.");
                }
            }

            throw new IndexOutOfRangeException("Attempt to parse integer at position " + 
                CurrentToken.ToString() + 
                ", array contains " + 
                Tokens.Count().ToString() + 
                " values.");
        }

        public static bool IsColumnNumberAlreadyDefined(int ColumnNumber, ImportDefinition ID)
        {
            if(ID == null)
            {
                throw new ArgumentNullException("ID");
            }

            return ID.Columns.Where(x => x.ColumnNumber == ColumnNumber).Count() > 0;
        }

        public static bool IsPropertyNameAlreadyDefined(string PropertyName, ImportDefinition ID)
        {
            if(string.IsNullOrWhiteSpace(PropertyName))
            {
                throw new ArgumentNullException("PropertyName");
            }

            if(ID == null)
            {
                throw new ArgumentNullException("ID");
            }

            return ID.Columns.Where(x => x.PropertyName == PropertyName).Count() > 0;
        }

        public static bool IsDestinationPropertyAlreadyDefined(string PropertyName, ImportDefinition ID)
        {
            if(string.IsNullOrWhiteSpace(PropertyName))
            {
                throw new ArgumentNullException("PropertyName");
            }

            if(ID == null)
            {
                throw new ArgumentNullException("ID");
            }

            return ID.DestinationProperties.Where(x => x.PropertyName == PropertyName).Count() > 0;
        }

        public static bool ParametersValid(string Line, ImportDefinition ID, string ColumnName = null)
        {
            if (string.IsNullOrWhiteSpace(Line))
            {
                throw new ArgumentNullException("Line");
            }

            if (ID == null)
            {
                throw new ArgumentNullException("ID");
            }

            if(ColumnName == null)
            {
                return true;
            }

            if (!Line.StartsWith(ColumnName))
            {
                throw new ArgumentException("Line is not a " + ColumnName + " declaration.");
            }

            return true;
        }

        public static bool CanSetValueBeParsedAsType(string value, Enums.DataTypes dataType)
        {
            switch (dataType)
            {
                case Enums.DataTypes.BOOLEAN:
                    return bool.TryParse(value, out bool boolResult);
                case Enums.DataTypes.BYTE:
                    return byte.TryParse(value, out byte byteResult);
                case Enums.DataTypes.CHAR:
                    return char.TryParse(value, out char charResult);
                case Enums.DataTypes.DATETIME:
                    return DateTime.TryParse(value, out DateTime dtResult);
                case Enums.DataTypes.DECIMAL:
                    return Decimal.TryParse(value, out Decimal decimalResult);
                case Enums.DataTypes.DOUBLE:
                    return Double.TryParse(value, out Double doubleResult);
                case Enums.DataTypes.GUID:
                    return Guid.TryParse(value, out Guid guidResult);
                case Enums.DataTypes.INT16:
                    return Int16.TryParse(value, out Int16 int16Result);
                case Enums.DataTypes.INT32:
                    return Int32.TryParse(value, out Int32 int32Result);
                case Enums.DataTypes.INT64:
                    return Int64.TryParse(value, out Int64 int64Result);
                case Enums.DataTypes.STRING:
                    return true;
                default:
                    throw new ArgumentException("Data type of " + dataType.ToString() + " is not mapped to a data type.");
            }
        }
    }
}
