using System;
using System.Security.Principal;
using GDB.App.Domain;
using GDB.App.Domain.Enums;
using GDB.App.Domain.Exceptions;

namespace GDB.App.Domain.Models
{
    public abstract class Account : IAccount
    {
        // TODO: Declare protected fields:
        // accountNumber (string), name (string), age (int), balance (decimal), accountType (string), status (string), pin (string)
        protected string _accountNumber;
        protected string _name;
        protected int _age;
        protected decimal _balance;
        protected AccountType _accountType;
        protected AccountStatus _status;
        protected string _pin;
        protected AccountPrivilege _privilege;
        private const decimal WITHDRAWLIMIT=1000000m;
        private const decimal DEPOSITLIMIT =1000000m;

        public Account(string accountNumber, string name, int age, decimal balance, AccountType accountType, AccountStatus status, string pin, AccountPrivilege privilege)
        {
            // TODO: Initialize instance variables with parameters and validate input rules
            this._accountNumber = accountNumber;
            this._name = name;
            this._age = age;
            this._balance = balance;
            this._accountType = accountType;
            this._status = status;
            this._pin = pin;
            this._privilege = privilege;
        }

        public bool ValidatePin(string enteredPin)
        {
            // TODO: Validate PIN logic
            if (_pin == enteredPin)
            {
                return true;
            }
            return false;
        }

        public bool ChangePin(string oldPin, string newPin)
        {
            // TODO: Change PIN logic
            if (_pin != oldPin)
            {
                return false;
            }
            else if (newPin.Length != 4)
            {
                return false;
            }
            _pin = newPin;
            return true;
        }

        public void Deposit(decimal amount)
        {
            // TODO: Validate positive amount and add to balance

            if (!CheckIfAccountIsActive()) throw new InactiveAccountException("Account is not active");

            if (!CheckIfAmountIsValid(amount,DEPOSITLIMIT)) throw new InvalidAmountException("Deposit must be valid");

            _balance += amount;
        }

        // 1. Validate entered PIN (throw InvalidPinException if incorrect).
        // 2. Validate account status == "ACTIVE" (throw InactiveAccountException if not).
        // 3. Validate positive amount (throw InvalidAmountException if amount <= 0).
        // 4. Delegate to abstract ProcessDebit(amount).

        //Template method design pattern-create a template to have a common piece of
        //logic and enforce the subclasses to follow the template
        public void Withdraw(decimal amount, string enteredPin)
        {
            if (!CheckIfAccountIsActive()) throw new InactiveAccountException("Account is not active");
            
            if (!ValidatePin(enteredPin)) throw new InvalidPinException("Invalid PIN");
            
            if (!CheckIfAmountIsValid(amount, WITHDRAWLIMIT)) throw new InvalidAmountException("Withdrawal must be valid");
            
            ProcessDebit(amount);
            
        }

        public bool CheckIfAccountIsActive()
        {
            if(Enums.AccountStatus.Active != _status)
            {
                return false;
            }
            return true;
        }

        public bool CheckIfAmountIsValid(decimal amount, decimal limit)
        {
            if(amount <= 0.0m || amount>limit)
            {
                return false;
            }
            
            return true;
        }


        // TODO: Declare abstract primitive method to be implemented by each subclass:
        public abstract void ProcessDebit(decimal amount);


        public string AccountNumber => _accountNumber;
        public string Name => _name;
        public int Age => _age;
        public decimal Balance => _balance;
        public AccountType AccountType => _accountType;
        public AccountStatus Status => _status;

        public AccountPrivilege Privilege => _privilege;
    }
}
