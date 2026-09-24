using GDB.App.Domain.Enums;
using GDB.App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
namespace GDB.App.Domain;
public class AccountFactory
{
    public static Account CreateAccount(AccountType accountType,
                                        string accountNumber,
                                        string name,
                                        int age,
                                        decimal balance,
                                        AccountStatus status,
                                        string pin,
                                        AccountPrivilege privilege,
                                        decimal overdraftLimit = 25000.0m,
                                        int tenureMonths = 12,
                                        double interestRate = 6.5,
                                        decimal minBalance = 1000.0m,
                                        string employerName = "TechCorp")
    { 
        Account userAccount = accountType switch
        {
            AccountType.Savings =>
            new SavingsAccount(
                accountNumber,
                name,
                age,
                balance,
                accountType,
                status,
                pin,
                privilege,
                minBalance,
                interestRate),

            AccountType.Current =>
                new CurrentAccount(
                    accountNumber,
                    name,
                    age,
                    balance,
                    accountType,
                    status,
                    pin,
                    privilege,
                    overdraftLimit),

            AccountType.Salary =>
                new SalaryAccount(
                    accountNumber,
                    name,
                    age,
                    balance,
                    accountType,
                    status,
                    pin,
                    privilege,
                    employerName),

            AccountType.FixedDeposit =>
                new FixedDepositAccount(
                    accountNumber,
                    name,
                    age,
                    balance,
                    accountType,
                    status,
                    pin,
                    privilege,
                    tenureMonths,
                    interestRate),

            _ => throw new Exception("Invalid Account Type")
        };
        return userAccount;
    }
}