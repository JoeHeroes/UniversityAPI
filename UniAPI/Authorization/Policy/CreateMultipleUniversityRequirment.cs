using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniAPI.Authorization.Policy
{
    public class CreateMultipleUniversityRequirment : IAuthorizationRequirement
    {
        public CreateMultipleUniversityRequirment(int _MinimumCreatedUni)
        {
            MinimumCreatedUni = _MinimumCreatedUni;
        }
        public int MinimumCreatedUni {get;}
    }
}
