using System;
using System.Data;

namespace GDB.App.Data
{
    /// <summary>
    /// Factory for creating the in-memory GDB DataSet.
    /// The schema matches the current Domain Account classes.
    /// </summary>
    public static class GDBInMemoryDB
    {

        public static DataSet CreateDataSet()
        {
            var ds = new DataSet("GDBDataSet");

            // Create tables
            CreateAccountTable(ds);
            CreateSavingsTable(ds);
            CreateCurrentTable(ds);
            CreateFixedDepositTable(ds);
            CreateSalaryTable(ds);
            CreateTransactionTable(ds);

            // Create relationships
            CreateRelations(ds);

            // Populate sample data
            PopulateSampleData(ds);

            return ds;
        }
        // =========================================================
        // TRANSACTION
        // =========================================================

        private static void CreateTransactionTable(DataSet ds)
        {
            var dt = new DataTable("TRANSACTION");

            var dcTransactionId =
                new DataColumn("TransactionId", typeof(int))
                {
                    AllowDBNull = false
                };

            var dcFromAccountNumber =
                new DataColumn("FromAccountNumber", typeof(string))
                {
                    AllowDBNull = true
                };

            var dcToAccountNumber =
                new DataColumn("ToAccountNumber", typeof(string))
                {
                    AllowDBNull = true
                };

            var dcAmount =
                new DataColumn("Amount", typeof(decimal))
                {
                    AllowDBNull = false
                };

            var dcTransactionType =
                new DataColumn("TransactionType", typeof(string))
                {
                    AllowDBNull = false
                };

            var dcTransactionStatus =
                new DataColumn("TransactionStatus", typeof(string))
                {
                    AllowDBNull = false
                };

            var dcTimestamp =
                new DataColumn("Timestamp", typeof(DateTime))
                {
                    AllowDBNull = false
                };

            var dcBalanceAfterFrom =
                new DataColumn("BalanceAfterFrom", typeof(decimal))
                {
                    AllowDBNull = true
                };

            var dcBalanceAfterTo =
                new DataColumn("BalanceAfterTo", typeof(decimal))
                {
                    AllowDBNull = true
                };

            dt.Columns.Add(dcTransactionId);
            dt.Columns.Add(dcFromAccountNumber);
            dt.Columns.Add(dcToAccountNumber);
            dt.Columns.Add(dcAmount);
            dt.Columns.Add(dcTransactionType);
            dt.Columns.Add(dcTransactionStatus);
            dt.Columns.Add(dcTimestamp);
            dt.Columns.Add(dcBalanceAfterFrom);
            dt.Columns.Add(dcBalanceAfterTo);

            dt.PrimaryKey = new[] { dcTransactionId };

            ds.Tables.Add(dt);
        }

        // =========================================================
        // ACCOUNT
        // =========================================================

        private static void CreateAccountTable(DataSet ds)
        {
            var dt = new DataTable("ACCOUNT");

            var dcAccountNumber =
                new DataColumn("AccountNumber", typeof(string))
                {
                    AllowDBNull = false,
                    MaxLength = 32
                };

            var dcName =
                new DataColumn("Name", typeof(string))
                {
                    AllowDBNull = false,
                    MaxLength = 100
                };

            var dcAge =
                new DataColumn("Age", typeof(int))
                {
                    AllowDBNull = false
                };

            var dcBalance =
                new DataColumn("Balance", typeof(decimal))
                {
                    AllowDBNull = false,
                    DefaultValue = 0.00m
                };

            var dcAccountType =
                new DataColumn("AccountType", typeof(string))
                {
                    AllowDBNull = false,
                    MaxLength = 32
                };

            var dcStatus =
                new DataColumn("Status", typeof(string))
                {
                    AllowDBNull = false,
                    MaxLength = 32
                };

            var dcPin =
                new DataColumn("Pin", typeof(string))
                {
                    AllowDBNull = false,
                    MaxLength = 4
                };

            var dcPrivilage =
                new DataColumn("Privilage", typeof(string))
                {
                    AllowDBNull = false,
                    MaxLength = 32
                };


            dt.Columns.Add(dcAccountNumber);
            dt.Columns.Add(dcName);
            dt.Columns.Add(dcAge);
            dt.Columns.Add(dcBalance);
            dt.Columns.Add(dcAccountType);
            dt.Columns.Add(dcStatus);
            dt.Columns.Add(dcPin);
            dt.Columns.Add(dcPrivilage);

            // AccountNumber acts as the primary key
            dt.PrimaryKey = new[] { dcAccountNumber };

            ds.Tables.Add(dt);
        }


        // =========================================================
        // SAVINGS ACCOUNT
        // =========================================================

        private static void CreateSavingsTable(DataSet ds)
        {
            var dt = new DataTable("SAVINGS_ACCOUNT");

            var dcAccountNumber =
                new DataColumn("AccountNumber", typeof(string))
                {
                    AllowDBNull = false
                };

            var dcMinBalance =
                new DataColumn("MinBalance", typeof(decimal))
                {
                    AllowDBNull = false,
                    DefaultValue = 1000.00m
                };

            var dcInterestRate =
                new DataColumn("InterestRate", typeof(double))
                {
                    AllowDBNull = false,
                    DefaultValue = 4.0
                };


            dt.Columns.Add(dcAccountNumber);
            dt.Columns.Add(dcMinBalance);
            dt.Columns.Add(dcInterestRate);

            dt.PrimaryKey = new[] { dcAccountNumber };

            ds.Tables.Add(dt);
        }


        // =========================================================
        // CURRENT ACCOUNT
        // =========================================================

        private static void CreateCurrentTable(DataSet ds)
        {
            var dt = new DataTable("CURRENT_ACCOUNT");

            var dcAccountNumber =
                new DataColumn("AccountNumber", typeof(string))
                {
                    AllowDBNull = false
                };

            var dcOverdraftLimit =
                new DataColumn("OverdraftLimit", typeof(decimal))
                {
                    AllowDBNull = false,
                    DefaultValue = 25000.00m
                };


            dt.Columns.Add(dcAccountNumber);
            dt.Columns.Add(dcOverdraftLimit);

            dt.PrimaryKey = new[] { dcAccountNumber };

            ds.Tables.Add(dt);
        }


        // =========================================================
        // FIXED DEPOSIT ACCOUNT
        // =========================================================

        private static void CreateFixedDepositTable(DataSet ds)
        {
            var dt = new DataTable("FIXED_DEPOSIT_ACCOUNT");

            var dcAccountNumber =
                new DataColumn("AccountNumber", typeof(string))
                {
                    AllowDBNull = false
                };

            var dcTenureMonths =
                new DataColumn("TenureMonths", typeof(int))
                {
                    AllowDBNull = false,
                    DefaultValue = 12
                };

            var dcInterestRate =
                new DataColumn("InterestRate", typeof(double))
                {
                    AllowDBNull = false,
                    DefaultValue = 6.5
                };


            dt.Columns.Add(dcAccountNumber);
            dt.Columns.Add(dcTenureMonths);
            dt.Columns.Add(dcInterestRate);

            dt.PrimaryKey = new[] { dcAccountNumber };

            ds.Tables.Add(dt);
        }


        // =========================================================
        // SALARY ACCOUNT
        // =========================================================

        private static void CreateSalaryTable(DataSet ds)
        {
            var dt = new DataTable("SALARY_ACCOUNT");

            var dcAccountNumber =
                new DataColumn("AccountNumber", typeof(string))
                {
                    AllowDBNull = false
                };

            var dcEmployerName =
                new DataColumn("EmployerName", typeof(string))
                {
                    AllowDBNull = false,
                    MaxLength = 100
                };

            var dcInactiveMonths =
                new DataColumn("InactiveMonths", typeof(int))
                {
                    AllowDBNull = false,
                    DefaultValue = 0
                };


            dt.Columns.Add(dcAccountNumber);
            dt.Columns.Add(dcEmployerName);
            dt.Columns.Add(dcInactiveMonths);

            dt.PrimaryKey = new[] { dcAccountNumber };

            ds.Tables.Add(dt);
        }


        // =========================================================
        // RELATIONSHIPS
        // =========================================================

        private static void CreateRelations(DataSet ds)
        {
            AddRelation(
                ds,
                "AccountSavings",
                "ACCOUNT",
                "AccountNumber",
                "SAVINGS_ACCOUNT",
                "AccountNumber");

            AddRelation(
                ds,
                "AccountCurrent",
                "ACCOUNT",
                "AccountNumber",
                "CURRENT_ACCOUNT",
                "AccountNumber");

            AddRelation(
                ds,
                "AccountFixedDeposit",
                "ACCOUNT",
                "AccountNumber",
                "FIXED_DEPOSIT_ACCOUNT",
                "AccountNumber");

            AddRelation(
                ds,
                "AccountSalary",
                "ACCOUNT",
                "AccountNumber",
                "SALARY_ACCOUNT",
                "AccountNumber");
        }


        private static void AddRelation(
            DataSet ds,
            string relationName,
            string parentTable,
            string parentColumnName,
            string childTable,
            string childColumnName)
        {
            var parent =
                ds.Tables[parentTable].Columns[parentColumnName];

            var child =
                ds.Tables[childTable].Columns[childColumnName];

            var relation =
                new DataRelation(
                    relationName,
                    parent,
                    child,
                    createConstraints: true);

            ds.Relations.Add(relation);

            relation.ChildKeyConstraint.DeleteRule = Rule.None;
            relation.ChildKeyConstraint.UpdateRule = Rule.None;
        }


        // =========================================================
        // SAMPLE DATA
        // =========================================================

        private static void PopulateSampleData(DataSet ds)
        {
            var accounts = ds.Tables["ACCOUNT"];
            var savings = ds.Tables["SAVINGS_ACCOUNT"];
            var current = ds.Tables["CURRENT_ACCOUNT"];
            var fd = ds.Tables["FIXED_DEPOSIT_ACCOUNT"];
            var salary = ds.Tables["SALARY_ACCOUNT"];


            // -----------------------------------------------------
            // ACCOUNT
            // -----------------------------------------------------

            AddAccount(
                accounts,
                "1000001001",
                "Rahul Sharma",
                25,
                25000.75m,
                "Savings",
                "Active",
                "1234",
                "Silver");

            AddAccount(
                accounts,
                "1000001002",
                "Priya Kumar",
                30,
                150000.00m,
                "Savings",
                "Active",
                "2345",
                "Gold");

            AddAccount(
                accounts,
                "1000001003",
                "Arun Kumar",
                28,
                5000.00m,
                "Savings",
                "Active",
                "3456",
                "Silver");

            AddAccount(
                accounts,
                "1000001004",
                "Sneha Rao",
                32,
                30000.00m,
                "Savings",
                "Active",
                "4567",
                "Silver");


            // Current accounts
            AddAccount(
                accounts,
                "1000001005",
                "Vikram Singh",
                40,
                125000.00m,
                "Current",
                "Active",
                "5678",
                "Gold");

            AddAccount(
                accounts,
                "1000001006",
                "Anita Patel",
                35,
                50000.00m,
                "Current",
                "Active",
                "6789",
                "Gold");

            AddAccount(
                accounts,
                "1000001007",
                "Karan Mehta",
                42,
                75000.00m,
                "Current",
                "Active",
                "7890",
                "Gold");


            // Fixed Deposit accounts
            AddAccount(
                accounts,
                "1000001008",
                "Ravi Verma",
                45,
                100000.00m,
                "FixedDeposit",
                "Active",
                "8901",
                "Premium");

            AddAccount(
                accounts,
                "1000001009",
                "Meena Rao",
                50,
                200000.00m,
                "FixedDeposit",
                "Active",
                "9012",
                "Premium");


            // Salary accounts
            AddAccount(
                accounts,
                "1000001010",
                "Suresh Kumar",
                29,
                35000.00m,
                "Salary",
                "Active",
                "1122",
                "Premium");

            AddAccount(
                accounts,
                "1000001011",
                "Divya Sharma",
                31,
                48000.00m,
                "Salary",
                "Active",
                "2233",
                "Premium");


            // Another Savings account
            AddAccount(
                accounts,
                "1000001012",
                "Neha Kapoor",
                27,
                12000.00m,
                "Savings",
                "Active",
                "3344",
                "Premium");


            // -----------------------------------------------------
            // SAVINGS_ACCOUNT
            // -----------------------------------------------------

            savings.Rows.Add(
                "1000001001",
                500.00m,
                3.5);

            savings.Rows.Add(
                "1000001002",
                1000.00m,
                4.0);

            savings.Rows.Add(
                "1000001003",
                200.00m,
                3.0);

            savings.Rows.Add(
                "1000001004",
                500.00m,
                3.75);

            savings.Rows.Add(
                "1000001012",
                500.00m,
                3.3);


            // -----------------------------------------------------
            // CURRENT_ACCOUNT
            // -----------------------------------------------------

            current.Rows.Add(
                "1000001005",
                200000.00m);

            current.Rows.Add(
                "1000001006",
                50000.00m);

            current.Rows.Add(
                "1000001007",
                75000.00m);


            // -----------------------------------------------------
            // FIXED_DEPOSIT_ACCOUNT
            // -----------------------------------------------------

            fd.Rows.Add(
                "1000001008",
                12,
                6.5);

            fd.Rows.Add(
                "1000001009",
                36,
                6.5);


            // -----------------------------------------------------
            // SALARY_ACCOUNT
            // -----------------------------------------------------

            salary.Rows.Add(
                "1000001010",
                "TechCorp",
                0);

            salary.Rows.Add(
                "1000001011",
                "TechCorp",
                0);
        }


        // =========================================================
        // HELPER
        // =========================================================

        private static void AddAccount(
            DataTable accounts,
            string accountNumber,
            string name,
            int age,
            decimal balance,
            string accountType,
            string status,
            string pin,
            string privilage)
        {
            var row = accounts.NewRow();

            row["AccountNumber"] = accountNumber;
            row["Name"] = name;
            row["Age"] = age;
            row["Balance"] = balance;
            row["AccountType"] = accountType;
            row["Status"] = status;
            row["Pin"] = pin;
            row["Privilage"] = privilage;

            accounts.Rows.Add(row);
        }
    }
}