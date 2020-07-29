using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CrudDemo.Exceptions
{
    public class DuplicateRecordException : ApplicationException
    {
        public DuplicateRecordException()
        {
        }

        public DuplicateRecordException(string message) : base(message)
        {
        }

        public DuplicateRecordException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public DuplicateRecordException(Exception innerException) : base(innerException)
        {
        }

        protected DuplicateRecordException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
