using System;


namespace LibraryApp.Services.Exceptions
{
    public class NotValidActionException : Exception
    {
        public NotValidActionException(string message) : base (message)  { }
    }
}
