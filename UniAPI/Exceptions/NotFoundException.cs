using System;

namespace UniAPI.Exceptions
{
    public class NotFoundException: Exception
    {
        public NotFoundException (string message) : base(message)
        {

        }
    }
}
