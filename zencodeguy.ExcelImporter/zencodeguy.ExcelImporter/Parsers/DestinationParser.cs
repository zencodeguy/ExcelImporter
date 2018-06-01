using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zencodeguy.ExcelImporter.Parsers
{
    public static class DestinationParser
    {
        public static void Parse(string Line, ImportDefinition ID)
        {
            Helpers.ParametersValid(Line, ID, "DESTINATION");

            var tokens = Line.Split(' ');
            if (tokens.Length < 4)
            {
                throw new ArgumentException("Destination definition line must contain at least 4 tokens.");
            }

#pragma warning disable IDE0017 // Simplify object initialization
            var dp = new DestinationPropertyDefinition();
#pragma warning restore IDE0017 // Simplify object initialization

            dp.PropertyName = tokens[1];
            if (Helpers.IsDestinationPropertyAlreadyDefined(tokens[1], ID))
            {
                throw new InvalidOperationException("A destination property named " + tokens[1] + " is defined more than once.");
            }

            if (Enum.IsDefined(typeof(Enums.DataTypes), tokens[2]))
            {
                try
                {
                    dp.DataType = (Enums.DataTypes)Enum.Parse(typeof(Enums.DataTypes), tokens[2]);
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException("Invalid data type value: " + tokens[2], ex);
                }
            }
            else
            {
                throw new InvalidCastException("Cannot convert the token " + tokens[2] + " to a valid DataType.");
            }

            if (tokens[3] == "SET")
            {
                ParseDestinationSetToken(Line, dp);
            }
            else if (tokens[3] == "GENERATE")
            {
                ParseDestinationGenerateToken(tokens, dp);
            }
            else if (tokens[3] == "SUBSTITUTE")
            {
                ParseDestinationSubstitution(tokens, dp);
            }
            else
            {
                throw new ArgumentException("Invalid token for Destination declaration: " + tokens[3]);
            }

            ID.DestinationProperties.Add(dp);
        }

        private static void ParseDestinationGenerateToken(string[] tokens, DestinationPropertyDefinition dp)
        {
            if (dp.DataType == Enums.DataTypes.GUID)
            {
                dp.Generate = true;
            }
            else if (dp.DataType == Enums.DataTypes.DATETIME)
            {
                dp.Generate = true;
                if(tokens.Length > 4 && tokens[4] == "DATEONLY")
                {
                    dp.GenerateDateOnly = true;
                }
            }
            else
            {
                throw new ArgumentException("Cannot set GENERATE on a data type other than GUID and DATETIME.");
            }
        }

        private static void ParseDestinationSetToken(string line, DestinationPropertyDefinition dp)
        {
            var index = line.IndexOf(" SET ") + 5;
            if (index == -1 || index >= line.Length)
            {
                throw new InvalidOperationException("Destination Property " + dp.PropertyName + " contains a SET without a value.");
            }

            dp.SetValue = line.Substring(index).ExtractDelimitedSection('{', '}', 0).Item1;
            if (!Helpers.CanSetValueBeParsedAsType(dp.SetValue, dp.DataType))
            {
                throw new InvalidCastException("Destination Property " + dp.PropertyName +
                    " attempted to declare a Set Value of " + dp.SetValue +
                    " which cannot be cast to the declared data type of " +
                    dp.DataType.ToString());
            }
        }

        private static void ParseDestinationSubstitution(string[] tokens, DestinationPropertyDefinition dp)
        {
            if(tokens.Length < 5)
            {
                throw new IndexOutOfRangeException("SUBSTITUTE called with no parameter name specified.");
            }

            var placeholderName = tokens[4].ExtractDelimitedSection('{', '}', 0).Item1;

            dp.Substitute = true;
            dp.SubstitutionName = placeholderName;
        }
    }
}
