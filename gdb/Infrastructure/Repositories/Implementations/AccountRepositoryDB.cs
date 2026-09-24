using gdb.Logging;
using GDB.App.Domain.Enums;
using GDB.App.Domain.Models;
using GDB.App.Infrastructure.Repositories.Contracts;
using GDB.App.Infrastructure.Repositories.Queries;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;

namespace GDB.App.Infrastructure.Repositories.Implementations
{
    internal class AccountRepositoryDB : IAccountRepository
    {
        private static readonly ILogger _logger = AppLogger.CreateLogger<AccountRepositoryDB>();

        public async Task<IAccount> GetAccountAsync(string accountNumber)
        {
            try
            {
                using (DbConnection connection =
                       DataBaseConnectionManager.GetConnection())
                {
                    connection.Open();

                    using (DbCommand command =
                           connection.CreateCommand())
                    {
                        command.CommandText = "GetAccountt";
                        command.CommandType = CommandType.StoredProcedure;

                        AddParameter(
                            command,
                            "@AccountNumber",
                            accountNumber);

                        using (DbDataReader reader =
                               command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return CreateAccount(reader);
                            }
                        }
                    }
                }

                _logger.LogWarning(
                    "Account {AccountNumber} not found in DB",
                    accountNumber);

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch account {AccountNumber}", accountNumber);
                throw;
            }
        }


        public void SaveAccount(
            IAccount account,
            string pin)
        {
            try
            {
                using (DbConnection connection =
                       DataBaseConnectionManager.GetConnection())
                {
                    connection.Open();

                    DbTransaction transaction =
                        connection.BeginTransaction();

                    try
                    {
                        long accountId;

                        using (DbCommand command =
                               connection.CreateCommand())
                        {
                            command.Transaction = transaction;
                            //command.CommandText =
                            //    AccountQueries.InsertAccount;

                            command.CommandText = "InsertAccount";
                            command.CommandType = CommandType.StoredProcedure;

                            AddParameter(
                                command,
                                "@AccountNumber",
                                account.AccountNumber);

                            AddParameter(
                                command,
                                "@Name",
                                account.Name);

                            AddParameter(
                                command,
                                "@Age",
                                account.Age);

                            AddParameter(
                                command,
                                "@AccountType",
                                GetAccountTypeCode(
                                    account.AccountType));

                            AddParameter(
                                command,
                                "@Balance",
                                account.Balance);

                            AddParameter(
                                command,
                                "@AccountStatus",
                                GetAccountStatusCode(
                                    account.Status));

                            AddParameter(
                                command,
                                "@AccountPrivilege",
                                GetAccountPrivilegeCode(
                                    account.Privilege));

                            AddParameter(
                                command,
                                "@Pin",
                                pin);

                            accountId =
                                Convert.ToInt64(
                                    command.ExecuteScalar());
                        }

                        SaveAccountType(
                            account,
                            accountId,
                            connection,
                            transaction);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Rolling back SaveAccount for {AccountNumber}", account.AccountNumber);
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save account {AccountNumber}", account.AccountNumber);
                throw;
            }
        }


        private void SaveAccountType(
            IAccount account,
            long accountId,
            DbConnection connection,
            DbTransaction transaction)
        {
            if (account is SavingsAccount savings)
            {
                SaveSavingsAccount(
                    savings,
                    accountId,
                    connection,
                    transaction);
            }
            else if (account is CurrentAccount current)
            {
                SaveCurrentAccount(
                    current,
                    accountId,
                    connection,
                    transaction);
            }
            else if (account is FixedDepositAccount fixedDeposit)
            {
                SaveFixedDepositAccount(
                    fixedDeposit,
                    accountId,
                    connection,
                    transaction);
            }
            else if (account is SalaryAccount salary)
            {
                SaveSalaryAccount(
                    salary,
                    accountId,
                    connection,
                    transaction);
            }
            else
            {
                _logger.LogWarning("No subtype table for account {AccountNumber} of type {AccountType}", account.AccountNumber, account.GetType().Name);
            }
        }


        private void SaveSavingsAccount(
            SavingsAccount savings,
            long accountId,
            DbConnection connection,
            DbTransaction transaction)
        {
            using (DbCommand command =
                   connection.CreateCommand())
            {
                command.Transaction = transaction;
                //command.CommandText =
                //    AccountQueries.InsertSavingsAccount;

                command.CommandText = "InsertSavingsAccount";
                command.CommandType = CommandType.StoredProcedure;

                AddParameter(
                    command,
                    "@AccountId",
                    accountId);

                AddParameter(
                    command,
                    "@InterestRate",
                    savings.InterestRate / 100.0);

                AddParameter(
                    command,
                    "@MinimumBalance",
                    savings.MinBalance);

                command.ExecuteNonQuery();
            }
        }


        private void SaveCurrentAccount(
            CurrentAccount current,
            long accountId,
            DbConnection connection,
            DbTransaction transaction)
        {
            using (DbCommand command =
                   connection.CreateCommand())
            {
                command.Transaction = transaction;
                //command.CommandText =
                //    AccountQueries.InsertCurrentAccount;
                command.CommandText = "InsertCurrentAccount";
                command.CommandType = CommandType.StoredProcedure;

                AddParameter(
                    command,
                    "@AccountId",
                    accountId);

                AddParameter(
                    command,
                    "@OverdraftLimit",
                    current.OverdraftLimit);

                command.ExecuteNonQuery();
            }
        }


        private void SaveFixedDepositAccount(
            FixedDepositAccount fixedDeposit,
            long accountId,
            DbConnection connection,
            DbTransaction transaction)
        {
            using (DbCommand command =
                   connection.CreateCommand())
            {
                command.Transaction = transaction;
                //command.CommandText =
                //    AccountQueries.InsertFixedDepositAccount;
                command.CommandText = "InsertFixedDepositAccount";
                command.CommandType = CommandType.StoredProcedure;

                AddParameter(
                    command,
                    "@AccountId",
                    accountId);

                AddParameter(
                    command,
                    "@InterestRate",
                    fixedDeposit.InterestRate / 100.0);

                AddParameter(
                    command,
                    "@TenureMonths",
                    fixedDeposit.TenureMonths);

                AddParameter(
                    command,
                    "@PrincipalAmount",
                    fixedDeposit.Balance);

                command.ExecuteNonQuery();
            }
        }


        private void SaveSalaryAccount(
            SalaryAccount salary,
            long accountId,
            DbConnection connection,
            DbTransaction transaction)
        {
            using (DbCommand command =
                   connection.CreateCommand())
            {
                command.Transaction = transaction;
                //command.CommandText =
                //    AccountQueries.InsertSalaryAccount;
                command.CommandText = "InsertSalaryAccount";
                command.CommandType = CommandType.StoredProcedure;

                AddParameter(
                    command,
                    "@AccountId",
                    accountId);

                AddParameter(
                    command,
                    "@EmployerName",
                    salary.EmployerName);

                AddParameter(
                    command,
                    "@InactiveMonths",
                    salary.InactiveMonths);

                AddParameter(
                    command,
                    "@SalaryAmount",
                    salary.Balance);

                command.ExecuteNonQuery();
            }
        }


        public void UpdateBalance(
            string accountNumber,
            decimal balance)
        {
            try
            {
                using DbConnection connection =
                    DataBaseConnectionManager.GetConnection();

                connection.Open();

                using DbCommand command =
                    connection.CreateCommand();

                command.CommandText =
                    AccountQueries.UpdateBalance;

                AddParameter(
                    command,
                    "@Balance",
                    balance);

                AddParameter(
                    command,
                    "@AccountNumber",
                    accountNumber);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    _logger.LogWarning("UpdateBalance affected no rows for account {AccountNumber}", accountNumber);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update balance for account {AccountNumber}", accountNumber);
                throw;
            }
        }


        public void CloseAccount(
            string accountNumber)
        {
            try
            {
                using (DbConnection connection =
                       DataBaseConnectionManager.GetConnection())
                {
                    connection.Open();

                    using (DbCommand command =
                           connection.CreateCommand())
                    {
                        command.CommandText =
                            AccountQueries.CloseAccount;

                        AddParameter(
                            command,
                            "@AccountNumber",
                            accountNumber);

                        int rowsAffected =
                            command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            _logger.LogWarning("CloseAccount: account {AccountNumber} not found", accountNumber);
                            throw new Exception(
                                "Account not found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to close account {AccountNumber}", accountNumber);
                throw;
            }
        }


        public List<IAccount> GetAllAccounts()
        {
            try
            {
                List<IAccount> accounts =
                    new List<IAccount>();

                using (DbConnection connection =
                       DataBaseConnectionManager.GetConnection())
                {
                    connection.Open();

                    using (DbCommand command =
                           connection.CreateCommand())
                    {
                        command.CommandText =
                            AccountQueries.GetAllAccounts;

                        using (DbDataReader reader =
                               command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                accounts.Add(
                                    CreateAccount(reader));
                            }
                        }
                    }
                }

                return accounts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch all accounts");
                throw;
            }
        }


        public void SaveAccounts(
            IAccount fromAccount,
            IAccount toAccount)
        {
            try
            {
                using (DbConnection connection =
                       DataBaseConnectionManager.GetConnection())
                {
                    connection.Open();

                    DbTransaction transaction =
                        connection.BeginTransaction();

                    try
                    {
                        using (DbCommand command =
                               connection.CreateCommand())
                        {
                            command.Transaction =
                                transaction;

                            command.CommandText =
                                AccountQueries.SaveAccounts;

                            AddParameter(
                                command,
                                "@Balance",
                                fromAccount.Balance);

                            AddParameter(
                                command,
                                "@AccountNumber",
                                fromAccount.AccountNumber);

                            command.ExecuteNonQuery();

                            command.Parameters[
                                "@Balance"].Value =
                                toAccount.Balance;

                            command.Parameters[
                                "@AccountNumber"].Value =
                                toAccount.AccountNumber;

                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Rolling back SaveAccounts for {FromAccount} -> {ToAccount}", fromAccount.AccountNumber, toAccount.AccountNumber);
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save accounts {FromAccount} -> {ToAccount}", fromAccount.AccountNumber, toAccount.AccountNumber);
                throw;
            }
        }


        private IAccount CreateAccount(
            DbDataReader reader)
        {
            string accountNumber =
                reader["AccountNumber"].ToString();

            string name =
                reader["Name"].ToString();

            int age =
                Convert.ToInt32(
                    reader["Age"]);

            decimal balance =
                Convert.ToDecimal(
                    reader["Balance"]);

            AccountType accountType =
                ConvertAccountType(
                    reader["AccountType"].ToString());

            AccountStatus status =
                ConvertAccountStatus(
                    reader["AccountStatus"].ToString());

            AccountPrivilege privilege =
                ConvertAccountPrivilege(
                    reader["AccountPrivilege"].ToString());

            string pin =
                reader["Pin"].ToString();

            switch (accountType)
            {
                case AccountType.Savings:
                    return CreateSavingsAccount(
                        reader,
                        accountNumber,
                        name,
                        age,
                        balance,
                        accountType,
                        status,
                        pin,
                        privilege);

                case AccountType.Current:
                    return CreateCurrentAccount(
                        reader,
                        accountNumber,
                        name,
                        age,
                        balance,
                        accountType,
                        status,
                        pin,
                        privilege);

                case AccountType.FixedDeposit:
                    return CreateFixedDepositAccount(
                        reader,
                        accountNumber,
                        name,
                        age,
                        balance,
                        accountType,
                        status,
                        pin,
                        privilege);

                case AccountType.Salary:
                    return CreateSalaryAccount(
                        reader,
                        accountNumber,
                        name,
                        age,
                        balance,
                        accountType,
                        status,
                        pin,
                        privilege);

                default:
                    throw new Exception(
                        "Unknown account type.");
            }
        }


        private IAccount CreateSavingsAccount(
            DbDataReader reader,
            string accountNumber,
            string name,
            int age,
            decimal balance,
            AccountType accountType,
            AccountStatus status,
            string pin,
            AccountPrivilege privilege)
        {
            decimal minBalance =
                reader["SavingsMinimumBalance"] ==
                DBNull.Value
                ? 1000.0m
                : Convert.ToDecimal(
                    reader["SavingsMinimumBalance"]);

            double interestRate =
                reader["SavingsInterestRate"] ==
                DBNull.Value
                ? 4.0
                : Convert.ToDouble(
                    reader["SavingsInterestRate"]) * 100;

            return new SavingsAccount(
                accountNumber,
                name,
                age,
                balance,
                accountType,
                status,
                pin,
                privilege,
                minBalance,
                interestRate);
        }


        private IAccount CreateCurrentAccount(
            DbDataReader reader,
            string accountNumber,
            string name,
            int age,
            decimal balance,
            AccountType accountType,
            AccountStatus status,
            string pin,
            AccountPrivilege privilege)
        {
            decimal overdraftLimit =
                reader["OverdraftLimit"] ==
                DBNull.Value
                ? 25000.0m
                : Convert.ToDecimal(
                    reader["OverdraftLimit"]);

            return new CurrentAccount(
                accountNumber,
                name,
                age,
                balance,
                accountType,
                status,
                pin,
                privilege,
                overdraftLimit);
        }


        private IAccount CreateFixedDepositAccount(
            DbDataReader reader,
            string accountNumber,
            string name,
            int age,
            decimal balance,
            AccountType accountType,
            AccountStatus status,
            string pin,
            AccountPrivilege privilege)
        {
            int tenureMonths =
                reader["TenureMonths"] ==
                DBNull.Value
                ? 12
                : Convert.ToInt32(
                    reader["TenureMonths"]);

            double interestRate =
                reader["FixedDepositInterestRate"] ==
                DBNull.Value
                ? 6.5
                : Convert.ToDouble(
                    reader["FixedDepositInterestRate"]) * 100;

            return new FixedDepositAccount(
                accountNumber,
                name,
                age,
                balance,
                accountType,
                status,
                pin,
                privilege,
                tenureMonths,
                interestRate);
        }


        private IAccount CreateSalaryAccount(
            DbDataReader reader,
            string accountNumber,
            string name,
            int age,
            decimal balance,
            AccountType accountType,
            AccountStatus status,
            string pin,
            AccountPrivilege privilege)
        {
            string employerName =
                reader["EmployerName"] ==
                DBNull.Value
                ? "TechCorp"
                : reader["EmployerName"].ToString();

            return new SalaryAccount(
                accountNumber,
                name,
                age,
                balance,
                accountType,
                status,
                pin,
                privilege,
                employerName);
        }


        private AccountType ConvertAccountType(
            string value)
        {
            switch (value)
            {
                case "SAVINGS":
                    return AccountType.Savings;

                case "CURRENT":
                    return AccountType.Current;

                case "FIXED_DEPOSIT":
                    return AccountType.FixedDeposit;

                case "SALARY":
                    return AccountType.Salary;

                default:
                    _logger.LogWarning("Unknown account type code {AccountTypeCode} read from DB", value);
                    throw new Exception(
                        "Invalid account type: " + value);
            }
        }


        private AccountStatus ConvertAccountStatus(
            string value)
        {
            return (AccountStatus)Enum.Parse(
                typeof(AccountStatus),
                value,
                true);
        }


        private AccountPrivilege ConvertAccountPrivilege(
            string value)
        {
            return (AccountPrivilege)Enum.Parse(
                typeof(AccountPrivilege),
                value,
                true);
        }


        private string GetAccountTypeCode(
            AccountType accountType)
        {
            switch (accountType)
            {
                case AccountType.Savings:
                    return "SAVINGS";

                case AccountType.Current:
                    return "CURRENT";

                case AccountType.FixedDeposit:
                    return "FIXED_DEPOSIT";

                case AccountType.Salary:
                    return "SALARY";

                default:
                    throw new Exception(
                        "Invalid account type.");
            }
        }


        private string GetAccountStatusCode(
            AccountStatus status)
        {
            return status.ToString().ToUpper();
        }


        private string GetAccountPrivilegeCode(
            AccountPrivilege privilege)
        {
            return privilege.ToString().ToUpper();
        }


        private void AddParameter(
            DbCommand command,
            string parameterName,
            object value)
        {
            DbParameter parameter =
                command.CreateParameter();

            parameter.ParameterName =
                parameterName;

            parameter.Value =
                value ?? DBNull.Value;

            command.Parameters.Add(parameter);
        }
    }
}