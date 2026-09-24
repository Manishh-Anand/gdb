using GDB.App.Application.Dtos;
using GDB.App.Application.Services.Implementations;
using GDB.App.Domain;
using GDB.App.Domain.Enums;
using GDB.App.Domain.Exceptions;
using GDB.App.Domain.Models;
using GDB.App.Infrastructure.Repositories.Contracts;

namespace GDB.App.Test
{
    [TestClass]
    public sealed class AccountAndTransactionTests
    {
        private IAccount _savings;
        private IAccount _current;
        private IAccount _fixed;
        private IAccount _salary;

        [TestInitialize]
        public void Setup()
        {
            _savings = AccountFactory.CreateAccount(
                AccountType.Savings,
                "SAV001",
                "Saving User",
                30,
                5000.00m,
                AccountStatus.Active,
                "1111",
                AccountPrivilege.Silver
            );

            _current = AccountFactory.CreateAccount(
                AccountType.Current,
                "CUR001",
                "Current User",
                40,
                1000.00m,
                AccountStatus.Active,
                "2222",
                AccountPrivilege.Gold,
                overdraftLimit: 500.00m
            );

            _fixed = AccountFactory.CreateAccount(
                AccountType.FixedDeposit,
                "FD001",
                "Fixed User",
                45,
                10000.00m,
                AccountStatus.Active,
                "3333",
                AccountPrivilege.Premium
            );

            _salary = AccountFactory.CreateAccount(
                AccountType.Salary,
                "SAL001",
                "Salary User",
                28,
                2000.00m,
                AccountStatus.Active,
                "4444",
                AccountPrivilege.Premium
            );
        }

        // -------------------- Account domain tests --------------------

        [TestMethod]
        public void Withdraw_Savings_PositiveAmount_DecreasesBalance()
        {
            decimal amount = 500m;
            decimal initial = _savings.Balance;

            _savings.Withdraw(amount, "1111");

            Assert.AreEqual(initial - amount, _savings.Balance);
        }

        [TestMethod]
        public void Withdraw_Savings_BelowMinBalance_ThrowsMinimumBalanceViolationException()
        {
            // savings min default is 1000, balance 5000, withdrawing 4501 should breach
            decimal amount = 4501m;

            Assert.ThrowsException<MinimumBalanceViolationException>(() => _savings.Withdraw(amount, "1111"));
        }

        [TestMethod]
        public void Withdraw_Current_AllowsOverdraftWithinLimit()
        {
            // balance 1000, overdraft 500 -> allowed to withdraw up to 1500
            decimal amount = 1400m;
            _current.Withdraw(amount, "2222");
            Assert.AreEqual(1000m - amount, _current.Balance);
        }

        [TestMethod]
        public void Withdraw_Current_ExceedsOverdraft_ThrowsInsufficientBalanceException()
        {
            decimal amount = 2000m; // exceeds 1000 + 500
            Assert.ThrowsException<InsufficientBalanceException>(() => _current.Withdraw(amount, "2222"));
        }

        [TestMethod]
        public void Withdraw_FixedDeposit_AlwaysThrowsAccountException()
        {
            Assert.ThrowsException<AccountException>(() => _fixed.Withdraw(100m, "3333"));
        }

        [TestMethod]
        public void Withdraw_Salary_InsufficientFunds_ThrowsInsufficientBalanceException()
        {
            decimal amount = 3000m; // greater than salary balance
            Assert.ThrowsException<InsufficientBalanceException>(() => _salary.Withdraw(amount, "4444"));
        }

        [TestMethod]
        public void Withdraw_InvalidPin_ThrowsInvalidPinException()
        {
            Assert.ThrowsException<InvalidPinException>(() => _savings.Withdraw(100m, "9999"));
        }

        [TestMethod]
        public void Withdraw_NegativeAmount_ThrowsInvalidAmountException()
        {
            Assert.ThrowsException<InvalidAmountException>(() => _savings.Withdraw(-10m, "1111"));
        }

        [TestMethod]
        public void ChangePin_ValidOldPin_ReturnsTrueAndUpdatesPin()
        {
            bool result = _savings.ChangePin("1111", "0000");
            Assert.IsTrue(result);
            Assert.IsTrue(_savings.ValidatePin("0000"));
        }

        [TestMethod]
        public void ChangePin_InvalidOldPin_ReturnsFalse()
        {
            bool result = _savings.ChangePin("9999", "0000");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidatePin_CorrectAndIncorrectBehaviors()
        {
            Assert.IsTrue(_savings.ValidatePin("1111"));
            Assert.IsFalse(_savings.ValidatePin("1234"));
        }

        [TestMethod]
        public void CheckIfAccountIsActive_ReturnsFalseForInactive()
        {
            var acc = AccountFactory.CreateAccount(AccountType.Savings, "S2", "U", 20, 100m, AccountStatus.Inactive, "1212", AccountPrivilege.Silver);
            Assert.IsFalse(acc.CheckIfAccountIsActive());
        }

        //  Transaction command tests 

        private class SimpleAccountRepository : IAccountRepository
        {
            private readonly Dictionary<string, IAccount> _store = new();

            public SimpleAccountRepository(IEnumerable<IAccount> accounts)
            {
                foreach (var a in accounts)
                    _store[a.AccountNumber] = a;
            }

            public Task<IAccount> GetAccountAsync(string accountNumber)
            {
                _store.TryGetValue(accountNumber, out var acc);
                return Task.FromResult(acc);
            }

            public void CloseAccount(string accountNumber) => throw new NotImplementedException();
            public void SaveAccount(IAccount account, string pin) => throw new NotImplementedException();
            public List<IAccount> GetAllAccounts() => new List<IAccount>(_store.Values);
            public void SaveAccounts(IAccount fromAccount, IAccount toAccount)
            {
                if (!_store.ContainsKey(fromAccount.AccountNumber) || !_store.ContainsKey(toAccount.AccountNumber))
                    throw new Exception("Account not found");

                _store[fromAccount.AccountNumber] = fromAccount;
                _store[toAccount.AccountNumber] = toAccount;
            }

            public void UpdateBalance(string accountNumber, decimal balance)
            {
                if (_store.TryGetValue(accountNumber, out var acc))
                {
                    // create a new account object not needed; these are reference objects so update would already reflect.
                }
                else throw new Exception("Account not found");
            }
        }

        private class SimpleTransactionRepository : GDB.App.Infrastructure.Repositories.Contracts.ITransactionRepository
        {
            private readonly List<ViewRecentTransactionsResponseDto> _list = new();
            public List<ViewRecentTransactionsResponseDto> GetRecentTransactions(string accountNumber) => _list;
            public void SaveTransaction(string fromAccountNumber, string toAccountNumber, Domain.Enums.TransactionType transactionType, decimal amount, Domain.Enums.TransactionStatus transactionStatus, decimal balanceAfterFrom, decimal balanceAfterTo)
            {
                // no-op for tests
            }
        }

        [TestMethod]
        public void Transfer_Success_UpdatesBalances()
        {
            // arrange
            var from = AccountFactory.CreateAccount(AccountType.Savings, "T_FROM", "From", 30, 5000m, AccountStatus.Active, "fpin", AccountPrivilege.Silver);
            var to = AccountFactory.CreateAccount(AccountType.Savings, "T_TO", "To", 25, 1000m, AccountStatus.Active, "tpin", AccountPrivilege.Silver);

            var accRepo = new SimpleAccountRepository(new[] { from, to });
            var txRepo = new SimpleTransactionRepository();

            var cmd = new TransferTransactionCommand(accRepo, txRepo);

            var dto = new TransactionDto { FromAccount = "T_FROM", ToAccount = "T_TO", Pin = "fpin", Amount = 1000m };

            // act
            var result = cmd.ExecuteAsync(dto).GetAwaiter().GetResult();

            // assert
            Assert.AreEqual(4000m, from.Balance);
            Assert.AreEqual(2000m, to.Balance);
            Assert.AreEqual(Domain.Enums.TransactionStatus.Success, result.TransactionStat);
        }

        [TestMethod]
        public void Transfer_InvalidPin_ThrowsInvalidPinException()
        {
            var from = AccountFactory.CreateAccount(AccountType.Savings, "F1", "From", 30, 5000m, AccountStatus.Active, "fpin", AccountPrivilege.Silver);
            var to = AccountFactory.CreateAccount(AccountType.Savings, "T1", "To", 25, 1000m, AccountStatus.Active, "tpin", AccountPrivilege.Silver);

            var accRepo = new SimpleAccountRepository(new[] { from, to });
            var txRepo = new SimpleTransactionRepository();

            var cmd = new TransferTransactionCommand(accRepo, txRepo);
            var dto = new TransactionDto { FromAccount = "F1", ToAccount = "T1", Pin = "bad", Amount = 100m };

            Assert.ThrowsException<InvalidPinException>(() => cmd.ExecuteAsync(dto).GetAwaiter().GetResult());
        }

        [TestMethod]
        public void Transfer_InactiveAccount_ThrowsInactiveAccountException()
        {
            var from = AccountFactory.CreateAccount(AccountType.Savings, "F2", "From", 30, 5000m, AccountStatus.Inactive, "fpin", AccountPrivilege.Silver);
            var to = AccountFactory.CreateAccount(AccountType.Savings, "T2", "To", 25, 1000m, AccountStatus.Active, "tpin", AccountPrivilege.Silver);

            var accRepo = new SimpleAccountRepository(new[] { from, to });
            var txRepo = new SimpleTransactionRepository();

            var cmd = new TransferTransactionCommand(accRepo, txRepo);
            var dto = new TransactionDto { FromAccount = "F2", ToAccount = "T2", Pin = "fpin", Amount = 100m };

            Assert.ThrowsException<InactiveAccountException>(() => cmd.ExecuteAsync(dto).GetAwaiter().GetResult());
        }

        [TestMethod]
        public void WithdrawCommand_Success_UpdatesBalance()
        {
            var acc = AccountFactory.CreateAccount(AccountType.Savings, "WC1", "W", 30, 2000m, AccountStatus.Active, "wpin", AccountPrivilege.Silver);
            var repo = new SimpleAccountRepository(new[] { acc });
            var txRepo = new SimpleTransactionRepository();
            var cmd = new WithdrawTransactionCommand(repo, txRepo);
            var dto = new TransactionDto { AccountNumber = "WC1", Pin = "wpin", Amount = 500m };

            var resp = cmd.ExecuteAsync(dto).GetAwaiter().GetResult();
            Assert.AreEqual(1500m, acc.Balance);
            Assert.AreEqual(Domain.Enums.TransactionStatus.Success, resp.TransactionStat);
        }

        [TestMethod]
        public void WithdrawCommand_InvalidPin_ThrowsInvalidPinException()
        {
            var acc = AccountFactory.CreateAccount(AccountType.Savings, "WC2", "W", 30, 2000m, AccountStatus.Active, "wpin", AccountPrivilege.Silver);
            var repo = new SimpleAccountRepository(new[] { acc });
            var txRepo = new SimpleTransactionRepository();
            var cmd = new WithdrawTransactionCommand(repo, txRepo);
            var dto = new TransactionDto { AccountNumber = "WC2", Pin = "bad", Amount = 100m };

            Assert.ThrowsException<InvalidPinException>(() => cmd.ExecuteAsync(dto).GetAwaiter().GetResult());
        }
    }
}
