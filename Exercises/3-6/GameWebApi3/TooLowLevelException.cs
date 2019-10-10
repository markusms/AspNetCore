using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GameWebApi3
{
    public class TooLowLevelException : Exception
    {
        private string exceptionMessage = "Level is too low!";
        public string ExceptionMessage { get { return exceptionMessage; } set { exceptionMessage = value; } }
        public TooLowLevelException() : base() { }

        public TooLowLevelException(string exceptionMessage) : base(exceptionMessage)
        {
            this.exceptionMessage = exceptionMessage;
        }

        public TooLowLevelException(string exceptionMessage, string message) : base(message)
        {
            this.exceptionMessage = exceptionMessage;
        }

        public TooLowLevelException(string exceptionMessage, string message, Exception innerException) : base(message, innerException)
        {
            this.exceptionMessage = exceptionMessage;
        }

    }
}
