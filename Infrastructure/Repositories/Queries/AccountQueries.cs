namespace GDB.App.Infrastructure.Repositories.Queries
{
    internal static class AccountQueries
    {
        public const string GetAccount = "GetAccount";
        //public const string GetAccount = @"
        //    SELECT
        //        a.AccountId,
        //        a.AccountNumber,
        //        a.Name,
        //        a.Age,
        //        a.Balance,
        //        a.Pin,

        //        at.Code AS AccountType,
        //        ast.Code AS AccountStatus,
        //        ap.Code AS AccountPrivilege,

        //        sa.InterestRate AS SavingsInterestRate,
        //        sa.MinimumBalance AS SavingsMinimumBalance,

        //        ca.OverdraftLimit,

        //        fda.InterestRate AS FixedDepositInterestRate,
        //        fda.TenureMonths,

        //        sya.EmployerName,
        //        sya.InactiveMonths

        //    FROM Accounts a

        //    INNER JOIN AccountTypes at
        //        ON a.AccountTypeId = at.AccountTypeId

        //    INNER JOIN AccountStatuses ast
        //        ON a.AccountStatusId = ast.AccountStatusId

        //    INNER JOIN AccountPrivileges ap
        //        ON a.AccountPrivilegeId = ap.AccountPrivilegeId

        //    LEFT JOIN SavingsAccounts sa
        //        ON a.AccountId = sa.AccountId

        //    LEFT JOIN CurrentAccounts ca
        //        ON a.AccountId = ca.AccountId

        //    LEFT JOIN FixedDepositAccounts fda
        //        ON a.AccountId = fda.AccountId

        //    LEFT JOIN SalaryAccounts sya
        //        ON a.AccountId = sya.AccountId

        //    WHERE a.AccountNumber = @AccountNumber";


        public const string GetAllAccounts = @"
            SELECT
                a.AccountId,
                a.AccountNumber,
                a.Name,
                a.Age,
                a.Balance,
                a.Pin,

                at.Code AS AccountType,
                ast.Code AS AccountStatus,
                ap.Code AS AccountPrivilege,

                sa.InterestRate AS SavingsInterestRate,
                sa.MinimumBalance AS SavingsMinimumBalance,

                ca.OverdraftLimit,

                fda.InterestRate AS FixedDepositInterestRate,
                fda.TenureMonths,

                sya.EmployerName,
                sya.InactiveMonths

            FROM Accounts a

            INNER JOIN AccountTypes at
                ON a.AccountTypeId = at.AccountTypeId

            INNER JOIN AccountStatuses ast
                ON a.AccountStatusId = ast.AccountStatusId

            INNER JOIN AccountPrivileges ap
                ON a.AccountPrivilegeId = ap.AccountPrivilegeId

            LEFT JOIN SavingsAccounts sa
                ON a.AccountId = sa.AccountId

            LEFT JOIN CurrentAccounts ca
                ON a.AccountId = ca.AccountId

            LEFT JOIN FixedDepositAccounts fda
                ON a.AccountId = fda.AccountId

            LEFT JOIN SalaryAccounts sya
                ON a.AccountId = sya.AccountId

            ORDER BY a.AccountId";
        public const string CreateAccount = "CreateAccount";
        public const string InsertCurrentAccount =
    "InsertCurrentAccount";
        public const string InsertSavingsAccount =
     "InsertSavingsAccount";
        public const string InsertFixedDepositAccount =
    "InsertFixedDepositAccount";
        public const string InsertSalaryAccount =
    "InsertSalaryAccount";

        //public const string InsertAccount = @"
        //    INSERT INTO Accounts
        //    (
        //        AccountNumber,
        //        Name,
        //        Age,
        //        AccountTypeId,
        //        Balance,
        //        AccountStatusId,
        //        AccountPrivilegeId,
        //        Pin
        //    )
        //    VALUES
        //    (
        //        @AccountNumber,
        //        @Name,
        //        @Age,
        //        (
        //            SELECT AccountTypeId
        //            FROM AccountTypes
        //            WHERE Code = @AccountType
        //        ),
        //        @Balance,
        //        (
        //            SELECT AccountStatusId
        //            FROM AccountStatuses
        //            WHERE Code = @AccountStatus
        //        ),
        //        (
        //            SELECT AccountPrivilegeId
        //            FROM AccountPrivileges
        //            WHERE Code = @AccountPrivilege
        //        ),
        //        @Pin
        //    );

        //    SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";


        //public const string InsertSavingsAccount = @"
        //    INSERT INTO SavingsAccounts
        //    (
        //        AccountId,
        //        InterestRate,
        //        MinimumBalance
        //    )
        //    VALUES
        //    (
        //        @AccountId,
        //        @InterestRate,
        //        @MinimumBalance
        //    )";


        //public const string InsertCurrentAccount = @"
        //    INSERT INTO CurrentAccounts
        //    (
        //        AccountId,
        //        OverdraftLimit
        //    )
        //    VALUES
        //    (
        //        @AccountId,
        //        @OverdraftLimit
        //    )";


        //public const string InsertFixedDepositAccount = @"
        //    INSERT INTO FixedDepositAccounts
        //    (
        //        AccountId,
        //        InterestRate,
        //        TenureMonths,
        //        PrincipalAmount,
        //        MaturityDate,
        //        MaturityAmount
        //    )
        //    VALUES
        //    (
        //        @AccountId,
        //        @InterestRate,
        //        @TenureMonths,
        //        @PrincipalAmount,
        //        DATEADD(
        //            MONTH,
        //            @TenureMonths,
        //            SYSUTCDATETIME()
        //        ),
        //        @PrincipalAmount
        //    )";


        //public const string InsertSalaryAccount = @"
        //    INSERT INTO SalaryAccounts
        //    (
        //        AccountId,
        //        EmployerName,
        //        InactiveMonths,
        //        SalaryAmount
        //    )
        //    VALUES
        //    (
        //        @AccountId,
        //        @EmployerName,
        //        @InactiveMonths,
        //        @SalaryAmount
        //    )";


        public const string UpdateBalance = @"
            UPDATE Accounts
            SET Balance = @Balance
            WHERE AccountNumber = @AccountNumber";


        public const string CloseAccount = @"
            UPDATE Accounts
            SET
                AccountStatusId =
                (
                    SELECT AccountStatusId
                    FROM AccountStatuses
                    WHERE Code = 'CLOSED'
                ),
                ClosedAt = SYSUTCDATETIME()
            WHERE AccountNumber = @AccountNumber";


        public const string SaveAccounts = @"
            UPDATE Accounts
            SET Balance = @Balance
            WHERE AccountNumber = @AccountNumber";
    }
}
