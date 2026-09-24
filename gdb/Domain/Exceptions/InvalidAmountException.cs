using System;
using GDB.App.Domain.Exceptions;


namespace GDB.App.Domain.Exceptions
{
    /// <summary>
    /// Purpose: Thrown when deposit or withdrawal amount <= 0.
    /// </summary>
    public class InvalidAmountException : AccountException
    {
        public InvalidAmountException(string message = "") : base(message) { }
    }
}
