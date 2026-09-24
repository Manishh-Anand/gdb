using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDB.App.Application.Dtos
{
    public class TransactionDto
    {
        public string AccountNumber { get; set; }

        public string FromAccount { get; set; }

        public string ToAccount { get; set; }

        public decimal Amount { get; set; }

        public string Pin { get; set; }
    }
    
}
