using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zencodeguy.ExcelImporter.Mappers
{
    public static class MapperHelpers
    {
        public static string RequiredSubstitutionsProvided(ImportDefinition ImportDefinition, 
            Dictionary<string, string> Substitutions)
        {
            StringBuilder sb = new StringBuilder();

            foreach(var d in ImportDefinition.DestinationProperties.Where(x => x.Substitute))
            {
                if(!Substitutions.ContainsKey(d.SubstitutionName))
                {
                    if(sb.Length > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(d.SubstitutionName);
                }
            }

            if(sb.Length == 0)
            {
                return null;
            }
            else
            {
                return sb.ToString();
            }
        }
    }
}
