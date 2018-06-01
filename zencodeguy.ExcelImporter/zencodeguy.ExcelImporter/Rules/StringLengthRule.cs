using System;
using System.Collections.Generic;

namespace zencodeguy.ExcelImporter.Rules
{
    public class StringLengthRule: Rule
    {
        protected int? length;

        public StringLengthRule(string RuleName): base(RuleName) { }

        public int? Length
        {
            get
            {
                return this.length;
            }
        }

        protected void SetProperty(string PropertyName)
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
        }

        protected void CheckCanTest()
        {
            if (string.IsNullOrWhiteSpace(this.propertyName))
            {
                throw new InvalidOperationException("Property Name is not set.");
            }
            if (!this.length.HasValue)
            {
                throw new InvalidOperationException("Minimum length value is not set.");
            }
        }

        public override List<string> GetPropertyNamesReferenced()
        {
            return new List<string>(1) { this.propertyName };
        }

    }
}
