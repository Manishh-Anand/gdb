using System;

namespace GDB.App.Domain.Exceptions
{
    /// <summary>
    /// Purpose: Base domain exception for all banking and account-related operations in Global Digital Bank.
    /// Where to use: Base class for all custom domain exceptions; used to catch general account errors.
    /// </summary>
    public class AccountException : Exception
    {
        public AccountException() : base() { }
        public AccountException(string message) : base(message) { }
    }
}
