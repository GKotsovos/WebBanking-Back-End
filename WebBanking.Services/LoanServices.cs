using System;
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
        public Loan GetLoan(string loanId)
        {
            return accountMamager.GetLoanById(accountId);
            return loanManager.GetLoanById(loanId);
        }

        public List<Loan> GetAllCustomerLoans(string customerId)
        {
            return accountMamager.GetAllCustomerLoans(customerId).ToList();
        }

    }
}
