using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Exceptions
{
    public class CustomException : Exception
    {
        public CustomException(string message) : base(message)
        {
        }

        public override string ToString()
        {
            return Message;
        }
    }

}
