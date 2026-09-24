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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDB.App.Application.Services.Implementations
{
    public class DepositTransactionCommand
        : ITransactionCommand<DepositResponseDto>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;

        private static readonly ILogger _logger =
            AppLogger.CreateLogger<DepositTransactionCommand>();

        public DepositTransactionCommand(
            IAccountRepository accountRepository,
            ITransactionRepository transactionRepository)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<DepositResponseDto> ExecuteAsync(
            TransactionDto transactionDto)
        {
            IAccount account =
                await _accountRepository.GetAccountAsync(
                    transactionDto.AccountNumber);

            if (account == null)
            {
                _logger.LogWarning(
                    "Deposit failed: account {AccountNumber} not found",
                    transactionDto.AccountNumber);

                throw new AccountException(
                    "Account not found");
            }

            // Domain handles the actual deposit rules
            account.Deposit(transactionDto.Amount);

            // Update account balance
            _accountRepository.UpdateBalance(
                transactionDto.AccountNumber,
                account.Balance);

            // Save transaction record
            _transactionRepository.SaveTransaction(
                null,
                transactionDto.AccountNumber,
                TransactionType.Deposit,
                transactionDto.Amount,
                TransactionStatus.Success,
                0,
                account.Balance);

            _logger.LogInformation(
    "Deposited {Amount} to {AccountNumber}",
    transactionDto.Amount.ToString("C", new CultureInfo("en-IN")),
    transactionDto.AccountNumber);

            return new DepositResponseDto
            {
                Balance = account.Balance,
                TransactionStat = TransactionStatus.Success
            };
        }
    }
}
