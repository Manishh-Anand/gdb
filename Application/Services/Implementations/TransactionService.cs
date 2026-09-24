using GDB.App.Application.Dtos;
using GDB.App.Application.Services.Contracts;
using GDB.App.Domain.Enums;
using GDB.App.Domain.Exceptions;
using GDB.App.Domain.Models;
using GDB.App.Infrastructure.Repositories;
using GDB.App.Infrastructure.Repositories.Contracts;
using GDB.App.Infrastructure.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using gdb.Logging;
using Microsoft.Extensions.Logging;


namespace GDB.App.Application.Services.Implementations
{
    public class TransactionService : ITransactionService
    {

        public async Task<TResponse> ExecuteAsync<TResponse>(ITransactionCommand<TResponse> command,TransactionDto transactionDto)
        {
            return await command.ExecuteAsync(transactionDto);
        }
        //private readonly IAccountRepository _accountRepository;
        //private readonly ITransactionRepository _transactionRepository;
        //private static readonly ILogger _logger = AppLogger.CreateLogger<TransactionService>();

        //public TransactionService()
        //{
        //    _accountRepository = AccountRepositoryFactory.Create("DB");
        //    _transactionRepository = TransactionRepositoryFactory.Create("DB");
        //}

        //public async Task<DepositResponseDto> DepositAsync(string accountNumber, decimal amount)
        //{
        //    IAccount account = await _accountRepository.GetAccountAsync(accountNumber);

        //    if (CheckIfAccountIsNull(account))
        //    {
        //        _logger.LogWarning("Deposit failed: account {AccountNumber} not found", accountNumber);
        //        throw new AccountException("Account not found");
        //    }

        //    //should follow domain driven architecture
        //    account.Deposit(amount);//method in Account.cs

        //    _accountRepository.UpdateBalance(
        //        accountNumber,
        //        account.Balance
        //    );
        //    _transactionRepository.SaveTransaction(
        //        null,
        //        accountNumber,
        //        TransactionType.Deposit,
        //        amount,
        //        TransactionStatus.Success,
        //        0,
        //        account.Balance
        //    );
        //    _logger.LogInformation("Deposited {Amount} to {AccountNumber}", amount, accountNumber);
        //    return new DepositResponseDto()
        //    {
        //        Balance = account.Balance,
        //        TransactionStat = TransactionStatus.Success
        //    };
        //}

        //public bool CheckIfAccountIsNull(IAccount account)
        //{
        //    if (account == null)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        //public async Task<WithdrawResponseDto> WithdrawAsync(
        //    string accountNumber,
        //    string pin,
        //    decimal amount)
        //{
        //    IAccount account = await _accountRepository.GetAccountAsync(accountNumber);

        //    if (CheckIfAccountIsNull(account))
        //    {
        //        _logger.LogWarning("Withdraw failed: account {AccountNumber} not found", accountNumber);
        //        throw new Exception("Account not found");
        //    }

        //    //This should also follow domain driven architecture
        //    account.Withdraw( amount, pin);//method in Account.cs
        //    //We can make a method for this 
        //    _accountRepository.UpdateBalance(
        //            accountNumber,
        //            account.Balance
        //        );

        //    _transactionRepository.SaveTransaction(
        //        accountNumber,
        //        null,
        //        TransactionType.Withdraw,
        //        amount,
        //        TransactionStatus.Success,
        //        account.Balance,
        //        0
        //    );
        //    _logger.LogInformation("Withdrew {Amount} from {AccountNumber}", amount, accountNumber);

        //    return new WithdrawResponseDto()
        //    {
        //        Balance = account.Balance,
        //        TransactionStat = TransactionStatus.Success
        //    };
        //}

        //public async Task<TranferFundsResponseDto> TransferFundsAsync(string fromAccountNumber,
        //                        string toAccountNumber,
        //                        string pin,
        //                        decimal amount)
        //{
        //    TransactionStatus status = TransactionStatus.Pending;

        //    IAccount fromAccount = null;
        //    IAccount toAccount = null;

        //    try
        //    {
        //        // Get From Account
        //        fromAccount = await GetAccountAsync(fromAccountNumber);
        //        if (CheckIfAccountIsNull(fromAccount))
        //        {
        //            _logger.LogWarning("Transfer failed: from account {AccountNumber} not found", fromAccountNumber);
        //            throw new Exception("From account not found");
        //        }

        //        // Check From Account
        //        CheckIfAccountIsActive(fromAccount);

        //        // Get To Account
        //        toAccount =await  GetAccountAsync(toAccountNumber);

        //        if (CheckIfAccountIsNull(toAccount))
        //        {
        //            _logger.LogWarning("Transfer failed: to account {AccountNumber} not found", toAccountNumber);
        //            throw new Exception("To account not found");
        //        }

        //        // Check To Account
        //        CheckIfAccountIsActive(toAccount);

        //        // Check PIN
        //        CheckIfPinIsValid(fromAccount, pin);

        //        //Console.WriteLine("BEFORE TRANSFER");

        //        //DisplayAccount("FROM ACCOUNT", fromAccount);
        //        //DisplayAccount("TO ACCOUNT", toAccount);

        //        // Withdraw from sender
        //        fromAccount.Withdraw(amount, pin);

        //        // Deposit into receiver
        //        toAccount.Deposit(amount);

        //        // Save both updated balances
        //        _accountRepository.SaveAccounts(
        //            fromAccount,
        //            toAccount
        //        );
        //        _transactionRepository.SaveTransaction(
        //            fromAccountNumber,
        //            toAccountNumber,
        //            TransactionType.Transfer,
        //            amount,
        //            TransactionStatus.Success,
        //            fromAccount.Balance,
        //            toAccount.Balance
        //        );

        //        //Console.WriteLine("AFTER TRANSFER");

        //        //DisplayAccount("FROM ACCOUNT", fromAccount);
        //        //DisplayAccount("TO ACCOUNT", toAccount);

        //        status = TransactionStatus.Success;
        //        _logger.LogInformation("Transferred {Amount} from {FromAccount} to {ToAccount}", amount, fromAccountNumber, toAccountNumber);
        //    }
        //    catch (InactiveAccountException)
        //    {
        //        _logger.LogWarning("Transfer {FromAccount} -> {ToAccount} rejected: inactive account", fromAccountNumber, toAccountNumber);
        //        throw new InactiveAccountException();
        //    }
        //    catch (InvalidPinException)
        //    {
        //        _logger.LogWarning("Transfer from {FromAccount} rejected: invalid PIN", fromAccountNumber);
        //        throw new InvalidPinException();
        //    }

        //    return new TranferFundsResponseDto()
        //    {
        //        FromAccountNumber = fromAccountNumber,
        //        ToAccountNumber = toAccountNumber,
        //        Amount = amount,
        //        FromAccountBalance = fromAccount.Balance,
        //        ToAccountBalance = toAccount.Balance,
        //        TransactionStat = status
        //    };
        //}



        //private bool CheckIfPinIsValid(IAccount account, string pinNumber)
        //{



        //    if (!account.ValidatePin(pinNumber))
        //        throw new InvalidPinException();



        //    return true;

        //}



        //private bool CheckIfAccountIsActive(IAccount account)
        //{



        //    if (!account.CheckIfAccountIsActive())
        //        throw new InactiveAccountException();



        //    return true;

        //}



        ////SRP - Single Responsiblity Principle

        ////To get the acount information only

        //private async Task<IAccount> GetAccountAsync(string accountNumber)
        //{

        //    // Get Account info from the database

        //    //Moment you create ab object of a class inside a method, then you are directly

        //    //dependent on the object. Tight Coupling

        //    //AccountfactoryRepository creatae method is returning an interace

        //    //therefore the TransferSerivce is programming to an interace and not implementation

        //    //Once - Use and Dispose - Uses Relationship

        //    //var accountRepository = AccountRepositoryFactory.Create();

        //    var account = await _accountRepository.GetAccountAsync(accountNumber);



        //    return account;

        //}
        ////private void DisplayAccount(string message, IAccount account)
        ////{
        ////    Console.WriteLine(message);
        ////    Console.WriteLine("--------------------------------");
        ////    Console.WriteLine($"Account Number : {account.AccountNumber}");
        ////    Console.WriteLine($"Name           : {account.Name}");
        ////    Console.WriteLine($"Balance        : {account.Balance}");
        ////    Console.WriteLine();
        ////}
        //public async Task<List<ViewRecentTransactionsResponseDto>> GetRecentTransactionsAsync(string accountNumber)
        //{
        //    IAccount account =
        //        await _accountRepository.GetAccountAsync(accountNumber);

        //    if (account == null)
        //        throw new Exception("Account not found.");

        //    return _transactionRepository.GetRecentTransactions(
        //        accountNumber);
        //}

        ////public List<ViewRecentTransactionsResponseDto> GetRecentTransactions(string accountNumber)
        ////{
        ////    throw new NotImplementedException();
        ////}

        ////public Task<DepositResponseDto> DepositAsync(string accountNumber, decimal amount)
        ////{
        ////    throw new NotImplementedException();
        ////}

        ////Task<WithdrawResponseDto> ITransactionService.WithdrawAsync(string accountNumber, string pin, decimal amount)
        ////{

        ////    throw new NotImplementedException();
        ////}

        ////TranferFundsResponseDto ITransactionService.TransferFunds(string fromAccountNumber, string toAccountNumber, string pin, decimal amount)
        ////{
        ////    throw new NotImplementedException();
        ////}
    }
}
