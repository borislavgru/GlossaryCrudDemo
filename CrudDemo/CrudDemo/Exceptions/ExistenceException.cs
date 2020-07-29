using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CrudDemo.Exceptions
{
    public class NonExistingItemException : ApplicationException
    {
        public NonExistingItemException()
        {
        }

        public NonExistingItemException(string message) : base(message)
        {
        }

        public NonExistingItemException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public NonExistingItemException(Exception innerException) : base(innerException)
        {
        }

        protected NonExistingItemException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
