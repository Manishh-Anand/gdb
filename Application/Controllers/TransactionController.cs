using GDB.App.Application.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDB.App.Domain.Enums;
using GDB.App.Application.Services;
using GDB.App.Application.Dtos;

namespace GDB.App.Application.Controllers
{
    public class TransactionController
    {
        private readonly ITransactionService _transactionService;
        private readonly ITransactionQueryService _transactionQueryService;


        public TransactionController()
        {
            _transactionService =
                TransactionServiceFactory.Create();

            _transactionQueryService =
                TransactionQueryServiceFactory.Create();
        
        }

        public async Task<DepositResponseDto> DepositAsync(string accountNumber, decimal amount)
        {
            // return await _transactionService.DepositAsync(accountNumber, amount);
            TransactionDto transactionDto =
                new TransactionDto
                {
                    AccountNumber = accountNumber,
                    Amount = amount
                };

            ITransactionCommand<DepositResponseDto> command =
                TransactionCommandFactory.CreateDepositCommand();

            return await _transactionService.ExecuteAsync(
                command,
                transactionDto);
        }

        public async Task<WithdrawResponseDto> WithdrawAsync(string accountNumber,string pin,decimal amount)
        {
            // return await _transactionService.WithdrawAsync(accountNumber,pin,amount);
            TransactionDto transactionDto =
                  new TransactionDto
                  {
                      AccountNumber = accountNumber,
                      Pin = pin,
                      Amount = amount
                  };

            ITransactionCommand<WithdrawResponseDto> command =
                TransactionCommandFactory.CreateWithdrawCommand();

            return await _transactionService.ExecuteAsync(
                command,
                transactionDto);
        }

        public async Task<TranferFundsResponseDto> TransferFundsAsync(
            string fromAccountNumber,
            string toAccountNumber,
            string pin,
            decimal amount)
        {
            //return await _transactionService.TransferFundsAsync(
            //                        fromAccountNumber,
            //                        toAccountNumber,
            //                        pin,
            //                        amount
            //                    );
            TransactionDto transactionDto =
                 new TransactionDto
                 {
                     FromAccount = fromAccountNumber,
                     ToAccount = toAccountNumber,
                     Pin = pin,
                     Amount = amount
                 };

            ITransactionCommand<TranferFundsResponseDto> command =
                TransactionCommandFactory.CreateTransferCommand();

            return await _transactionService.ExecuteAsync(
                command,
                transactionDto);
        }
        public async Task<List<ViewRecentTransactionsResponseDto>>
            GetRecentTransactionsAsync(string accountNumber)
        {
            return await _transactionQueryService.GetRecentTransactionsAsync(
                accountNumber);
        }
    }
}
