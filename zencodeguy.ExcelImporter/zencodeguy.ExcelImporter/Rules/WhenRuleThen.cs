using System;
using System.Collections.Generic;

using zencodeguy.ExcelImporter;

namespace zencodeguy.ExcelImporter.Rules
{
    public class WhenRuleThen : Rule
    {
        List<ValueComparison> whenComparisons;
        List<ValueComparison> thenComparisons;
        private bool whenClauseComplete = false;

        public WhenRuleThen(string RuleName) : base(RuleName)
        {
            this.whenComparisons = new List<ValueComparison>();
            this.thenComparisons = new List<ValueComparison>();
        }

        public List<ValueComparison> WhenComparisons
        {
            get
            {
                return this.whenComparisons;
            }
        }

        public List<ValueComparison> ThenComparisons
        {
            get
            {
                return this.thenComparisons;
            }
        }

        public WhenRuleThen Property(string PropertyName)
        {
            if (string.IsNullOrWhiteSpace(PropertyName))
            {
                throw new ArgumentNullException("PropertyName");
            }

            this.propertyName = PropertyName;
            return this;
        }

        public WhenRuleThen Then()
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
                whenClauseComplete = true;
                return this;
            }
        }

        public WhenRuleThen Equals(string Value)
        {
            var v = new ValueComparison(
                this.propertyName,
                Enums.ComparisonOperators.EQ,
                new List<string>(1) { Value });
            this.AddComparison(v);

            return this;
        }

        public WhenRuleThen NotEquals(string Value)
        {
            var v = new ValueComparison(
                this.propertyName,
                Enums.ComparisonOperators.NOTEQ,
                new List<string>(1) { Value });
            this.AddComparison(v);

            return this;
        }

        public WhenRuleThen In(List<string> Values)
        {
            var v = new ValueComparison(
                this.propertyName,
                Enums.ComparisonOperators.IN,
                Values);
            this.AddComparison(v);

            return this;
        }

        public WhenRuleThen NotIn(List<string> Values)
        {
            var v = new ValueComparison(
                this.propertyName,
                Enums.ComparisonOperators.NOTIN,
                Values);
            this.AddComparison(v);

            return this;
        }

        private void AddComparison(ValueComparison v)
        {
            if (!this.whenClauseComplete)
            {
                this.whenComparisons.Add(v);
            }
            else
            {
                this.thenComparisons.Add(v);
            }

            this.propertyName = null;
        }

        protected override bool CanIsValidBeCalled()
        {
            if (!string.IsNullOrWhiteSpace(this.propertyName))
            {
                throw new InvalidOperationException("Cannot call Isvalid when a property was specified with no comparison or values.");
            }

            if (this.WhenComparisons.Count == 0)
            {
                throw new InvalidOperationException("Cannot call IsValid when no WHEN comparisons are defined.");
            }

            if (this.ThenComparisons.Count == 0)
            {
                throw new InvalidOperationException("Cannot call IsValid when no THEN comparisons are defined.");
            }

            return true;
        }
        public override bool IsValid(Dictionary<string, string> Values)
        {
            this.errorMessage = null;

            this.CanIsValidBeCalled();

            this.CheckProvidedValues(Values);

            if(Values.Keys.Count == 0)
            {
                throw new ArgumentException("Values dictionary contains no columns/properties.");
            }

            var isValid = true;

            try
            {
                // Does WhenClauseApply throws InvalidOperationException if 
                // the matching property name is not found
                if (DoesWhenClauseApply(Values))
                {
                    return IsThenClauseValid(Values);
                }
            }
            catch(InvalidOperationException)
            {
                //... which means the condition cannot be true.
                return false;
            }

            return isValid;
        }

        public override List<string> GetPropertyNamesReferenced()
        {
            var l = new List<string>();
            foreach (var c in this.WhenComparisons)
            {
                l.Add(c.PropertyName);
            }
            foreach (var c in this.ThenComparisons)
            {
                l.Add(c.PropertyName);
            }
            return l;
        }

        private bool DoesWhenClauseApply(Dictionary<string, string> Values)
        {
            foreach (var c in this.whenComparisons)
            {
                if(!Values.ContainsKey(c.PropertyName))
                {
                    throw new InvalidOperationException("WHEN clause calls property " + c.PropertyName + ", which does not exist in the Values dictionary.");
                }

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

        private bool IsThenClauseValid(Dictionary<string, string> Values)
        {
            foreach (var c in this.thenComparisons)
            {
                if (!c.Compare(Values[c.PropertyName]))
                {
                    if(this.errorMessage == null)
                    {
                        this.errorMessage = "The following properties have invalid values:";
                    }
                    this.errorMessage += " Property: " + c.PropertyName + ", Value: {" + Values[c.PropertyName] +
                        "}, Valid Values are: {" + c.ValidValues.AsString() + "}";
                    
                    return false;
                }
            }

            return true;
        }
    }
}
