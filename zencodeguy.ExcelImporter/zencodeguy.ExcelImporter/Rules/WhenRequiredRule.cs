using System;
using System.Collections.Generic;

namespace zencodeguy.ExcelImporter.Rules
{
    public class WhenRequiredRule: Rule
    {
        List<ValueComparison> whenComparisons;
        private bool whenClauseComplete = false;
        List<string> requiredProperties;

        public WhenRequiredRule(string RuleName) : base(RuleName)
        {
            this.whenComparisons = new List<ValueComparison>();
            this.requiredProperties = new List<string>();
        }

        public List<ValueComparison> WhenComparisons
        {
            get
            {
                return this.whenComparisons;
            }
        }

        public List<string> RequiredProperties
        {
            get
            {
                return this.requiredProperties;
            }
        }

        public WhenRequiredRule Property(string PropertyName)
        {
            if (string.IsNullOrWhiteSpace(PropertyName))
            {
                throw new ArgumentNullException("PropertyName");
            }

            if(!string.IsNullOrWhiteSpace(this.propertyName))
            {
                throw new InvalidOperationException("Cannot define another property name until a comparison operator for the current property name is declared.");
            }

            if(this.whenClauseComplete)
            {
                throw new InvalidOperationException("Cannot define another property name once Then has been called. Use Required instead.");

            }
            this.propertyName = PropertyName;
            return this;
        }

        public WhenRequiredRule Then()
        {
            if (this.propertyName != null)
            {
                throw new InvalidOperationException("Then called before previous comparison clause completed.");
            }
            else if (this.whenComparisons.Count == 0)
            {
                throw new InvalidOperationException("Cannot call Then when no When comparisons have been set.");
            }
            else
            {
                this.whenClauseComplete = true;
                return this;
            }
        }

        public WhenRequiredRule Equals(string Value)
        {
            this.CanCallSingleValueOperator(Value);

            this.whenComparisons.Add(new ValueComparison(
                this.propertyName,
                Enums.ComparisonOperators.EQ,
                new List<string>(1) { Value }));

            this.propertyName = null;
            return this;
        }

        public WhenRequiredRule NotEquals(string Value)
        {
            this.CanCallSingleValueOperator(Value);

            this.whenComparisons.Add(new ValueComparison(
                this.propertyName,
                Enums.ComparisonOperators.NOTEQ,
                new List<string>(1) { Value }));

            this.propertyName = null;
            return this;
        }


        public WhenRequiredRule In(List<string> Values)
        {
            this.CanCallListValueOperator(Values);

            this.whenComparisons.Add(new ValueComparison(
                this.propertyName,
                Enums.ComparisonOperators.IN,
                Values));

            this.propertyName = null;
            return this;
        }

        public WhenRequiredRule NotIn(List<string> Values)
        {
            this.CanCallListValueOperator(Values);

            this.whenComparisons.Add(new ValueComparison(
                this.propertyName,
                Enums.ComparisonOperators.NOTIN,
                Values));

            this.propertyName = null;
            return this;
        }

        public WhenRequiredRule Required(string Property)
        {
            if(string.IsNullOrWhiteSpace(Property))
            {
                throw new ArgumentNullException("Property");
            }

            this.requiredProperties.Add(Property);
            return this;
        }

        public override bool IsValid(Dictionary<string, string> Values)
        {
            this.CanIsValidBeCalled();

            var isValid = true;
            this.errorMessage = null;

            if (DoesWhenClauseApply(Values))
            {
                foreach (var p in this.requiredProperties)
                {
                    if(string.IsNullOrWhiteSpace(Values[p]))
                    {
                        if(this.errorMessage == null)
                        {
                            this.errorMessage = "The following properties are required but not provided:";
                        }
                        this.errorMessage += " " + p;
                        isValid = false;
                    }
                }
            }

            return isValid;
        }

        
        private bool DoesWhenClauseApply(Dictionary<string, string> Values)
        {
            foreach (var c in this.whenComparisons)
            {
                if(!Values.ContainsKey(c.PropertyName))
                {
                    // If the required property to test is not in the 
                    // dictionary, the test cannot evaluate to true
                    return false;
                }
                // If the value of the When clause is null,
                // the when clause can never invoke
                if (string.IsNullOrWhiteSpace(Values[c.PropertyName]))
                {
                    return false;
                }

                if (!c.Compare(Values[c.PropertyName]))
                {
                    return false;
                }
            }

            return true;
        }

        public override List<string> GetPropertyNamesReferenced()
        {
            var l = new List<string>();
            foreach (var c in this.WhenComparisons)
            {
                l.Add(c.PropertyName);
            }
            l.AddRange(this.requiredProperties);
            return l;
        }

        private void CanCallOperator()
        {
            if (this.whenClauseComplete)
            {
                throw new InvalidOperationException("Cannot call comparison operator after Then() has been called.");
            }

            if (string.IsNullOrWhiteSpace(this.propertyName))
            {
                throw new InvalidOperationException("Cannot call operator before the property is declared.");
            }
        }

        private bool CanCallSingleValueOperator(string Value)
        {
            this.CanCallOperator();

            if(string.IsNullOrWhiteSpace(Value))
            {
                throw new ArgumentNullException("Value");
            }
            return true;
        }

        private bool CanCallListValueOperator(List<string> Values)
        {
            this.CanCallOperator();

            if(Values.Count == 0)
            {
                throw new ArgumentException("Values list contains no values.");
            }

            foreach(var s in Values)
            {
                if(string.IsNullOrWhiteSpace(s))
                {
                    throw new ArgumentNullException("Values", "At least one value passed is null or null equivalent.");
                }
            }

            return true;
        }

        protected override bool CanIsValidBeCalled()
        {
            if(this.WhenComparisons.Count == 0)
            {
                throw new InvalidOperationException("Cannot vall IsValid when no WHEN comparisons are defined.");
            }

            if(this.requiredProperties.Count == 0)
            {
                throw new InvalidOperationException("Cannot call IsValid when no required properties are declared.");
            }

            return true;
        }
    }
}
