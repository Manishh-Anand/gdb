using GDB.App.Application.Services.Contracts;
using GDB.App.Application.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDB.App.Application.Services
{
    public static class TransactionQueryServiceFactory
    {
        public static ITransactionQueryService Create()
        {
            return new TransactionQueryService();
        }
    }
}
