using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDB.App.Infrastructure.Repositories.Queries
{
    internal static class TransactionQueries
    {
        public const string GetRecentTransactions = @"
            SELECT TOP 10

                t.TransactionId,

                fromAccount.AccountNumber AS FromAccountNumber,

                toAccount.AccountNumber AS ToAccountNumber,

                t.Amount,

                tt.Code AS TransactionType,

                ts.Code AS TransactionStatus,

                t.Timestamp,

                t.BalanceAfterFrom,

                t.BalanceAfterTo

            FROM Transactions t

            LEFT JOIN Accounts fromAccount
                ON t.FromAccountId = fromAccount.AccountId

            LEFT JOIN Accounts toAccount
                ON t.ToAccountId = toAccount.AccountId

            INNER JOIN TransactionTypes tt
                ON t.TransactionTypeId = tt.TransactionTypeId

            INNER JOIN TransactionStatuses ts
                ON t.TransactionStatusId = ts.TransactionStatusId

            WHERE
                t.FromAccountId =
                (
                    SELECT AccountId
                    FROM Accounts
                    WHERE AccountNumber = @AccountNumber
                )

                OR

                t.ToAccountId =
                (
                    SELECT AccountId
                    FROM Accounts
                    WHERE AccountNumber = @AccountNumber
                )

            ORDER BY t.Timestamp DESC";


        public const string InsertTransaction = @"
            INSERT INTO Transactions
            (
                TransactionTypeId,
                FromAccountId,
                ToAccountId,
                Amount,
                TransactionStatusId,
                Timestamp,
                BalanceAfterFrom,
                BalanceAfterTo
            )
            VALUES
            (
                (
                    SELECT TransactionTypeId
                    FROM TransactionTypes
                    WHERE Code = @TransactionType
                ),

                (
                    SELECT AccountId
                    FROM Accounts
                    WHERE AccountNumber = @FromAccountNumber
                ),

                (
                    SELECT AccountId
                    FROM Accounts
                    WHERE AccountNumber = @ToAccountNumber
                ),

                @Amount,

                (
                    SELECT TransactionStatusId
                    FROM TransactionStatuses
                    WHERE Code = @TransactionStatus
                ),

                GETDATE(),

                @BalanceAfterFrom,
                @BalanceAfterTo
            )";
    
    }
}
