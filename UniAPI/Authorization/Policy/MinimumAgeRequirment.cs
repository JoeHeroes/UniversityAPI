using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniAPI.Authorization.Policy
{
    public class MinimumAgeRequirment:IAuthorizationRequirement
    {
        public int MinAge { get;}

        public MinimumAgeRequirment(int minAge)
        {
            MinAge = minAge;
        }
    }
}
