using System;

namespace ZTn.Json.Editor.Forms
{
    public class WrongJsonStreamException : Exception
    {
        public WrongJsonStreamException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}