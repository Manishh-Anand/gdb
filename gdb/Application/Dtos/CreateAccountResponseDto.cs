using GDB.App.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDB.App.Application.Dtos
{
    public class CreateAccountResponseDto
    {
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public decimal Balance { get; set; }
        

        public AccountType AccountType { get; set; }
        public AccountStatus Status { get; set; }
        public AccountPrivilege Privilege { get; set; }

        public decimal OverdraftLimit { get; set; }
        public int TenureMonths { get; set; }
        public double InterestRate { get; set; }
        public decimal MinimumBalance { get; set; }
        public string EmployerName { get; set; }
    }
}
