using GDB.App.Application.Dtos;
using GDB.App.Application.Services.Contracts;
using GDB.App.Domain;
using GDB.App.Domain.Enums;
using GDB.App.Domain.Models;
using GDB.App.Infrastructure.Repositories;
using GDB.App.Infrastructure.Repositories.Contracts;
using GDB.App.Infrastructure.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using gdb.Logging;
using Microsoft.Extensions.Logging;

namespace GDB.App.Application.Services.Implementations
{
    /// <summary>
    /// Provides business logic and service operations for account management within the Global Digital Bank application.
    /// </summary>
    /// <remarks>
    /// This namespace contains the implementation of service classes that handle account-related operations
    /// such as account retrieval, account creation, balance inquiries, and account closure.
    /// These services act as intermediaries between the presentation layer and the data access layer.
    /// </remarks>
    /// <summary>
    /// Implements account service operations for managing bank accounts.
    /// </summary>
    /// <remarks>
    /// The <see cref="AccountService"/> class provides core business logic for account operations including:
    /// <list type="bullet">
    /// <item><description>Retrieving account information asynchronously</description></item>
    /// <item><description>Creating new accounts with various types (Savings, Current, Fixed Deposit, Salary)</description></item>
    /// <item><description>Viewing account details and balance inquiries</description></item>
    /// <item><description>Closing existing accounts with appropriate validation</description></item>
    /// <item><description>Retrieving all accounts in the system with detailed information</description></item>
    /// </list>
    /// This service uses dependency injection for the account repository and follows the repository pattern
    /// for data access operations. It also implements structured logging for audit trails and debugging.
    /// </remarks>
    internal class AccountService : IAccountService
    {
        /// <summary>
        /// Provides data access operations for account entities.
        /// </summary>
        private readonly IAccountRepository _accountRepository;

        /// <summary>
        /// Logger instance used for logging account service operations.
        /// </summary>
        private static readonly ILogger _logger = AppLogger.CreateLogger<AccountService>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountService"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor initializes the account repository using the factory pattern.
        /// The repository is configured to use the database ("DB") as the data source,
        /// allowing for abstraction of the underlying data storage mechanism.
        /// </remarks>
        public AccountService()
        {
            // Repository is created via factory pattern for loose coupling
            _accountRepository = AccountRepositoryFactory.Create("DB");
        }

        /// <summary>
        /// Retrieves an account asynchronously by its account number.
        /// </summary>
        /// <param name="accNo">The account number to retrieve.</param>
        /// <returns>
        /// A task representing the asynchronous operation that returns the <see cref="IAccount"/> instance if found.
        /// </returns>
        /// <remarks>
        /// This method provides a direct pass-through to the repository layer for retrieving account data.
        /// It uses asynchronous operations to prevent blocking I/O operations.
        /// </remarks>
        /// <example>
        /// <code>
        /// var account = await accountService.GetAccountAsync("ACC123456");
        /// </code>
        /// </example>
        public async Task<IAccount> GetAccountAsync(string accNo)
        {
            // Delegate to repository for data retrieval
            var account = await _accountRepository.GetAccountAsync(accNo);
            return account;
        }

        /// <summary>
        /// Retrieves all accounts in the system with their detailed information.
        /// </summary>
        /// <returns>
        /// A <see cref="List{T}"/> of <see cref="ViewAllAccountsResponseDto"/> containing information
        /// for all accounts including name, account number, balance, privilege level, account type, and age.
        /// </returns>
        /// <remarks>
        /// This method retrieves all accounts from the repository and transforms them into Data Transfer Objects (DTOs)
        /// for presentation to the client. Each account is mapped to include the following details:
        /// <list type="bullet">
        /// <item><description>Account Number</description></item>
        /// <item><description>Account Holder Name</description></item>
        /// <item><description>Current Balance</description></item>
        /// <item><description>Account Privilege Level</description></item>
        /// <item><description>Account Type (Savings, Current, Fixed Deposit, Salary)</description></item>
        /// <item><description>Account Holder Age</description></item>
        /// </list>
        /// </remarks>
        /// <example>
        /// <code>
        /// var allAccounts = accountService.GetAllAccounts();
        /// foreach (var account in allAccounts)
        /// {
        ///     Console.WriteLine($"{account.Name} - {account.AccountNumber}: {account.Balance}");
        /// }
        /// </code>
        /// </example>
        public List<ViewAllAccountsResponseDto> GetAllAccounts()
        {
            // Retrieve all accounts from the repository
            List<IAccount> accounts = _accountRepository.GetAllAccounts();

            // Initialize collection for DTOs
            List<ViewAllAccountsResponseDto> dto = new List<ViewAllAccountsResponseDto>();

            // Transform each account domain model to a DTO
            foreach (var account in accounts)
            {
                dto.Add(new ViewAllAccountsResponseDto()
                {
                    Name = account.Name,
                    AccountNumber = account.AccountNumber,
                    Balance = account.Balance,
                    AccountPrivilege = account.Privilege,
                    AccountType = account.AccountType,
                    Age = account.Age
                });
            }

            return dto;
        }

        /// <summary>
        /// Retrieves the balance of a specific account asynchronously.
        /// </summary>
        /// <param name="accNo">The account number for which to retrieve the balance.</param>
        /// <returns>
        /// A task representing the asynchronous operation that returns a <see cref="ViewBalanceResponseDto"/>
        /// containing the account number and current balance.
        /// </returns>
        /// <remarks>
        /// This method provides a lightweight query for balance information without retrieving the entire account details.
        /// It is useful for quick balance inquiries and displays.
        /// </remarks>
        /// <example>
        /// <code>
        /// var balanceInfo = await accountService.GetBalanceAsync("ACC123456");
        /// Console.WriteLine($"Account: {balanceInfo.AccountNumber}, Balance: {balanceInfo.Balance}");
        /// </code>
        /// </example>
        /// 
        public async Task<ViewBalanceResponseDto> GetBalanceAsync(string accNo)
        {
            // Retrieve account from repository
            var account = await _accountRepository.GetAccountAsync(accNo);

            // Map to balance response DTO
            return new ViewBalanceResponseDto()
            {
                AccountNumber = account.AccountNumber,
                Balance = account.Balance
            };
        }

        /// <summary>
        /// Retrieves comprehensive account details asynchronously.
        /// </summary>
        /// <param name="accNo">The account number to retrieve.</param>
        /// <returns>
        /// A task representing the asynchronous operation that returns a <see cref="ViewAccountResponseDto"/>
        /// containing the account number, account holder name, and current balance.
        /// </returns>
        /// <remarks>
        /// This method provides a view of the account with essential information including the account holder's name
        /// and current balance. It is typically used for account summary displays.
        /// </remarks>
        /// <example>
        /// <code>
        /// var accountInfo = await accountService.ViewAccountAsync("ACC123456");
        /// Console.WriteLine($"{accountInfo.Name}'s Account: {accountInfo.AccountNumber}");
        /// </code>
        /// </example>
        public async Task<ViewAccountResponseDto> ViewAccountAsync(string accNo)
        {
            // Retrieve account from repository
            var account = await _accountRepository.GetAccountAsync(accNo);

            // Map to account view response DTO
            return new ViewAccountResponseDto()
            {
                AccountNumber = account.AccountNumber,
                Balance = account.Balance,
                Name = account.Name
            };
        }

        /// <summary>
        /// Closes an existing account with validation asynchronously.
        /// </summary>
        /// <param name="request">
        /// A <see cref="CloseAccountRequestDto"/> containing the account number to close.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation that returns a <see cref="CloseAccountResponseDto"/>
        /// containing the account number, updated status, and confirmation message.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when:
        /// <list type="bullet">
        /// <item><description>The specified account is not found in the system.</description></item>
        /// <item><description>The account is already closed.</description></item>
        /// </list>
        /// </exception>
        /// <remarks>
        /// Before closing an account, this method validates that:
        /// <list type="bullet">
        /// <item><description>The account exists in the system</description></item>
        /// <item><description>The account has not already been closed</description></item>
        /// </list>
        /// Once validated, the account is closed in the repository and logged for audit purposes.
        /// This method provides both business logic validation and operational logging.
        /// </remarks>
        /// <example>
        /// <code>
        /// var request = new CloseAccountRequestDto { AccountNumber = "ACC123456" };
        /// var response = await accountService.CloseAccountAsync(request);
        /// Console.WriteLine(response.Message); // "Account closed successfully."
        /// </code>
        /// </example>
        public async Task<CloseAccountResponseDto> CloseAccountAsync(CloseAccountRequestDto request)
        {
            // Retrieve account from repository
            IAccount account = await _accountRepository.GetAccountAsync(request.AccountNumber);

            // Validate account exists
            if (account == null)
            {
                _logger.LogWarning("Close requested for unknown account {AccountNumber}", request.AccountNumber);
                throw new Exception("Account not found.");
            }

            // Validate account is not already closed
            if (account.Status == AccountStatus.Closed)
            {
                _logger.LogWarning("Close requested for already closed account {AccountNumber}", request.AccountNumber);
                throw new Exception("Account is already closed.");
            }

            // Close account in repository
            _accountRepository.CloseAccount(request.AccountNumber);

            // Log successful closure
            _logger.LogInformation("Account {AccountNumber} closed", request.AccountNumber);

            // Return closure confirmation
            return new CloseAccountResponseDto()
            {
                AccountNumber = request.AccountNumber,
                Status = AccountStatus.Closed,
                Message = "Account closed successfully."
            };
        }

        /// <summary>
        /// Creates a new account with the specified details.
        /// </summary>
        /// <param name="request">
        /// A <see cref="CreateAccountRequestDto"/> containing all required information for account creation,
        /// including account type, holder information, initial balance, and type-specific parameters.
        /// </param>
        /// <returns>
        /// A <see cref="CreateAccountResponseDto"/> containing the created account's details including
        /// account number, holder information, balance, account type, status, privilege level, and
        /// type-specific properties (overdraft limit, tenure, interest rate, minimum balance, employer name).
        /// </returns>
        /// <remarks>
        /// This method orchestrates the account creation process by:
        /// <list type="bullet">
        /// <item><description>Using the account factory to create the appropriate account type based on the request</description></item>
        /// <item><description>Persisting the account and its PIN in the repository</description></item>
        /// <item><description>Logging the creation event for audit trails</description></item>
        /// <item><description>Mapping the created account to a response DTO with all relevant information</description></item>
        /// </list>
        /// The method supports creating accounts of different types: Savings, Current, Fixed Deposit, and Salary accounts.
        /// Each account type may have specific properties that are conditionally included in the response:
        /// <list type="bullet">
        /// <item><description>Current Accounts: Include overdraft limit</description></item>
        /// <item><description>Fixed Deposit Accounts: Include tenure months and interest rate</description></item>
        /// <item><description>Savings Accounts: Include interest rate and minimum balance</description></item>
        /// <item><description>Salary Accounts: Include employer name</description></item>
        /// </list>
        /// </remarks>
        /// <example>
        /// <code>
        /// var createRequest = new CreateAccountRequestDto
        /// {
        ///     AccountType = AccountType.SAVINGS,
        ///     AccountNumber = "ACC123456",
        ///     Name = "John Doe",
        ///     Age = 30,
        ///     Balance = 5000,
        ///     Pin = "1234",
        ///     Status = AccountStatus.ACTIVE,
        ///     Privilege = AccountPrivilege.REGULAR,
        ///     InterestRate = 4.5,
        ///     MinimumBalance = 1000
        /// };
        /// 
        /// var response = await accountService.CreateAccount(createRequest);
        /// Console.WriteLine($"Account created: {response.AccountNumber}");
        /// </code>
        /// </example>
        public CreateAccountResponseDto CreateAccount(CreateAccountRequestDto request)
        {
            // Create appropriate account type using factory pattern
            Account account = AccountFactory.CreateAccount(
                request.AccountType,
                request.AccountNumber,
                request.Name,
                request.Age,
                request.Balance,
                request.Status,
                request.Pin,
                request.Privilege,
                request.OverdraftLimit,
                request.TenureMonths,
                request.InterestRate,
                request.MinimumBalance,
                request.EmployerName
            );

            // Persist account to repository
            _accountRepository.SaveAccount(account, request.Pin);

            // Log account creation
            _logger.LogInformation("Created {AccountType} account {AccountNumber}", account.AccountType, account.AccountNumber);

            // Build and return response with type-specific properties using pattern matching
            return new CreateAccountResponseDto()
            {
                AccountNumber = account.AccountNumber,
                Name = account.Name,
                Age = account.Age,
                Balance = account.Balance,
                AccountType = account.AccountType,
                Status = account.Status,
                Privilege = account.Privilege,

                // Include overdraft limit only for Current accounts
                OverdraftLimit = account is CurrentAccount current
                    ? current.OverdraftLimit
                    : 0,

                // Include tenure months only for Fixed Deposit accounts
                TenureMonths = account is FixedDepositAccount fixedDeposit
                    ? fixedDeposit.TenureMonths
                    : 0,

                // Include interest rate for Savings or Fixed Deposit accounts
                InterestRate = account is SavingsAccount savings
                    ? savings.InterestRate
                    : account is FixedDepositAccount fd
                        ? fd.InterestRate
                        : 0,

                // Include minimum balance only for Savings accounts
                MinimumBalance = account is SavingsAccount savingsAccount
                    ? savingsAccount.MinBalance
                    : 0,

                // Include employer name only for Salary accounts
                EmployerName = account is SalaryAccount salary
                    ? salary.EmployerName
                    : null
            };
        }
    }
}
