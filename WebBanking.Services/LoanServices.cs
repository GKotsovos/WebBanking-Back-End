﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;
using WebBanking.DAL;

namespace WebBanking.Services
{
    public class LoanServices
    {
        LoanManager accountMamager;

        public LoanServices()
        {
            accountMamager = new LoanManager();
        }

        public Loan GetLoan(string accountId)
        {
            return accountMamager.GetLoanById(accountId);
        }

        public List<Loan> GetAllCustomerLoans(string customerId)
        {
            return accountMamager.GetAllCustomerLoans(customerId).ToList();
        }

    }
}
