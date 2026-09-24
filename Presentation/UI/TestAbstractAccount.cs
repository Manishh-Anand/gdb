using GDB.App.Application.Services.Implementations;
using GDB.App.Domain;
using GDB.App.Domain.Enums;
using GDB.App.Domain.Exceptions;
using GDB.App.Domain.Models;
using GDB.App.Infrastructure.Repositories;
using System;
using System.Text;


namespace GDB.App.Presentation.UI
{
    public class TestAbstractAccount
    {
        // TODO: Implement TransferFunds(AbstractAccount from, AbstractAccount to, decimal amount, string pin)
        //public static void TransferFunds(Account fromAcc, Account toAcc, decimal amount, string pin)
        //{
        //    // TODO: Execute transfer workflow
        //    try
        //    {
        //        fromAcc.Withdraw(amount, pin);

        //        toAcc.Deposit(amount);
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Console.WriteLine($"Fund Transfer Failed: {ex.Message}");
        //    }
        //}

        public static async Task Main(string[] args)
        {
            DataBaseProviderRegistration.Register();
            //    Console.WriteLine("=== Activity 10: Abstract Classes & Template Method Tests ===");

            //    //AbstractAccount savings = new SavingsAccount("S001","Vishnu",25,10000.0m,"ACTIVE","1234" );

            //    //AbstractAccount current = new CurrentAccount( "C001", "Vishnu", 25,50000.0m, "ACTIVE", "5678" );

            //    //AbstractAccount fixedDeposit = new FixedDepositAccount("F001","Vishnu", 25, 20000.0m, "ACTIVE", "9999" );

            //    try
            //    {
            //        var savings = AccountFactory.CreateAccount(AccountType.Savings, "S001", "Vishnu", 25, 10000.0m, AccountStatus.Active, "1234", AccountPrivilege.Premium);


            //        var current = AccountFactory.CreateAccount(AccountType.Current, "C001", "Vishnu", 25, 50000.0m, AccountStatus.Active, "5678", AccountPrivilege.Premium);

            //        var fixedDeposit = AccountFactory.CreateAccount(AccountType.FixedDeposit, "F001", "Vishnu", 25, 20000.0m, AccountStatus.Active, "9999", AccountPrivilege.Premium);

            //        Account[] portfolio = { savings, current, fixedDeposit };

            //        // TODO: Step 1 - Test Template Method Withdrawal on Savings Account

            //        savings.Withdraw(1000.0m, "1234");
            //        Console.WriteLine("Savings Account Withdraw Successful");


            //        // TODO: Step 2 - Test Template Method Withdrawal on Current Account Overdraft

            //        current.Withdraw(2000.0m, "5678");
            //        Console.WriteLine("Current Account Withdraw Successful");

            //        // TODO: Step 3 - Test Template Method Withdrawal on Fixed Deposit Block

            //        fixedDeposit.Withdraw(3000.0m, "9999");
            //        Console.WriteLine("Fixed Deposit Account Withdraw Successful");


            //        // TODO: Step 4 - Test Inter-Account Fund Transfer

            //        TransferFunds(current, savings, 10000.0m, "5678");
            //        Console.WriteLine("Fund Transfer Successful");
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e.Message);
            //    }

            //    Console.WriteLine("All abstract account unit tests executed!");



            Console.OutputEncoding = Encoding.UTF8;
            await new Home().Start();
        }
    }
}