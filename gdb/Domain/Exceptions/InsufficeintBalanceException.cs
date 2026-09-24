using System;
using GDB.App.Domain.Exceptions;

namespace GDB.App.Domain.Exceptions
{
    /// <summary>
    /// Purpose: Thrown when withdrawal amount exceeds available funds.
    /// </summary>
    public class InsufficientBalanceException : AccountException
    {
        public InsufficientBalanceException(string message = "") : base(message) { }
    }
}
