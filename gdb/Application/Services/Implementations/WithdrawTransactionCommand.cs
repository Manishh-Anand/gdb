using gdb.Logging;
using GDB.App.Application.Dtos;
using GDB.App.Application.Services.Contracts;
using GDB.App.Domain.Enums;
using GDB.App.Domain.Exceptions;
using GDB.App.Domain.Models;
using GDB.App.Infrastructure.Repositories.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDB.App.Application.Services.Implementations
{
    public class WithdrawTransactionCommand
        : ITransactionCommand<WithdrawResponseDto>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;

        private static readonly ILogger _logger =
            AppLogger.CreateLogger<WithdrawTransactionCommand>();

        public WithdrawTransactionCommand(
            IAccountRepository accountRepository,
            ITransactionRepository transactionRepository)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<WithdrawResponseDto> ExecuteAsync(
            TransactionDto transactionDto)
        {
            IAccount account =
                await _accountRepository.GetAccountAsync(
                    transactionDto.AccountNumber);

            if (account == null)
            {
                _logger.LogWarning(
                    "Withdraw failed: account {AccountNumber} not found",
                    transactionDto.AccountNumber);

                throw new AccountException(
                    "Account not found");
            }

            // Domain handles PIN,
            // amount validation,
            // balance validation,
            // account-specific withdrawal rules.
            account.Withdraw(
                transactionDto.Amount,
                transactionDto.Pin);

            // Update balance
            _accountRepository.UpdateBalance(
                transactionDto.AccountNumber,
                account.Balance);

            // Save transaction
            _transactionRepository.SaveTransaction(
                transactionDto.AccountNumber,
                null,
                TransactionType.Withdraw,
                transactionDto.Amount,
                TransactionStatus.Success,
                account.Balance,
                0);

            _logger.LogInformation(
                "Withdrew {Amount} from {AccountNumber}",
                transactionDto.Amount,
                transactionDto.AccountNumber);

            return new WithdrawResponseDto
            {
                Balance = account.Balance,
                TransactionStat = TransactionStatus.Success
            };
        }
    }
}
