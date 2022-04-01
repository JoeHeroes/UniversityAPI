using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniAPI.Exceptions
{
    public class ForbidExeption : Exception
    {
        public ForbidExeption(string message) : base(message)
        {
        }
    }
}
