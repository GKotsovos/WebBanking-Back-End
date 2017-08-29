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
        LoanManager loanManager;

        public LoanServices()
        {
            loanManager = new LoanManager();
        }

        public Loan GetLoanById(string loanId)
        {
            return loanManager.GetLoanById(loanId);
        }

        public List<Loan> GetAllCustomerLoans(string customerId)
        {
            return loanManager.GetAllCustomerLoans(customerId).ToList();
        }

        public void LoanUpdate(Loan loan)
        {
            loanManager.Update(loan);
        }
    }
}
