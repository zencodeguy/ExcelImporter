using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zencodeguy.ExcelImporter
{
    public sealed class DestinationPropertyDefinition
    {
        public string PropertyName { get; set; }
        public Enums.DataTypes DataType { get; set; }
        public bool Generate { get; set; }
        public bool GenerateDateOnly { get; set; }
        public string SetValue { get; set; }
        public bool Substitute { get; set; }
        public string SubstitutionName { get; set; }

        public string GetValueToSet()
        {
            if(Generate)
            {
                if (DataType == Enums.DataTypes.GUID)
                {
                    return Guid.NewGuid().ToString();
                }
                else if (DataType == Enums.DataTypes.DATETIME)
                {
                    if(this.GenerateDateOnly)
                    {
                        return System.DateTime.UtcNow.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        return System.DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
                    }
                }
                else
                {
                    throw new InvalidOperationException("Can only generate values for GUID types; Property " +
                        PropertyName + " is declared as type " + DataType.ToString());
                }
            }

            return SetValue;
        }
    }
}
