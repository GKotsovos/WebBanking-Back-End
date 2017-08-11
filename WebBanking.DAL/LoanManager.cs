using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;

namespace WebBanking.DAL
{
    public class LoanManager
    {
        public Loan GetLoanById(string loanId)
        {
            using (var bankContext = new BankingContext())
            {
                var loan = bankContext.Loan
                    .FirstOrDefault(_loan => _loan.Id == loanId);

                if (loan != null)
                {
                    return loan;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<Loan> GetAllCustomerLoans(string customerId)
        {
            using (var bankContext = new BankingContext())
            {
                var loans = bankContext.Loan
                    .Where(loan => loan.CustomerId == customerId)
                    .ToList();

                if (loans != null)
                {
                    return loans;
                }
                else
                {
                    return null;
                }
            }
        }

        public void Update(Loan loan)
        {
            using (var bankContext = new BankingContext())
            {
                bankContext.Loan.Update(loan);
                bankContext.SaveChanges();
            }
        }
    }
}
