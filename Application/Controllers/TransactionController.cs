using GDB.App.Application.Services.Contracts;
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

        public async Task<DepositResponseDto> DepositAsync(
            string accountNumber, decimal amount)
        {
            TransactionDto transactionDto =
                new TransactionDto
                {
                    AccountNumber = accountNumber,
                    Amount = amount
                };

            return await _transactionService
                .ProcessTransactionAsync<DepositResponseDto>(
                    transactionDto,
                    TransactionType.Deposit);
        }

        public async Task<WithdrawResponseDto> WithdrawAsync(
            string accountNumber, string pin, decimal amount)
        {
            TransactionDto transactionDto =
                new TransactionDto
                {
                    AccountNumber = accountNumber,
                    Pin = pin,
                    Amount = amount
                };

            return await _transactionService
                .ProcessTransactionAsync<WithdrawResponseDto>(
                    transactionDto,
                    TransactionType.Withdraw);
        }

        public async Task<TranferFundsResponseDto> TransferFundsAsync(
            string fromAccountNumber,
            string toAccountNumber,
            string pin,
            decimal amount)
        {
            TransactionDto transactionDto =
                new TransactionDto
                {
                    FromAccount = fromAccountNumber,
                    ToAccount = toAccountNumber,
                    Pin = pin,
                    Amount = amount
                };

            return await _transactionService
                .ProcessTransactionAsync<TranferFundsResponseDto>(
                    transactionDto,
                    TransactionType.Transfer);
        }

        public async Task<List<ViewRecentTransactionsResponseDto>>
            GetRecentTransactionsAsync(string accountNumber)
        {
            return await _transactionQueryService
                .GetRecentTransactionsAsync(accountNumber);
        }
    }
}
