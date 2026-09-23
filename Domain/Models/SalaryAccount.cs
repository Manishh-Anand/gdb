using GDB.App.Domain;
using GDB.App.Domain.Enums;
using GDB.App.Domain.Exceptions;
using System;


namespace GDB.App.Domain.Models
{
    public class SalaryAccount : Account
    {
        private string _employerName;
        private int _inactiveMonths;

        public SalaryAccount(string accountNumber, string name, int age, decimal balance,AccountType accountType,AccountStatus status, string pin,AccountPrivilege privilege, string employerName = "TechCorp")
            : base(accountNumber, name, age, balance, accountType, status, pin, privilege)
        {
            this._employerName = employerName;
            this._inactiveMonths = 0;
        }

        public override void ProcessDebit(decimal amount)
        {
            if (!CheckInsufficientFunds(amount))
                throw new InsufficientBalanceException("Insufficient funds in Salary account");
            _balance -= amount;
        }
        public bool CheckInsufficientFunds(decimal amount)
        {
            if (amount > _balance)
            {
                return false;
            }
            return true;
        }
        public void IncrementInactiveMonths()
        {
            _inactiveMonths++;
            if (!CheckInactiveMonthsDuration()) _status = Enums.AccountStatus.Frozen;
        }
        public bool CheckInactiveMonthsDuration()
        {
            if (_inactiveMonths >= 3)
            {
                return false;
            }
            return true;
        }

        public string EmployerName => _employerName;
        public int InactiveMonths => _inactiveMonths;
    }
}
