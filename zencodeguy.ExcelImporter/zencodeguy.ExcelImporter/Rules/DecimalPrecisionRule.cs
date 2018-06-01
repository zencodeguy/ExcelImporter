using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zencodeguy.ExcelImporter.Rules
{
    public class DecimalPrecisionRule: Rule
    {
        private int? precision;
        private int? scale;

        public DecimalPrecisionRule(string RuleName): base(RuleName) { }

        public int? PrecisionValue
        {
            get
            {
                return this.precision;
            }
        }

        public int? ScaleValue
        {
            get
            {
                return this.scale;
            }
        }

        public DecimalPrecisionRule Property(string PropertyName)
        {
            if(string.IsNullOrWhiteSpace(PropertyName))
            {
                throw new ArgumentNullException("PropertyName");
            }

            if(!string.IsNullOrWhiteSpace(this.propertyName))
            {
                throw new InvalidOperationException("Cannot call Property once it has been declared.");
            }

            this.propertyName = PropertyName;
            return this;
        }

        public DecimalPrecisionRule Precision(int Value)
        {
            this.CheckBounds(Value, "Precision");

            this.precision = Value;
            return this;
        }

        public DecimalPrecisionRule Scale(int Value)
        {
            this.CheckBounds(Value, "Scale");

            if(!this.precision.HasValue)
            {
                throw new InvalidOperationException("Scale cannot be declared until Precision has been set.");
            }

            if(Value > this.precision.Value)
            {
                throw new InvalidOperationException("Scale cannot be greater than Precision value of " + this.precision.Value.ToString());
            }

            this.scale = Value;
            return this;
        }

        private void CheckBounds(int Value, string Property)
        {
            if(string.IsNullOrWhiteSpace(this.propertyName))
            {
                throw new InvalidOperationException("Cannot specify " + Property + " until Property has been set.");
            }

            if (Value < 1)
            {
                throw new ArgumentOutOfRangeException(Property, "Decimal " + Property + " cannot be 0 or less.");
            }

            if(Value > 38)
            {
                throw new ArgumentOutOfRangeException(Property, "Decimal " + Property + " cannot be greater than 38.");
            }
        }

        protected override bool CanIsValidBeCalled()
        {
            if(string.IsNullOrWhiteSpace(this.propertyName))
            {
                throw new InvalidOperationException("Cannot call IsValid when Property is not set.");
            }

            if(!this.precision.HasValue)
            {
                throw new InvalidOperationException("Cannot call IsValid when Precision is not set.");
            }

            if(!this.scale.HasValue)
            {
                throw new InvalidOperationException("Cannot call IsValid when Scale is not set.");
            }

            return true;
        }

        public override List<string> GetPropertyNamesReferenced()
        {
            return new List<string>(1) { this.propertyName };
        }

        public override bool IsValid(Dictionary<string, string> Values)
        {
            this.errorMessage = null;

            this.CanIsValidBeCalled();

            this.CheckProvidedValues(Values);

            // If a value is required it will be tested separately.
            if(!Values.ContainsKey(this.propertyName))
            {
                return true;
            }

            var valueAsString = Values[this.propertyName];
            if(string.IsNullOrWhiteSpace(valueAsString))
            {
                return true;
            }

            if(!Decimal.TryParse(valueAsString, out decimal result))
            {
                this.errorMessage = this.propertyName + " value of \"" + valueAsString +
                    "\" cannot be interpreted as a Decimal value. Rule not applied.";
                return false;
            }

            var valueWithoutDecimal = valueAsString.Replace(".", string.Empty).Replace("-", string.Empty);
            if(valueWithoutDecimal.Length > this.precision.Value)
            {
                this.errorMessage = this.propertyName + " value of \"" + valueAsString + "\"" +
                    " is greater than the specified Precision of " + this.precision.Value.ToString();
                return false;
            }

            if (valueAsString.Contains("."))
            {
                var valueParts = Values[this.propertyName].Split('.');
                if (valueParts[1].Length > this.scale.Value)
                {
                    this.errorMessage = this.propertyName + " value of \"" + valueAsString + "\" " +
                        "has more digits in the decimal part of the number than the specified Scale of " +
                        this.scale.ToString();
                    return false;
                }
            }

            return true;
        }
    }
}
