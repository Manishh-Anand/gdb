using GDB.App.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDB.App.Application.Dtos
{
    public class CloseAccountResponseDto
    {
        public string AccountNumber { get; set; }
        public AccountStatus Status { get; set; }
        public string Message { get; set; }
    }
}
