using System;
using System.Collections.Generic;

using zencodeguy.ExcelImporter;

namespace zencodeguy.ExcelImporter.Rules
{
    public sealed class ValueComparison
    {
        public ValueComparison(string PropertyName, 
            Enums.ComparisonOperators ComparisonOperator,
            List<string> ValidValues)
        {
            if (string.IsNullOrWhiteSpace(PropertyName))
            {
                throw new ArgumentNullException("PropertyName");
            }
            this.propertyName = PropertyName;

            this.comparisonOperator = ComparisonOperator;

            if (ValidValues == null)
            {
                throw new ArgumentNullException("ValidValues");
            }
            if(ValidValues.Count == 0)
            {
                throw new ArgumentException("ValidValues List cannot be empty.");
            }
            this.validValues = ValidValues;
        }

        private string propertyName;
        public string PropertyName
        {
            get
            {
                return this.propertyName;
            }
        }

        private Enums.ComparisonOperators comparisonOperator;
        public Enums.ComparisonOperators ComparisonOperator
        {
            get
            {
                return this.comparisonOperator;
            }
        }

        private List<string> validValues;
        public List<string> ValidValues
        {
            get
            {
                return this.validValues;
            }
        }

        public bool Compare(string Value)
        {
            if(string.IsNullOrWhiteSpace(Value))
            {
                return true;
            }

            if(this.comparisonOperator == Enums.ComparisonOperators.EQ ||
                this.comparisonOperator == Enums.ComparisonOperators.IN)
            {
                return this.ValidValues.Contains(Value);
            }

            if(this.comparisonOperator == Enums.ComparisonOperators.NOTEQ ||
                this.comparisonOperator == Enums.ComparisonOperators.NOTIN)
            {
                return !this.ValidValues.Contains(Value);
            }

            throw new InvalidOperationException("Comparison must be EQ, IN, NOTEQ, or NOTIN.");
        }
    }
}
