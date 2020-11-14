using System;

namespace Shimakaze.Struct.Ini.Exceptions
{
    [Serializable]
    public class DataNotExistsException : Exception
    {
        public DataNotExistsException() { }
        public DataNotExistsException(string message) : base(message) { }
        public DataNotExistsException(string message, Exception inner) : base(message, inner) { }
        protected DataNotExistsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
