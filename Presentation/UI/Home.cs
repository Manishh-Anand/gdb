using GDB.App.Application.Controllers;
using GDB.App.Application.Dtos;
using GDB.App.Application.Services.Contracts;
using GDB.App.Application.Services.Implementations;
using GDB.App.Domain.Enums;
using GDB.App.Domain.Exceptions;
using GDB.App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GDB.App.Presentation.UI
{
    public class Home
    {
        int choice;

        private static string FormatRupee(decimal? amount) =>
            amount.HasValue
                ? amount.Value.ToString("C", new CultureInfo("en-IN")): "N/A";

        private static bool IsValidAccountNumberInput(string acc)
        {
            if (string.IsNullOrWhiteSpace(acc)) return false;
            acc = acc.Trim();

            // Example rule: exactly 10 digits. Change to suit your business rule.
            return Regex.IsMatch(acc, @"^\d{10}$");
        }

        private static bool IsValidPinInput(string pin)
        {
            if (string.IsNullOrWhiteSpace(pin)) return false;
            pin = pin.Trim();

            // Rule: exactly 4 digits
            return Regex.IsMatch(pin, @"^\d{4}$");
        }

        public async Task Start()
        {
             choice = -1;

            while (choice != 0)
            {
                Console.WriteLine();
                Console.WriteLine("Welcome to GDB");
                Console.WriteLine("1. Create Account\n" +
                    "2. View Account\n" +
                    "3. View All Accounts\n" +
                    "4. View Balance\n" +
                    "5. View Recent Transactions\n" +
                    "6. Withdraw\n" +
                    "7. Deposit\n" +
                    "8. Transfer Funds\n" +
                    "9. Close Account\n" +
                    "0. Exit");

                Console.WriteLine("Enter Your Choice.");
                choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        CreateAccount();
                        break;

                    case 2:
                        await ViewAccountAsync();
                        break;

                    case 3:
                        ViewAllAccounts();
                        break;

                    case 4:
                        await ViewBalanceAsync();
                        break;

                    case 5:
                        await ViewRecentTransactionsAsync();
                        break;

                    case 6:
                        await WithdrawAsync();
                        break;

                    case 7:
                        await DepositAsync();
                        break;

                    case 8:
                        await TransferFundsAsync();
                        break;

                    case 9:
                        await CloseAccountAsync();
                        break;

                    case 0:
                        Exit();
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        


        }
        public void CreateAccount()
        {
            AccountController controller = new AccountController();

            Console.WriteLine("===== CREATE ACCOUNT =====");

            Console.Write("Enter Account Number: ");
            var accountNumber = Console.ReadLine().Trim();

            // Check if the user has entered valid Account Number or not
            while (!IsValidAccountNumberInput(accountNumber))
            {
                Console.WriteLine("Invalid account number. Please enter a 10-digit numeric account number.");
                Console.Write("Enter Account Number: ");
                accountNumber = Console.ReadLine()!;
            }

            Console.Write("Enter Name: ");
            var name = Console.ReadLine().Trim();

            Console.Write("Enter Age: ");
            var age = Convert.ToInt32(Console.ReadLine().Trim());

            Console.Write("Enter Initial Balance: ");
            var balance = Convert.ToDecimal(Console.ReadLine().Trim());

            Console.Write("Enter PIN: ");
            var pin = Console.ReadLine()!.Trim();

            // Check if the user has entered valid PIN or not
            while (!IsValidPinInput(pin))
            {
                Console.WriteLine("Invalid PIN. Please enter a 4-digit numeric PIN.");
                Console.Write("Enter PIN: ");
                pin = Console.ReadLine()!.Trim();
            }

            

            Console.WriteLine("Select Account Type:");
            Console.WriteLine("1. Savings");
            Console.WriteLine("2. Current");
            Console.WriteLine("3. Fixed Deposit");
            Console.WriteLine("4. Salary");

            Console.Write("Enter Choice: ");
            int typeChoice = Convert.ToInt32(Console.ReadLine());

            AccountType accountType;

            switch (typeChoice)
            {
                case 1:
                    accountType = AccountType.Savings;
                    break;

                case 2:
                    accountType = AccountType.Current;
                    break;

                case 3:
                    accountType = AccountType.FixedDeposit;
                    break;

                case 4:
                    accountType = AccountType.Salary;
                    break;

                default:
                    throw new Exception("Invalid account type.");
            }

            Console.Write("Enter Privilege (Premium/Gold/Silver): ");
            string privilegeInput = Console.ReadLine()!;

            AccountPrivilege privilege =
                (AccountPrivilege)Enum.Parse(
                    typeof(AccountPrivilege),
                    privilegeInput,
                    true);

            // Default account-specific values
            decimal overdraftLimit = 25000m;
            int tenureMonths = 12;
            double interestRate = 6.5;
            decimal minimumBalance = 1000m;
            string employerName = "TechCorp";
            AccountStatus status = AccountStatus.Active;

            if (accountType == AccountType.Current)
            {
                Console.Write("Enter Overdraft Limit: ");
                overdraftLimit = Convert.ToDecimal(Console.ReadLine());
            }
            else if (accountType == AccountType.FixedDeposit)
            {
                Console.Write("Enter Tenure Months: ");
                tenureMonths = Convert.ToInt32(Console.ReadLine());

                Console.Write("Enter Interest Rate: ");
                interestRate = Convert.ToDouble(Console.ReadLine());
            }
            else if (accountType == AccountType.Savings)
            {
                Console.Write("Enter Minimum Balance: ");
                minimumBalance = Convert.ToDecimal(Console.ReadLine());

                Console.Write("Enter Interest Rate: ");
                interestRate = Convert.ToDouble(Console.ReadLine());
            }
            else if (accountType == AccountType.Salary)
            {
                Console.Write("Enter Employer Name: ");
                employerName = Console.ReadLine()!;
            }

            // ============================================================
            // CREATE REQUEST DTO
            // ============================================================

            CreateAccountRequestDto request = new CreateAccountRequestDto()
            {
                AccountNumber = accountNumber,
                Name = name,
                Age = age,
                Balance = balance,
                Pin = pin,
                AccountType = accountType,
                Status = status,
                Privilege = privilege,
                OverdraftLimit = overdraftLimit,
                TenureMonths = tenureMonths,
                InterestRate = interestRate,
                MinimumBalance = minimumBalance,
                EmployerName = employerName
            };

            

            try
            {
                // ============================================================
                // SEND REQUEST DTO TO CONTROLLER
                // AND RECEIVE RESPONSE DTO
                // ============================================================


                CreateAccountResponseDto response =
                controller.CreateAccount(request);

                // ============================================================
                // DISPLAY RESPONSE DTO
                // ============================================================

                Console.WriteLine();
                Console.WriteLine("===== ACCOUNT CREATED SUCCESSFULLY =====");

                Console.WriteLine($"Account Number : {response.AccountNumber}");
                Console.WriteLine($"Name           : {response.Name}");
                Console.WriteLine($"Account Type   : {response.AccountType}");
                Console.WriteLine($"Balance        : {FormatRupee(response.Balance)}");
                Console.WriteLine($"Status         : {response.Status}");
                Console.WriteLine($"Privilege      : {response.Privilege}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("\n" + ex.Message);
            }
            
        }


        public async Task ViewAccountAsync()
        {
            //Accept accNo to get the accountInfo
            Console.WriteLine("Enter the account number.");
            string accNo = Console.ReadLine();

            //Contact the database to get the accountInfo
            //UI->controller
            ViewAccountResponseDto account = await new AccountController().ViewAccountAsync(accNo);
            //Display the accountInfo
            
            if (account == null)
            {
                Console.WriteLine("Account not found.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Account Number : " + account.AccountNumber);
            Console.WriteLine("Name           : " + account.Name);
            Console.WriteLine($"Balance       : {FormatRupee(account.Balance)}");
        }
        public void ViewAllAccounts()
        {
            AccountController controller = new AccountController();

            var accounts = controller.GetAllAccounts();

            Console.WriteLine();
            Console.WriteLine("All Accounts");
            Console.WriteLine("----------------------------");

            foreach (var account in accounts)
            {
                Console.WriteLine("Account Type   : " + account.AccountType);
                Console.WriteLine("Account Number : " + account.AccountNumber);
                Console.WriteLine("Name           : " + account.Name);
                Console.WriteLine("Age            : " + account.Age);
                Console.WriteLine($"Balance       : {FormatRupee(account.Balance)}");
                Console.WriteLine("Status         : " + account.AccountStatus);
                Console.WriteLine("Privilege      : " + account.AccountPrivilege);
            }
        }
        public async Task ViewBalanceAsync()
        {
            Console.WriteLine("Enter Account Number:");
            string accountNumber = Console.ReadLine();

            AccountController controller = new AccountController();

            ViewBalanceResponseDto account = await controller.GetBalanceAsync(accountNumber);

            if (account == null)
            {
                Console.WriteLine("Account not found.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"Balance  : {FormatRupee(account.Balance)}");
            

        }
        public async Task ViewRecentTransactionsAsync()
        {
            Console.WriteLine();
            Console.WriteLine("===== VIEW RECENT TRANSACTIONS =====");

            Console.Write("Enter Account Number: ");
            string accountNumber = Console.ReadLine()!;

            try
            {
                TransactionController controller =
                    new TransactionController();

                List<ViewRecentTransactionsResponseDto> transactions =await
                    controller.GetRecentTransactionsAsync(accountNumber);

                if (transactions.Count == 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("No transactions found.");
                    return;
                }

                Console.WriteLine();
                Console.WriteLine("===== RECENT TRANSACTIONS =====");

                foreach (ViewRecentTransactionsResponseDto transaction
                         in transactions)
                {
                    Console.WriteLine();
                    Console.WriteLine("-------------------------------");

                    Console.WriteLine(
                        $"Transaction ID   : {transaction.TransactionId}");

                    Console.WriteLine(
                        $"From Account     : {transaction.FromAccountNumber ?? "N/A"}");

                    Console.WriteLine(
                        $"To Account       : {transaction.ToAccountNumber ?? "N/A"}");

                    Console.WriteLine(
                        $"Transaction Type : {transaction.TransactionType}");

                    Console.WriteLine($"Amount  : {FormatRupee(transaction.Amount)}");

                    Console.WriteLine(
                        $"Status           : {transaction.TransactionStatus}");

                    Console.WriteLine(
                        $"Timestamp        : {transaction.Timestamp}");

                    if (transaction.BalanceAfterFrom.HasValue)
                    {
                        Console.WriteLine(
                            $"Balance After From : {FormatRupee(transaction.BalanceAfterFrom)}");
                    }

                    if (transaction.BalanceAfterTo.HasValue)
                    {
                        Console.WriteLine(
                            $"Balance After To   : {FormatRupee(transaction.BalanceAfterTo)}");
                    }
                }

                Console.WriteLine();
                Console.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        public async Task WithdrawAsync()
        {
            Console.WriteLine("Enter Account Number:");
            string accountNumber = Console.ReadLine().Trim();

            Console.WriteLine("Enter PIN:");
            string pin = Console.ReadLine().Trim();

            Console.WriteLine("Enter Amount:");
            decimal amount = decimal.Parse(Console.ReadLine().Trim());

            try
            {
                TransactionController controller =
                    new TransactionController();

                var account = await controller.WithdrawAsync(
                    accountNumber,
                    pin,
                    amount
                );

                Console.WriteLine($"Balance  : {FormatRupee(account.Balance)}");
                Console.WriteLine("Status: " + account.TransactionStat);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public async Task DepositAsync()
        {
            Console.WriteLine("Enter Account Number:");
            string accountNumber = Console.ReadLine();

            Console.WriteLine("Enter Amount:");
            decimal amount = decimal.Parse(Console.ReadLine());

            try
            {
                TransactionController controller =
                    new TransactionController();

                var account = await controller.DepositAsync(accountNumber, amount);

                Console.WriteLine($"Balance   : {FormatRupee(account.Balance)}");
                Console.WriteLine("Status: " + account.TransactionStat);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task TransferFundsAsync()
        {
            Console.WriteLine("Enter From Account Number:");
            string fromAccountNumber = Console.ReadLine();

            Console.WriteLine("Enter To Account Number:");
            string toAccountNumber = Console.ReadLine();

            Console.WriteLine("Enter PIN:");
            string pin = Console.ReadLine();

            Console.WriteLine("Enter Amount:");
            decimal amount = decimal.Parse(Console.ReadLine());

            try
            {
                if (fromAccountNumber.Equals(toAccountNumber))
                {
                    throw new AccountException("\nSource Account and Destination Account cannot be the same");
                }
                TransactionController controller =
                    new TransactionController();

                var result = await
                    controller.TransferFundsAsync(
                        fromAccountNumber,
                        toAccountNumber,
                        pin,
                        amount
                    );

                Console.WriteLine();
                
                Console.WriteLine($"Transaction Status: {result.TransactionStat}");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n" + ex.Message);
            }
        }
        public async Task CloseAccountAsync()
        {
            Console.WriteLine("===== CLOSE ACCOUNT =====");

            Console.Write("Enter Account Number: ");
            string accountNumber = Console.ReadLine()!;

            try
            {
                CloseAccountRequestDto request =
                    new CloseAccountRequestDto()
                    {
                        AccountNumber = accountNumber
                    };

                AccountController controller =new AccountController();

                CloseAccountResponseDto response = await controller.CloseAccountAsync(request);

                Console.WriteLine();
                Console.WriteLine("===== ACCOUNT CLOSED =====");
                Console.WriteLine($"Account Number : {response.AccountNumber}");
                Console.WriteLine($"Status         : {response.Status}");
                Console.WriteLine($"Message        : {response.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }
        public void Exit()
        {
            Console.WriteLine("\nThank you for using GDB.");
            choice = 0;
        }

        

    }
}


