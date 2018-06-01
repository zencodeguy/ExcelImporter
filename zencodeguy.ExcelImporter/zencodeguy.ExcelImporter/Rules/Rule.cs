using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zencodeguy.ExcelImporter.Rules
{
    public class Rule
    {
        public Rule(string RuleName)
        {
            if (string.IsNullOrWhiteSpace(RuleName))
            {
                throw new ArgumentNullException("RuleName");
            }
            this.ruleName = RuleName;
        }

        private string ruleName;
        public string RuleName
        {
            get
            {
                return this.ruleName;
            }
        }

        protected string errorMessage;
        public string ErrorMessage
        {
            get
            {
                return this.errorMessage;
            }
        }

        protected string propertyName;
        public string PropertyName
        {
            get
            {
                return this.propertyName;
            }
        }

        protected void CheckProvidedValues(Dictionary<string,string> Values)
        {
            if (Values == null)
            {
                throw new ArgumentNullException("Values");
            }

            if (Values.Keys.Count == 0)
            {
                throw new ArgumentException("Values dictionary contains no columns/properties.");
            }

        }

        protected virtual bool CanIsValidBeCalled()
        {
            throw new NotImplementedException();
        }
        public virtual bool IsValid(Dictionary<string, string> Values)
        {
            throw new NotImplementedException();
        }

        public virtual List<string> GetPropertyNamesReferenced()
        {
            throw new NotImplementedException();
        }
    }
}
