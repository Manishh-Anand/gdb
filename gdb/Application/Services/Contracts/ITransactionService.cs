using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDB.App.Application.Dtos;
using GDB.App.Domain.Enums;
using GDB.App.Application.Dtos;

namespace GDB.App.Application.Services.Contracts
{
    public interface ITransactionService
    {
        //Task<List<ViewRecentTransactionsResponseDto>> GetRecentTransactionsAsync(
        //   string accountNumber);
        //Task<DepositResponseDto> DepositAsync(string accountNumber, decimal amount);

        //Task<WithdrawResponseDto> WithdrawAsync(string accountNumber, string pin, decimal amount);

        //Task<TranferFundsResponseDto> TransferFundsAsync(
        //    string fromAccountNumber,
        //    string toAccountNumber,
        //    string pin,
        //    decimal amount);

        Task<TResponse> ExecuteAsync<TResponse>(
            ITransactionCommand<TResponse> command,
            Dtos.TransactionDto transactionDto);
    }
}
