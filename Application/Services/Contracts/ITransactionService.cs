using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDB.App.Application.Dtos;
using GDB.App.Domain.Enums;

namespace GDB.App.Application.Services.Contracts
{
    public interface ITransactionService
    {
        Task<TResponse> ProcessTransactionAsync<TResponse>(
            TransactionDto transactionDto,
            TransactionType transactionType);
    }
}
