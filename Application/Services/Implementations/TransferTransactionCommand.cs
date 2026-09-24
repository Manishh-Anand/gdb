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
    public class TransferTransactionCommand
    : ITransactionCommand<TranferFundsResponseDto>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;

        private static readonly ILogger _logger =
            AppLogger.CreateLogger<TransferTransactionCommand>();

        public TransferTransactionCommand(
            IAccountRepository accountRepository,
            ITransactionRepository transactionRepository)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<TranferFundsResponseDto> ExecuteAsync(
            TransactionDto transactionDto)
        {
            // Get sender
            IAccount fromAccount =
                await _accountRepository.GetAccountAsync(
                    transactionDto.FromAccount);

            if (fromAccount == null)
            {
                _logger.LogWarning(
                    "Transfer failed: from account {AccountNumber} not found",
                    transactionDto.FromAccount);

                throw new AccountException(
                    "From account not found");
            }

            // Get receiver
            IAccount toAccount =
                await _accountRepository.GetAccountAsync(
                    transactionDto.ToAccount);

            if (toAccount == null)
            {
                _logger.LogWarning(
                    "Transfer failed: to account {AccountNumber} not found",
                    transactionDto.ToAccount);

                throw new AccountException(
                    "To account not found");
            }

            // Check sender is active
            if (!fromAccount.CheckIfAccountIsActive())
            {
                throw new InactiveAccountException();
            }

            // Check receiver is active
            if (!toAccount.CheckIfAccountIsActive())
            {
                throw new InactiveAccountException();
            }

            // Check PIN
            if (!fromAccount.ValidatePin(
                    transactionDto.Pin))
            {
                throw new InvalidPinException();
            }

            // Withdraw from sender
            fromAccount.Withdraw(
                transactionDto.Amount,
                transactionDto.Pin);

            // Deposit into receiver
            toAccount.Deposit(
                transactionDto.Amount);

            // Save both accounts
            _accountRepository.SaveAccounts(
                fromAccount,
                toAccount);

            // Save transaction
            _transactionRepository.SaveTransaction(
                transactionDto.FromAccount,
                transactionDto.ToAccount,
                TransactionType.Transfer,
                transactionDto.Amount,
                TransactionStatus.Success,
                fromAccount.Balance,
                toAccount.Balance);

            _logger.LogInformation(
                "Transferred {Amount} from {FromAccount} to {ToAccount}",
                transactionDto.Amount,
                transactionDto.FromAccount,
                transactionDto.ToAccount);

            return new TranferFundsResponseDto
            {
                FromAccountNumber =
                    transactionDto.FromAccount,

                ToAccountNumber =
                    transactionDto.ToAccount,

                Amount =
                    transactionDto.Amount,

                FromAccountBalance =
                    fromAccount.Balance,

                ToAccountBalance =
                    toAccount.Balance,

                TransactionStat =
                    TransactionStatus.Success
            };
        }
    }
}
