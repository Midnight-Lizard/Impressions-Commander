using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MidnightLizard.Impressions.Commander.Infrastructure.Authentication
{
    public class UserId
    {
        public string Value { get; set; }

        public UserId(string id)
        {
            Value = id;
        }
    }
}
