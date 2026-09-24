using GDB.App.Data;
using GDB.App.Domain;
using GDB.App.Domain.Enums;
using GDB.App.Domain.Models;
using GDB.App.Infrastructure.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using GDB.App.Infrastructure.Repositories;

namespace GDB.App.Infrastructure.Repositories.Implementations
{
    internal class AccountRepositoryInMemory : IAccountRepository
    {


        private static DataSet _dataSet;

        public AccountRepositoryInMemory()
        {

            if (_dataSet == null)
            {
                _dataSet = GDBInMemoryDataStore.DataSet;
            }

        }

        public async Task<IAccount> GetAccountAsync(string accountNumber)
        {
            //return new SavingsAccount(accountNumber,"Hasini",21,100000m,AccountType.Savings,AccountStatus.Active, "1234", AccountPrivilege.Gold,1000m,4);
            DataTable accountTable = _dataSet.Tables["ACCOUNT"];

            DataRow row = accountTable.Select($"AccountNumber = '{accountNumber}'").FirstOrDefault();

            if (row == null)
            {
                return null;
            }

            // Get values from DataSet
            string number = row["AccountNumber"].ToString();
            string name = row["Name"].ToString();
            int age = Convert.ToInt32(row["Age"]);
            decimal balance = Convert.ToDecimal(row["Balance"]);

            string type = row["AccountType"].ToString();
            string status = row["Status"].ToString();

            string pin = row["Pin"].ToString();
            string privilage = row["Privilage"].ToString();


            // Convert AccountType string to enum
            AccountType accountType = type switch
            {
                "Savings" => AccountType.Savings,
                "Current" => AccountType.Current,
                "Salary" => AccountType.Salary,
                "FixedDeposit" => AccountType.FixedDeposit,
                _ => throw new Exception("Invalid account type")
            };

            // Convert Status string to enum
            AccountStatus accountStatus = status switch
            {
                "Active" => AccountStatus.Active,
                "Inactive" => AccountStatus.Inactive,
                "Frozen" => AccountStatus.Frozen,
                _ => throw new Exception("Invalid account status")
            };

            // Convert Privilege string to enum
            AccountPrivilege privilege = privilage switch
            {
                "Gold" => AccountPrivilege.Gold,
                "Silver" => AccountPrivilege.Silver,
                "Premium" => AccountPrivilege.Premium,
                _ => throw new Exception("Invalid account privilege")
            };

            // Create the correct Account object
            IAccount account = AccountFactory.CreateAccount(
                accountType,
                number,
                name,
                age,
                balance,
                accountStatus,
                pin,
                privilege
            );

            return (IAccount)Task.FromResult(account);
        }
        public void SaveAccounts(IAccount fromAccount, IAccount toAccount)
        {
            DataTable accountTable = _dataSet.Tables["ACCOUNT"];

            DataRow fromRow = accountTable.Rows.Find(fromAccount.AccountNumber);
            DataRow toRow = accountTable.Rows.Find(toAccount.AccountNumber);

            if (fromRow == null || toRow == null)
            {
                throw new Exception("Account not found");
            }

            // No withdrawal/deposit logic here.
            // Just take the already updated values from the objects.

            fromRow["Balance"] = fromAccount.Balance;
            toRow["Balance"] = toAccount.Balance;
        }
        public List<IAccount> GetAllAccounts()
        {
            List<IAccount> accounts = new List<IAccount>();

            DataTable accountTable = _dataSet.Tables["ACCOUNT"];

            foreach (DataRow row in accountTable.Rows)
            {
                string number = row["AccountNumber"].ToString();
                string name = row["Name"].ToString();
                int age = Convert.ToInt32(row["Age"]);
                decimal balance = Convert.ToDecimal(row["Balance"]);

                string type = row["AccountType"].ToString();
                string status = row["Status"].ToString();

                string pin = row["Pin"].ToString();
                string privilegeValue = row["Privilage"].ToString();

                AccountType accountType = type switch
                {
                    "Savings" => AccountType.Savings,
                    "Current" => AccountType.Current,
                    "Salary" => AccountType.Salary,
                    "FixedDeposit" => AccountType.FixedDeposit,
                    _ => throw new Exception("Invalid account type")
                };

                AccountStatus accountStatus = status switch
                {
                    "Active" => AccountStatus.Active,
                    "Inactive" => AccountStatus.Inactive,
                    "Suspended" => AccountStatus.Suspended,
                    "Closed" => AccountStatus.Closed,
                    "Frozen" => AccountStatus.Frozen,
                    _ => throw new Exception("Invalid account status")
                };

                AccountPrivilege privilege = privilegeValue switch
                {
                    "Gold" => AccountPrivilege.Gold,
                    "Silver" => AccountPrivilege.Silver,
                    "Premium" => AccountPrivilege.Premium,
                    _ => throw new Exception("Invalid account privilege")
                };

                IAccount account = AccountFactory.CreateAccount(
                    accountType,
                    number,
                    name,
                    age,
                    balance,
                    accountStatus,
                    pin,
                    privilege
                );

                accounts.Add(account);
            }

            return accounts;
        }
        public void CloseAccount(string accountNumber)
        {
            DataTable accountTable = _dataSet.Tables["ACCOUNT"];

            DataRow row = accountTable.Rows.Find(accountNumber);

            if (row == null)
                throw new Exception("Account not found.");

            string currentStatus = row["Status"].ToString();

            if (currentStatus == "Closed")
                throw new Exception("Account is already closed.");

            row["Status"] = "Closed";
        }
        public void SaveAccount(IAccount account, string pin)
        {
            DataTable accountTable = _dataSet.Tables["ACCOUNT"];

            DataRow row = accountTable.Rows.Find(account.AccountNumber);

            // Account should NOT already exist
            if (row != null)
            {
                throw new Exception("Account already exists");
            }

            // Create a new row
            row = accountTable.NewRow();

            row["AccountNumber"] = account.AccountNumber;
            row["Name"] = account.Name;
            row["Age"] = account.Age;
            row["Balance"] = account.Balance;
            row["AccountType"] = account.AccountType.ToString();
            row["Status"] = account.Status.ToString();
            row["Pin"] = pin;
            row["Privilage"] = account.Privilege.ToString();

            accountTable.Rows.Add(row);
        }

        public void UpdateBalance(string accountNumber, decimal balance)
        {
            DataTable accountTable = _dataSet.Tables["ACCOUNT"];

            DataRow row = accountTable.Rows.Find(accountNumber);

            if (row == null)
            {
                throw new Exception("Account not found");
            }

            row["Balance"] = balance;
        }
        //public void ChangePin(string accountNumber, string oldPin, string newPin)
        //{
        //    DataTable accountTable = _dataSet.Tables["ACCOUNT"];

        //    DataRow row = accountTable.Rows.Find(accountNumber);

        //    if (row == null)
        //    {
        //        throw new Exception("Account not found");
        //    }

        //    string currentPin = row["Pin"].ToString();

        //    if (currentPin != oldPin)
        //    {
        //        throw new Exception("Invalid old PIN");
        //    }

        //    row["Pin"] = newPin;
        //}
    }


}

