using GDB.App.Application.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDB.App.Application.Services.Contracts;
using GDB.App.Application.Services.Implementations;
using GDB.App.Application.Services.Contracts;

namespace GDB.App.Application.Services
{
    public class AccountServiceFactory
    {
        public static IAccountService Create()
        {
            return new AccountService();
        }
    }
}