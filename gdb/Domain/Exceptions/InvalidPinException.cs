using System;
using GDB.App.Domain.Exceptions;


namespace GDB.App.Domain.Exceptions
{
    /// <summary>
    /// Purpose: Thrown when entered PIN does not match account PIN.
    /// </summary>
    public class InvalidPinException : AccountException
    {
        public InvalidPinException(string message = "") : base(message) { }
    }
}
