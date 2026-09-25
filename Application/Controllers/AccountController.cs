using GDB.App.Application.Dtos;
using GDB.App.Application.Services;
using GDB.App.Application.Services.Contracts;
using GDB.App.Application.Services.Implementations;
using GDB.App.Domain.Enums;
using GDB.App.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GDB.App.Application.Controllers
{
    public class AccountController
    {
        private IAccountService _accountService;
        public AccountController()
        {
            _accountService = AccountServiceFactory.Create();
        }

        //Boundary Class 
        public async Task<IAccount> GetAccountAsync(string accNo)
        {

            IAccount account = null;

            //Controller->Service
            account = await _accountService.GetAccountAsync(accNo);


            return account;
        }
        public List<ViewAllAccountsResponseDto> GetAllAccounts()
        {
            return _accountService.GetAllAccounts();
        }

        //public void ChangePin(string accountNumber, string oldPin, string newPin)
        //{
        //    _accountService.ChangePin(accountNumber, oldPin, newPin);
        //}

        public async Task<ViewBalanceResponseDto> GetBalanceAsync(string accNo)
        {
            return await _accountService.GetBalanceAsync(accNo);
        }
        public async Task<ViewAccountResponseDto> ViewAccountAsync(string accNo)
        {
            return await _accountService.ViewAccountAsync(accNo);
        }


        public CreateAccountResponseDto CreateAccount(CreateAccountRequestDto request)
        {
            try
            {
                return _accountService.CreateAccount(request);
            }
            catch(InvalidOperationException ex)
            {
                throw ex;
            }
        }

        public async Task<CloseAccountResponseDto> CloseAccountAsync(CloseAccountRequestDto request)
        {
            return await _accountService.CloseAccountAsync(request);
        }
    }
}
