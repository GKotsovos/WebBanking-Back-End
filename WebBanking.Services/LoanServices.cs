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
        CardServices cardServices;
        AccountServices accountServices;
        TransactionService transactionHistoryService;

        public LoanServices()
        {
            loanManager = new LoanManager();
            cardServices = new CardServices();
            accountServices = new AccountServices();
            transactionHistoryService = new TransactionService();
        }

        public Loan GetLoan(string loanId)
        {
            return loanManager.GetLoanById(loanId);
        }

        public List<Loan> GetAllCustomerLoans(string customerId)
        {
            return loanManager.GetAllCustomerLoans(customerId).ToList();
        }

        public void Payment(string customerId, LoanPaymentDetails loanPaymentDetails)
        {
            var debitAccount = accountServices.GetAccountByIban(loanPaymentDetails.DebitAccount);
            var loan = loanManager.GetLoanById(loanPaymentDetails.LoanId);

            if (debitAccount.AvailableBalance >= loanPaymentDetails.Amount)
            {
                if (loan.NextInstallmentAmount >= loanPaymentDetails.Amount)
                {
                    loan.NextInstallmentAmount -= loanPaymentDetails.Amount;
                    loan.Debt -= loanPaymentDetails.Amount;
                }
                else if (loan.Debt >= loanPaymentDetails.Amount)
                {
                    decimal difference = loanPaymentDetails.Amount - loan.NextInstallmentAmount;

                    loan.NextInstallmentAmount = 0;
                    loan.Debt = -difference;
                }
                else
                {
                    return;
                }

                debitAccount.AvailableBalance -= loanPaymentDetails.Amount;
                debitAccount.LedgerBalance -= loanPaymentDetails.Amount;
                loanManager.Update(loan);
                accountServices.UpdateAccount(debitAccount);

                var transactionHistory = new Transaction();
                transactionHistory.Amount = loanPaymentDetails.Amount;
                transactionHistory.Beneficiary = "Agile Bank";
                transactionHistory.Currency = loanPaymentDetails.Currency;
                transactionHistory.CustomerId = customerId;
                transactionHistory.Date = loanPaymentDetails.Date;
                transactionHistory.Details = "ΠΛΗΡΩΜΗ ΔΑΝΕΙΟΥ";
                transactionHistory.LedgerBalance = debitAccount.LedgerBalance;
                transactionHistory.ProductId = debitAccount.Iban;
                transactionHistory.TransactionType = "debit";
                transactionHistoryService.AddTransaction(transactionHistory);
            }
        }
    }
}
