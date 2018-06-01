using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zencodeguy.ExcelImporter.Mappers
{
    public abstract class MapperBase
    {
        protected ImportDefinition importDefinition;
        protected ImportResult importResult;
        protected Dictionary<string, string> substitutions;

        public ImportDefinition ImportDefinition
        {
            get
            {
                return this.importDefinition;
            }
        }

        public ImportResult ImportResult
        {
            get
            {
                return this.importResult;
            }
        }

        public Dictionary<string,string> Substitutions
        {
            get
            {
                return this.substitutions;
            }
        }

        protected void CanMap()
        {
            if (this.importDefinition.ContainsSubstitutions() &&
                this.substitutions == null)
            {
                throw new ArgumentNullException("Substitutions",
                    "Substitutions dictionary must be provided when Destination Properties require them.");
            }
            else if (!this.importDefinition.ContainsSubstitutions() &&
                this.substitutions == null)
            {
                throw new ArgumentException("Substitution values were provided, but ImportDefinition does not " +
                    "contain any Destination properties with substitutions defined.");
            }

            if (this.importDefinition.ContainsSubstitutions() &&
                    this.substitutions != null)
            {
                var missingSubstitutions = MapperHelpers.RequiredSubstitutionsProvided(this.importDefinition, this.substitutions);
                if (!string.IsNullOrWhiteSpace(missingSubstitutions))
                {
                    throw new ArgumentException("The substitutions dictionary is missing entries for the following keys: " + missingSubstitutions);
                }
            }
        }

    }
}
