using System;
using System.Collections.Generic;
using System.Data;
using gdb.Logging;
using Microsoft.Extensions.Logging;
using GDB.App.Application.Dtos;
using GDB.App.Domain.Enums;
using GDB.App.Infrastructure.Repositories.Contracts;
using GDB.App.Infrastructure.Repositories.Queries;
using System.Data.Common;

namespace GDB.App.Infrastructure.Repositories.Implementations
{
    public class TransactionRepositoryDB : ITransactionRepository
    {
        public List<ViewRecentTransactionsResponseDto> GetRecentTransactions(
            string accountNumber)
        {
            List<ViewRecentTransactionsResponseDto> transactions =
                new List<ViewRecentTransactionsResponseDto>();

            using (DbConnection connection =
                   DataBaseConnectionManager.GetConnection())
            {
                connection.Open();

                using (DbCommand command =
                       connection.CreateCommand())
                {
                    command.CommandText =
                        TransactionQueries.GetRecentTransactions;

                    AddParameter(
                        command,
                        "@AccountNumber",
                        accountNumber);

                    using (DbDataReader reader =
                           command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ViewRecentTransactionsResponseDto transaction =
                                new ViewRecentTransactionsResponseDto();

                            transaction.TransactionId =
                                Convert.ToInt32(
                                    reader["TransactionId"]);

                            transaction.FromAccountNumber =
                                reader["FromAccountNumber"] == DBNull.Value
                                    ? null
                                    : reader["FromAccountNumber"].ToString();

                            transaction.ToAccountNumber =
                                reader["ToAccountNumber"] == DBNull.Value
                                    ? null
                                    : reader["ToAccountNumber"].ToString();

                            transaction.Amount =
                                Convert.ToDecimal(
                                    reader["Amount"]);

                            transaction.TransactionType =
                                (TransactionType)Enum.Parse(
                                    typeof(TransactionType),
                                    reader["TransactionType"].ToString(),
                                    true);

                            transaction.TransactionStatus =
                                (TransactionStatus)Enum.Parse(
                                    typeof(TransactionStatus),
                                    reader["TransactionStatus"].ToString(),
                                    true);

                            transaction.Timestamp =
                                Convert.ToDateTime(
                                    reader["Timestamp"]);

                            transaction.BalanceAfterFrom =
                                reader["BalanceAfterFrom"] == DBNull.Value
                                    ? null
                                    : Convert.ToDecimal(
                                        reader["BalanceAfterFrom"]);

                            transaction.BalanceAfterTo =
                                reader["BalanceAfterTo"] == DBNull.Value
                                    ? null
                                    : Convert.ToDecimal(
                                        reader["BalanceAfterTo"]);

                            transactions.Add(transaction);
                        }
                    }
                }
            }

            return transactions;
        }


        public void SaveTransaction(
            string fromAccountNumber,
            string toAccountNumber,
            TransactionType transactionType,
            decimal amount,
            TransactionStatus transactionStatus,
            decimal balanceAfterFrom,
            decimal balanceAfterTo)
        {
            using (DbConnection connection =
                   DataBaseConnectionManager.GetConnection())
            {
                connection.Open();

                using (DbCommand command =
                       connection.CreateCommand())
                {
                    command.CommandText =
                        TransactionQueries.InsertTransaction;

                    AddParameter(
                        command,
                        "@TransactionType",
                        transactionType.ToString().ToUpper());

                    AddParameter(
                        command,
                        "@FromAccountNumber",
                        string.IsNullOrEmpty(fromAccountNumber)
                            ? (object)DBNull.Value
                            : fromAccountNumber);

                    AddParameter(
                        command,
                        "@ToAccountNumber",
                        string.IsNullOrEmpty(toAccountNumber)
                            ? (object)DBNull.Value
                            : toAccountNumber);

                    AddParameter(
                        command,
                        "@Amount",
                        amount);

                    AddParameter(
                        command,
                        "@TransactionStatus",
                        transactionStatus.ToString().ToUpper());

                    AddParameter(
                        command,
                        "@BalanceAfterFrom",
                        balanceAfterFrom);

                    AddParameter(
                        command,
                        "@BalanceAfterTo",
                        balanceAfterTo);

                    command.ExecuteNonQuery();
                }
            }
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