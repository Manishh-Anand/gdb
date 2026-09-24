using GDB.App.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDB.App.Application.Services.Contracts
{
    public interface ITransactionQueryService
    {
        Task<List<ViewRecentTransactionsResponseDto>>
            GetRecentTransactionsAsync(string accountNumber);
    }
}
