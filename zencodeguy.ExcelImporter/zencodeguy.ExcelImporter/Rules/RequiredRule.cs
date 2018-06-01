using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zencodeguy.ExcelImporter.Rules
{
    public class RequiredRule : Rule
    {
        public RequiredRule(string RuleName) : base(RuleName) { }

        public RequiredRule Property(string PropertyName)
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

        public override bool IsValid(Dictionary<string, string> Values)
        {
            if(string.IsNullOrWhiteSpace(this.propertyName))
            {
                throw new InvalidOperationException("PropertyName is not set.");
            }

            //TODO: If Key missing or NULL?
            var v = Values[this.PropertyName];

            if(string.IsNullOrWhiteSpace(v))
            {
                this.errorMessage = this.propertyName + " is required, but is null, an empty string, or whitespace.";
                return false;
            }

            return true;
        }

        public override List<string> GetPropertyNamesReferenced()
        {
            if (!string.IsNullOrWhiteSpace(this.propertyName))
            {
                return new List<string>() { this.propertyName };
            }
            else
            {
                throw new InvalidOperationException("PropertyName has not been set.");
            }
        }

    }
}
