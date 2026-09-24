using GDB.App.Application.Dtos;
using GDB.App.Data;
using GDB.App.Domain.Enums;
using GDB.App.Infrastructure.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GDB.App.Infrastructure.Repositories;

namespace GDB.App.Infrastructure.Repositories.Implementations
{
    public class TransactionRepositoryInMemory : ITransactionRepository
    {
        private readonly DataSet _dataSet;

        public TransactionRepositoryInMemory()
        {
            _dataSet =GDBInMemoryDataStore.DataSet;
        }


        // =========================================================
        // GET RECENT TRANSACTIONS
        // =========================================================

        public List<ViewRecentTransactionsResponseDto>
            GetRecentTransactions(string accountNumber)
        {
            List<ViewRecentTransactionsResponseDto> transactions =
                new List<ViewRecentTransactionsResponseDto>();

            DataTable transactionTable =
                _dataSet.Tables["TRANSACTION"];


            var rows = transactionTable.AsEnumerable()
                .Where(row =>
                    (
                        !row.IsNull("FromAccountNumber")
                        &&
                        row.Field<string>(
                            "FromAccountNumber")
                            == accountNumber
                    )
                    ||
                    (
                        !row.IsNull("ToAccountNumber")
                        &&
                        row.Field<string>(
                            "ToAccountNumber")
                            == accountNumber
                    )
                )
                .OrderByDescending(row =>
                    row.Field<DateTime>("Timestamp"))
                .Take(10);


            foreach (DataRow row in rows)
            {
                ViewRecentTransactionsResponseDto transaction =
                    new ViewRecentTransactionsResponseDto();

                transaction.TransactionId =
                    row.Field<int>("TransactionId");

                transaction.FromAccountNumber =
                    row.IsNull("FromAccountNumber")
                        ? null
                        : row.Field<string>(
                            "FromAccountNumber");

                transaction.ToAccountNumber =
                    row.IsNull("ToAccountNumber")
                        ? null
                        : row.Field<string>(
                            "ToAccountNumber");

                transaction.Amount =
                    row.Field<decimal>("Amount");

                transaction.TransactionType =
                    Enum.Parse<TransactionType>(
                        row.Field<string>(
                            "TransactionType"));

                transaction.TransactionStatus =
                    Enum.Parse<TransactionStatus>(
                        row.Field<string>(
                            "TransactionStatus"));

                transaction.Timestamp =
                    row.Field<DateTime>("Timestamp");

                transaction.BalanceAfterFrom =
                    row.IsNull("BalanceAfterFrom")
                        ? (decimal?)null
                        : row.Field<decimal>(
                            "BalanceAfterFrom");

                transaction.BalanceAfterTo =
                    row.IsNull("BalanceAfterTo")
                        ? (decimal?)null
                        : row.Field<decimal>(
                            "BalanceAfterTo");

                transactions.Add(transaction);
            }

            return transactions;
        }


        // =========================================================
        // SAVE TRANSACTION
        // =========================================================

        public void SaveTransaction(
            string fromAccountNumber,
            string toAccountNumber,
            TransactionType transactionType,
            decimal amount,
            TransactionStatus transactionStatus,
            decimal balanceAfterFrom,
            decimal balanceAfterTo)
        {
            DataTable transactionTable =
                _dataSet.Tables["TRANSACTION"];


            int transactionId = 1;

            if (transactionTable.Rows.Count > 0)
            {
                transactionId =
                    transactionTable.AsEnumerable()
                        .Max(row =>
                            row.Field<int>(
                                "TransactionId")) + 1;
            }


            DataRow row =
                transactionTable.NewRow();


            row["TransactionId"] =
                transactionId;


            row["FromAccountNumber"] =
                string.IsNullOrEmpty(fromAccountNumber)
                    ? DBNull.Value
                    : fromAccountNumber;


            row["ToAccountNumber"] =
                string.IsNullOrEmpty(toAccountNumber)
                    ? DBNull.Value
                    : toAccountNumber;


            row["Amount"] =
                amount;


            row["TransactionType"] =
                transactionType.ToString();


            row["TransactionStatus"] =
                transactionStatus.ToString();


            row["Timestamp"] =
                DateTime.Now;


            row["BalanceAfterFrom"] =
                balanceAfterFrom;


            row["BalanceAfterTo"] =
                balanceAfterTo;


            transactionTable.Rows.Add(row);
        }
    }
}