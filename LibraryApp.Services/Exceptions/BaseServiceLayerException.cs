using System;


namespace LibraryApp.Services.Exceptions
{
    public class BaseServiceLayerException : Exception
    {
        public BaseServiceLayerException(string message) : base(message) { }
    }
}
