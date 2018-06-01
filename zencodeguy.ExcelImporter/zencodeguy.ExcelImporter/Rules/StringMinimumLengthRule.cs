using System;
using System.Collections.Generic;

namespace zencodeguy.ExcelImporter.Rules
{
    public class StringMinimumLengthRule : StringLengthRule
    {
        public StringMinimumLengthRule(string RuleName) : base(RuleName) { }

        public StringMinimumLengthRule Property(string PropertyName)
        {
            this.SetProperty(PropertyName);
            return this;
        }

        public StringMinimumLengthRule MinimumLength(int length)
        {
            this.length = length;
            return this;
        }

        public override bool IsValid(Dictionary<string, string> Values)
        {
            this.CheckCanTest();

            //TODO: If key is not present
            var v = Values[this.propertyName];

            if (string.IsNullOrWhiteSpace(v))
            {
                return true;
            }

            if (v.Length >= this.length)
            {
                return true;
            }
            else
            {
                this.errorMessage = this.propertyName + " value is " + v.Length.ToString() + " characters, which does not meet the minimum required length of " + this.length.Value.ToString() + " characters.";
                return false;
            }
        }

    }
}
