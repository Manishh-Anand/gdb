using System;
using GDB.App.Domain.Exceptions;


namespace GDB.App.Domain.Exceptions
{
    /// <summary>
    /// Purpose: Thrown when withdrawal breaches minimum balance requirement.
    /// </summary>
    public class MinimumBalanceViolationException : AccountException
    {
        public MinimumBalanceViolationException(string message = "") : base(message) { }
    }
}
