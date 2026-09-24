using GDB.App.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDB.App.Domain.Enums;

namespace GDB.App.Infrastructure.Repositories.Contracts
{
    public interface ITransactionRepository
    {
        List<ViewRecentTransactionsResponseDto> GetRecentTransactions(
        string accountNumber);

        void SaveTransaction(
           string fromAccountNumber,
           string toAccountNumber,
           TransactionType transactionType,
           decimal amount,
           TransactionStatus transactionStatus,
           decimal balanceAfterFrom,
           decimal balanceAfterTo);
    }
}
