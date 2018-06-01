using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using zencodeguy.ExcelImporter;

namespace zencodeguy.ExcelImporter.Mappers
{
    public class ToClass<T>: MapperBase
    {
        public ToClass(ImportDefinition ImportDefinition)
        {
            this.importDefinition = ImportDefinition ?? throw new ArgumentNullException("ImportDefinition");
        }

        public List<T> Map(ImportResult ImportResult,
            Dictionary<string, string> Substitutions = null)
        {
            this.importResult = ImportResult ?? throw new ArgumentNullException("ImportResult");

            this.substitutions = Substitutions;

            this.CanMap();

            var l = new List<T>();

            foreach (var row in this.importResult.Rows)
            {
                l.Add(CreateObject(this.importDefinition, row));
            }

            return l;
        }

        private T CreateObject(ImportDefinition id, ImportedRow ir)
        {
            var obj = (T)Activator.CreateInstance(typeof(T));

            foreach (var c in ir.Columns)
            {
                PopulateProperty(obj, c.Key, id.Columns.Single(x => x.PropertyName == c.Key).DataType, c.Value);
            }

            foreach (var dp in id.DestinationProperties)
            {
                if (dp.Substitute)
                {
                    PopulateProperty(obj, dp.PropertyName, dp.DataType, substitutions[dp.SubstitutionName]);
                }
                else
                {
                    PopulateProperty(obj, dp.PropertyName, dp.DataType, dp.GetValueToSet());
                }
            }

            return (T)obj;
        }

        private void PopulateProperty(T obj, string propertyName, Enums.DataTypes dataType, string value)
        {
            PropertyInfo prop;

            try
            {
                prop = obj.GetType().GetProperty(propertyName);
            }
            catch
            {
                throw new InvalidOperationException("Cannot populate property " + propertyName +
                    " of object type " + obj.GetType().Name + " because the type does not expose a property by that name.");
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                if ((prop.PropertyType.IsGenericType &&
                    prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) ||
                    !prop.PropertyType.IsValueType)
                {
                    prop.SetValue(obj, null);
                    return;
                }
                else
                {
                    throw new InvalidOperationException("Cannot populate non-nullable property with null value.");
                }
            }
            else
            {
                CastValueToType(obj, prop, value, dataType);
            }
        }
        private void CastValueToType(T obj, PropertyInfo prop, string value, Enums.DataTypes dataType)
        {
            switch (dataType)
            {
                case Enums.DataTypes.BOOLEAN:
                    prop.SetValue(obj, bool.Parse(value));
                    break;
                case Enums.DataTypes.BYTE:
                    prop.SetValue(obj, byte.Parse(value));
                    break;
                case Enums.DataTypes.CHAR:
                    prop.SetValue(obj, char.Parse(value));
                    break;
                case Enums.DataTypes.DATETIME:
                    prop.SetValue(obj, DateTime.Parse(value).ToUniversalTime());
                    break;
                case Enums.DataTypes.DECIMAL:
                    prop.SetValue(obj, Decimal.Parse(value));
                    break;
                case Enums.DataTypes.DOUBLE:
                    prop.SetValue(obj, Double.Parse(value));
                    break;
                case Enums.DataTypes.GUID:
                    prop.SetValue(obj, Guid.Parse(value));
                    break;
                case Enums.DataTypes.INT16:
                    prop.SetValue(obj, Int16.Parse(value));
                    break;
                case Enums.DataTypes.INT32:
                    prop.SetValue(obj, Int32.Parse(value));
                    break;
                case Enums.DataTypes.INT64:
                    prop.SetValue(obj, Int64.Parse(value));
                    break;
                case Enums.DataTypes.STRING:
                    prop.SetValue(obj, value);
                    break;
                default:
                    throw new ArgumentException("Data type of " + dataType.ToString() + " is not mapped to a valid data type.");
            }
        }
    }
}
