using GDB.App.Application.Services.Contracts;
using GDB.App.Application.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDB.App.Application.Services
{
    public static class TransactionServiceFactory
    {

        public static ITransactionService Create()
        {
            return new TransactionService();
        }
    }
}
