using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDB.App.Application.Dtos
{
    public class ViewBalanceResponseDto
    {
        public string AccountNumber { get; set; }
        
        public decimal Balance { get; set; }
    }
}
