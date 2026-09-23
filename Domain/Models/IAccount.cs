using System;
using GDB.App.Domain;
using GDB.App.Domain.Enums;
using GDB.App.Domain.Exceptions;
namespace GDB.App.Domain.Models

{
    public interface IAccount
    {
        string AccountNumber { get; }
        string Name { get; }
        int Age { get; }
        decimal Balance { get; }
        AccountType AccountType { get; }
        AccountStatus Status { get; }

        AccountPrivilege Privilege { get; }


        bool CheckIfAccountIsActive(); 
        void Deposit(decimal amount);
        void Withdraw(decimal amount, string enteredPin);
        bool ValidatePin(string enteredPin);

        bool ChangePin(string oldPIn, string newPin);

    }
}