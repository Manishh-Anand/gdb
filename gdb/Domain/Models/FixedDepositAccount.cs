
using System;
using GDB.App.Domain;
using GDB.App.Domain.Enums;
using GDB.App.Domain.Exceptions;


namespace GDB.App.Domain.Models
{
    public class FixedDepositAccount : Account
    {
        private int _tenureMonths;
        private double _interestRate;

        public FixedDepositAccount(string accountNumber, string name, int age, decimal balance,AccountType accountType, AccountStatus status, string pin,AccountPrivilege privilege, int tenureMonths = 12, double interestRate = 6.5)
            : base(accountNumber, name, age, balance, accountType, status, pin, privilege)
        {
            this._tenureMonths = tenureMonths;
            this._interestRate = interestRate;
        }

        public override void ProcessDebit(decimal amount)
        {
            throw new AccountException("Premature withdrawal not permitted on Fixed Deposit Account");
        }

        public decimal CalculateMaturityAmount() => _balance * (decimal)Math.Pow(1 + (_interestRate / 100.0) / 12, 12 * (_tenureMonths / 12.0));
        public int TenureMonths => _tenureMonths;
        public double InterestRate => _interestRate;
    }
}
