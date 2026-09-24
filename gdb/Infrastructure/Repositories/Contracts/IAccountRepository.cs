using GDB.App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDB.App.Infrastructure.Repositories.Contracts
{
    public interface IAccountRepository
    {
        Task<IAccount> GetAccountAsync(string accountNumber);
        void CloseAccount(string accountNumber);
        void SaveAccount(IAccount account, string pin);
        List<IAccount> GetAllAccounts();
        void SaveAccounts(IAccount fromAccount, IAccount toAccount);
        //void ChangePin(string accountNumber, string oldPin, string newPin);
        void UpdateBalance(string accountNumber, decimal balance);

    }
}
