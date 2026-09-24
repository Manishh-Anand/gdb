using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDB.App.Domain.Enums;

namespace GDB.App.Application.Dtos
{
    public class WithdrawResponseDto
    {
        public decimal Balance  { get; set; }

        public TransactionStatus TransactionStat { get; set; }
    }
}
