using System;
using zencodeguy.ExcelImporter.Rules;

namespace zencodeguy.ExcelImporter.Parsers
{
    public static class ColumnDefinitionParser 
    {
        public static void Parse(string Line, ImportDefinition ID)
        {
            Helpers.ParametersValid(Line, ID, "COLUMN");

            var tokens = Line.Split(' ');
            if (tokens.Length < 4)
            {
                throw new ArgumentException("Column definition line must contain at least 4 tokens.");
            }

            var cd = new ColumnDefinition();

            if (Int32.TryParse(tokens[1], out int colNum))
            {
                cd.ColumnNumber = colNum;
            }
            else
            {
                throw new InvalidCastException("Cannot convert the token " + tokens[1] + " to an integer value.");
            }

            if (Helpers.IsColumnNumberAlreadyDefined(colNum, ID))
            {
                throw new InvalidOperationException("A column with the number " + colNum.ToString() + " is defined more than once.");
            }

            cd.PropertyName = tokens[2];
            if (Helpers.IsPropertyNameAlreadyDefined(cd.PropertyName, ID))
            {
                throw new InvalidOperationException("A column with the name '" + cd.PropertyName + "' is defined more than once.");
            }

            if (Enum.IsDefined(typeof(Enums.DataTypes), tokens[3]))
            {
                try
                {
                    cd.DataType = (Enums.DataTypes)Enum.Parse(typeof(Enums.DataTypes), tokens[3]);
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException("Invalid data type value: " + tokens[3], ex);
                }
            }
            else
            {
                throw new InvalidCastException("Cannot convert the token " + tokens[3] + " to a valid DataType.");
            }

            ParseColumnValidationRules(tokens, cd, ID);

            ID.Columns.Add(cd);
        }

        public static void ParseColumnValidationRules(string[] tokens, ColumnDefinition cd, ImportDefinition id)
        {
            int currentToken = 4;
            while (currentToken < tokens.Length)
            {
                if (tokens[currentToken] == "REQUIRED")
                {
                    id.Rules.Add(new RequiredRule(cd.PropertyName + "-REQUIRED").Property(cd.PropertyName));
                    currentToken++;
                }
                else if (tokens[currentToken] == "MINLENGTH")
                {
                    if (cd.DataType == Enums.DataTypes.STRING)
                    {
                        int min = Helpers.ParseNextTokenAsInteger(tokens, currentToken);
                        id.Rules.Add(new StringMinimumLengthRule(cd.PropertyName + "-MINLENGTH")
                            .Property(cd.PropertyName)
                            .MinimumLength(min));

                        currentToken = currentToken + 2;
                    }
                    else
                    {
                        throw new ArgumentException("MINLENGTH validation token can only be specified for data type STRING.");
                    }
                }
                else if (tokens[currentToken] == "MAXLENGTH")
                {
                    if (cd.DataType == Enums.DataTypes.STRING)
                    {
                        int max = Helpers.ParseNextTokenAsInteger(tokens, currentToken);
                        id.Rules.Add(new StringMaximumLengthRule(cd.PropertyName + "-MAXLENGTH")
                            .Property(cd.PropertyName)
                            .MaximumLength(max));

                        currentToken = currentToken + 2;
                    }
                    else
                    {
                        throw new ArgumentException("MAXLENGTH validation token can only be specified for data type STRING.");
                    }
                }
                else if (tokens[currentToken] == "PRECISION")
                {
                    if (cd.DataType == Enums.DataTypes.DECIMAL)
                    {
                        int precision = Helpers.ParseNextTokenAsInteger(tokens, currentToken);
                        currentToken++;
                        int scale = Helpers.ParseNextTokenAsInteger(tokens, currentToken);
                        currentToken = currentToken + 2;

                        id.Rules.Add(new DecimalPrecisionRule(cd.PropertyName + "-PRECISION")
                            .Property(cd.PropertyName)
                            .Precision(precision)
                            .Scale(scale));
                    }
                    else
                    {
                        throw new ArgumentException("PRECISION validation token can only be specified for data type DECIMAL.");
                    }
                }
                else
                {
                    throw new ArgumentException("Invalid token: " + tokens[currentToken]);
                }
            }
        }

    }
}
