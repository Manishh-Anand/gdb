using GDB.App.Application.Dtos;
using GDB.App.Application.Services.Contracts;
using GDB.App.Application.Services.Implementations;
using GDB.App.Infrastructure.Repositories;
using GDB.App.Infrastructure.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDB.App.Application.Services
{
    public static class TransactionCommandFactory
    {
        public static ITransactionCommand<DepositResponseDto>
            CreateDepositCommand()
        {
            IAccountRepository accountRepository =
                AccountRepositoryFactory.Create("DB");

            ITransactionRepository transactionRepository =
                TransactionRepositoryFactory.Create("DB");

            return new DepositTransactionCommand(
                accountRepository,
                transactionRepository);
        }


        public static ITransactionCommand<WithdrawResponseDto>
            CreateWithdrawCommand()
        {
            IAccountRepository accountRepository =
                AccountRepositoryFactory.Create("DB");

            ITransactionRepository transactionRepository =
                TransactionRepositoryFactory.Create("DB");

            return new WithdrawTransactionCommand(
                accountRepository,
                transactionRepository);
        }


        public static ITransactionCommand<TranferFundsResponseDto>
            CreateTransferCommand()
        {
            IAccountRepository accountRepository =
                AccountRepositoryFactory.Create("DB");

            ITransactionRepository transactionRepository =
                TransactionRepositoryFactory.Create("DB");

            return new TransferTransactionCommand(
                accountRepository,
                transactionRepository);
        }
    }
}
