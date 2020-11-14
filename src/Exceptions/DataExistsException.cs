using System;
using System.Collections.Generic;
using System.Text;

namespace Shimakaze.Struct.Ini.Exceptions
{

    [Serializable]
    public class DataExistsException : Exception
    {
        public DataExistsException() { }
        public DataExistsException(string message) : base(message) { }
        public DataExistsException(string message, Exception inner) : base(message, inner) { }
        protected DataExistsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
