using GDB.App.Domain;
using GDB.App.Domain.Enums;
using GDB.App.Domain.Exceptions;
using GDB.App.Domain.Models;

namespace GDB.App.Test
{
    [TestClass]
    public sealed class Test1
    {
        private IAccount _account;

        [TestInitialize]
        public void Setup()
        {
            _account = AccountFactory.CreateAccount(
                AccountType.Savings,
                "1001",
                "Test User",
                25,
                100.00m,
                AccountStatus.Active,
                "1234",
                AccountPrivilege.Silver
            );
        }

        [TestMethod]
        public void Deposit_PositiveAmount_IncreasesBalance()
        {
            // Arrange
            decimal amount = 50m;
            decimal initialBalance = _account.Balance;

            // Act
            _account.Deposit(amount);

            // Assert
            Assert.AreEqual(initialBalance + amount, _account.Balance);
        }

        [TestMethod]
        public void Deposit_FractionalAmount_IncreasesBalance()
        {
            // Arrange
            decimal amount = 0.5m;
            decimal initialBalance = _account.Balance;

            // Act
            _account.Deposit(amount);

            // Assert
            Assert.AreEqual(initialBalance + amount, _account.Balance);
        }

        [TestMethod]
        public void Deposit_NegativeAmount_ThrowsInvalidAmountException()
        {
            // Arrange
            decimal amount = -0.5m;

            // Act & Assert
            Assert.ThrowsException<InvalidAmountException>(
                () => _account.Deposit(amount)
            );
        }

        [TestMethod]
        public void Deposit_JustBelowLimitAmount_IncreasesBalance()
        {
            // Arrange
            decimal amount = 999999m;
            decimal initialBalance = _account.Balance;

            // Act
            _account.Deposit(amount);

            // Assert
            Assert.AreEqual(initialBalance + amount, _account.Balance);
        }

        [TestMethod]
        public void Deposit_ExactLimitAmount_ThrowsInvalidAmountException()
        {
            // Arrange
            decimal amount = 1000000m;

            // Act & Assert
            Assert.ThrowsException<InvalidAmountException>(
                () => _account.Deposit(amount)
            );
        }

        [TestMethod]
        public void Deposit_JustAboveLimitAmount_ThrowsInvalidAmountException()
        {
            // Arrange
            decimal amount = 1000001m;

            // Act & Assert
            Assert.ThrowsException<InvalidAmountException>(
                () => _account.Deposit(amount)
            );
        }
    }
}