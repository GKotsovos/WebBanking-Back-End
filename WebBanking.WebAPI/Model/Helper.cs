﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;
using WebBanking.Services;

namespace WebBanking.WebAPI.Model
{
    public class Helper
    {
        AccountServices accountServices;
        CardServices cardServices;
        LoanServices loanServices;

        public Helper(AccountServices accountServices, CardServices cardServices, LoanServices loanServices)
        {
            this.accountServices = accountServices;
            this.cardServices = cardServices;
            this.loanServices = loanServices;
        }

        public IHasBalances GetDebitAccount(string debitAccountType, string debitAccountId)
        {
            IHasBalances debitAccount = null;

            switch (debitAccountType)
            {
                case "isAccount":
                    debitAccount = accountServices.GetAccountById(debitAccountId);
                    break;
                case "isCreditCard":
                    debitAccount = cardServices.GetCreditCardById(debitAccountId);
                    break;
                case "isPrepaidCard":
                    debitAccount = cardServices.GetPrePaidCardById(debitAccountId);
                    break;
                case "isLoan":
                    debitAccount = loanServices.GetLoanById(debitAccountId);
                    break;
            }

            return debitAccount;
        }

    }
}
