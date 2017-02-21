using System;

namespace ZTn.Json.JsonTreeView
{
    public class WrongJsonStreamException : Exception
    {
        public WrongJsonStreamException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}