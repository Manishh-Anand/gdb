using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDB.App.Domain.Enums;

namespace GDB.App.Application.Dtos
{
    public class ViewRecentTransactionsResponseDto
    {
        public int TransactionId { get; set; }

        public string FromAccountNumber { get; set; }

        public string ToAccountNumber { get; set; }

        public decimal Amount { get; set; }

        public TransactionType TransactionType { get; set; }

        public TransactionStatus TransactionStatus { get; set; }

        public DateTime Timestamp { get; set; }

        public decimal? BalanceAfterFrom { get; set; }

        public decimal? BalanceAfterTo { get; set; }
    }
}
