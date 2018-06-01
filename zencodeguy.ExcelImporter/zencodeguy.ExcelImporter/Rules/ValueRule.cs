using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using zencodeguy.ExcelImporter;

namespace zencodeguy.ExcelImporter.Rules
{
    /// <summary>
    /// This class represents a single validation rule for a single 
    /// property. The property must match one or more values in a
    /// list of valid values.
    /// </summary>
    public sealed class ValueRule: Rule
    {
        private ValueComparison valueComparison;
        private bool required = false;

        public ValueRule(string RuleName) : base(RuleName) { }
    
        public ValueComparison Comparison
        {
            get
            {
                return this.valueComparison;
            }
        }

        public ValueRule Property(string PropertyName)
        {
            if (!string.IsNullOrWhiteSpace(this.propertyName))
            {
                throw new InvalidOperationException("Property cannot be specified more than once.");
            }

            if (string.IsNullOrWhiteSpace(PropertyName))
            {
                throw new ArgumentNullException("PropertyName");
            }

            this.propertyName = PropertyName;
            return this;
        }

        public ValueRule Equals(string Value)
        {
            this.ArePreconditionsMetToCallOperator();

            this.IsValueNotNullEquivalent(Value);

            this.valueComparison = new ValueComparison(
                this.propertyName, 
                Enums.ComparisonOperators.EQ,
                new List<string>(1) { Value });
            return this;
        }

        public ValueRule NotEquals(string Value)
        {
            this.ArePreconditionsMetToCallOperator();

            this.IsValueNotNullEquivalent(Value);

            this.valueComparison = new ValueComparison(
                this.propertyName,
                Enums.ComparisonOperators.NOTEQ,
                new List<string>(1) { Value });
            return this;
        }

        public ValueRule In(List<string> Values)
        {
            this.ArePreconditionsMetToCallOperator();

            var sanitizedList = this.SanitizeList(Values);

            this.valueComparison = new ValueComparison(
                this.propertyName,
                Enums.ComparisonOperators.IN,
                sanitizedList);
            return this;
        }

        public ValueRule NotIn(List<string> Values)
        {
            this.ArePreconditionsMetToCallOperator();

            var sanitizedList = this.SanitizeList(Values);

            this.valueComparison = new ValueComparison(
                this.propertyName,
                Enums.ComparisonOperators.NOTIN,
                sanitizedList);
            return this;
        }

        public ValueRule Required()
        {
            this.required = true;
            return this;
        }

        protected override bool CanIsValidBeCalled()
        {
            if (string.IsNullOrWhiteSpace(this.propertyName))
            {
                throw new InvalidOperationException("Property Name is not set.");
            }
            if (this.valueComparison == null)
            {
                throw new InvalidOperationException("Comparison (Equals, NotEquals, In, NotIn) is not set.");
            }

            return true;
        }

        public override bool IsValid(Dictionary<string, string> Values)
        {
            this.CanIsValidBeCalled();
            this.CheckProvidedValues(Values);

            this.errorMessage = null;

            if(!Values.Keys.Contains(this.propertyName))
            {
                this.errorMessage = "Values does not contain a property named " + this.propertyName;
                return false;
            }

            var v = Values[this.propertyName];

            if(this.required && string.IsNullOrWhiteSpace(v))
            {
                this.errorMessage = this.propertyName + " is required, but is null, an empty string, or whitespace.";
                return false;
            }

            if(!this.required && string.IsNullOrWhiteSpace(v))
            {
                return true;
            }

            if(this.valueComparison.Compare(v))
            {
                return true;
            }
            else
            {
                this.errorMessage = this.propertyName + " value of '" + v + "' is invalid.";
                return false;
            }
        }


        public override List<string> GetPropertyNamesReferenced()
        {
            var l = new List<string>();
            l.Add(this.valueComparison.PropertyName);
            return l;
        }

        private void ArePreconditionsMetToCallOperator()
        {
            if (this.valueComparison != null)
            {
                throw new InvalidOperationException("Value Rule can only have on comparison on one property.");
            }

            if (string.IsNullOrWhiteSpace(this.propertyName))
            {
                throw new InvalidOperationException("Must specify Property before specifying operator.");
            }
        }

        private void IsValueNotNullEquivalent(string Value)
        {
            if(string.IsNullOrWhiteSpace(Value))
            {
                throw new ArgumentNullException("Value");
            }
        }

        private List<string> SanitizeList(List<string> SourceList)
        {
            var l = new List<String>();

            foreach (var s in SourceList)
            {
                if (!string.IsNullOrWhiteSpace(s))
                {
                    l.Add(s);
                }
            }

            if (l.Count == 0)
            {
                throw new ArgumentNullException("List does not contain any values that is not null, empty string, or whitespace.");
            }

            return l;
        }
    }
}
