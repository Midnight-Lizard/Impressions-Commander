using SemVer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MidnightLizard.Impressions.Commander.Infrastructure.Serialization
{
    public class SchemaVersionAttribute : Attribute
    {
        public Range VersionRange { get; set; }

        /// <summary>
        /// Schema version range attribute
        /// </summary>
        /// <param name="rangeSpec">SemVer version range spec</param>
        public SchemaVersionAttribute(string rangeSpec)
        {
            VersionRange = new SemVer.Range(rangeSpec);
        }
    }
}
