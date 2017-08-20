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
            cardServices = new CardServices(new AccountServices(), this, new TransactionService());
            accountServices = new AccountServices();
            transactionHistoryService = new TransactionService();
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

        public TransactionResult LoanPaymentUsingAccount(string customerId, LoanTransaction loanTransaction)
        {
            TransactionResult transactionResult = new TransactionResult(false, "");
            var debitAccount = accountServices.GetAccountByIban(loanTransaction.DebitAccount);
            if (debitAccount != null)
            {
                var loan = GetLoanById(loanTransaction.LoanId);
                if (loan != null)
                {
                    transactionResult = LoanPayment(loan, loanTransaction.Amount, debitAccount.AvailableBalance);
                    if (!transactionResult.HasError)
                    {
                        debitAccount.AvailableBalance -= loanTransaction.Amount;
                        debitAccount.LedgerBalance -= loanTransaction.Amount;
                        accountServices.UpdateAccount(debitAccount);
                        LoanUpdate(loan);
                        AddLoanPaymentTransactionHistory(customerId, loanTransaction, debitAccount.Iban, debitAccount.LedgerBalance);
                    }
                }
                else
                {
                    transactionResult = new TransactionResult(true, "Το δάνειο δε βρέθηκε");
                }
            }
            else
            {
                transactionResult = new TransactionResult(true, "Ο λογαριασμός χρέωσης δε βρέθηκε");
            }

            return transactionResult;
        }

        public TransactionResult LoanPaymentUsingLoan(string customerId, LoanTransaction loanTransaction)
        {
            TransactionResult transactionResult = new TransactionResult(false, "");
            var debitLoan = GetLoanById(loanTransaction.DebitAccount);
            if (debitLoan != null)
            {
                var loan = GetLoanById(loanTransaction.LoanId);
                if (loan != null)
                {
                    transactionResult = LoanPayment(loan, loanTransaction.Amount, debitLoan.AvailableBalance);
                    if (!transactionResult.HasError)
                    {
                        debitLoan.AvailableBalance -= loanTransaction.Amount;
                        LoanUpdate(debitLoan);
                        LoanUpdate(loan);
                        AddLoanPaymentTransactionHistory(customerId, loanTransaction, debitLoan.Id, debitLoan.AvailableBalance);
                    }
                }
                else
                {
                    transactionResult = new TransactionResult(true, "Το δάνειο δε βρέθηκε");
                }
            }
            else
            {
                transactionResult = new TransactionResult(true, "Το δάνειο χρέωσης δε βρέθηκε");
            }

            return transactionResult;
        }

        public TransactionResult LoanPaymentUsingCreditCard(string customerId, LoanTransaction loanTransaction)
        {
            TransactionResult transactionResult = new TransactionResult(false, "");
            var creditCard = cardServices.GetCreditCardById(loanTransaction.DebitAccount);
            if (creditCard != null)
            {
                var loan = GetLoanById(loanTransaction.LoanId);
                if (loan != null)
                {
                    transactionResult = LoanPayment(loan, loanTransaction.Amount, creditCard.AvailableLimit);
                    if (!transactionResult.HasError)
                    {
                        creditCard.AvailableLimit -= loanTransaction.Amount;
                        cardServices.UpdateCreditCard(creditCard);
                        LoanUpdate(loan);
                        AddLoanPaymentTransactionHistory(customerId, loanTransaction, creditCard.Id, creditCard.AvailableLimit);
                    }
                }
                else
                {
                    transactionResult = new TransactionResult(true, "Το δάνειο δε βρέθηκε");
                }
            }
            else
            {
                transactionResult = new TransactionResult(true, "Η πιστωτική κάρτα χρέωσης δε βρέθηκε");
            }

            return transactionResult;
        }

        public TransactionResult LoanPaymentUsingPrepaidCard(string customerId, LoanTransaction loanTransaction)
        {
            TransactionResult transactionResult = new TransactionResult(false, "");
            var prepaidCard = cardServices.GetPrePaidCardById(loanTransaction.DebitAccount);
            if (prepaidCard != null)
            {
                var loan = GetLoanById(loanTransaction.LoanId);
                if (loan != null)
                {
                    transactionResult = LoanPayment(loan, loanTransaction.Amount, prepaidCard.AvailableLimit);
                    if (!transactionResult.HasError)
                    {
                        prepaidCard.AvailableLimit -= loanTransaction.Amount;
                        prepaidCard.LedgerBalance -= loanTransaction.Amount;
                        cardServices.UpdatePrepaidCard(prepaidCard);
                        LoanUpdate(loan);
                        AddLoanPaymentTransactionHistory(customerId, loanTransaction, prepaidCard.Id, prepaidCard.AvailableLimit);
                    }
                }
                else
                {
                    transactionResult = new TransactionResult(true, "Η προπληρωμένη κάρτα δε βρέθηκε");
                }
            }
            else
            {
                transactionResult = new TransactionResult(true, "Η προπληρωμένη κάρτα χρέωσης δε βρέθηκε");
            }

            return transactionResult;
        }

        private TransactionResult LoanPayment(Loan loan, decimal payentAmount, decimal debitAccountAvailableBalance)
        {
            var transactionResult = new TransactionResult(false, "");

            if (debitAccountAvailableBalance >= payentAmount)
            {
                if ((loan.NextInstallmentAmount < payentAmount) && (loan.Debt < payentAmount))
                {
                    transactionResult = new TransactionResult(true, "Το ποσό πληρωμής είναι μεγαλύτερο από το σύνολο οφειλών");
                }
                else if (loan.Debt < payentAmount)
                {
                    transactionResult = new TransactionResult(true, "Το ποσό πληρωμής είναι μεγαλύτερο από την τρέχων οφειλή");
                }
                else if (loan.NextInstallmentAmount >= payentAmount)
                {
                    loan.NextInstallmentAmount -= payentAmount;
                    loan.Debt -= payentAmount;
                    loan.RepaymentBalance -= payentAmount;
                }
                else
                {
                    loan.NextInstallmentAmount = 0;
                    loan.Debt -= payentAmount;
                    loan.RepaymentBalance -= payentAmount;
                }
            }
            else
            {
                transactionResult = new TransactionResult(true, "Το υπόλοιπο του λογαριασμού χρέωσης δεν είναι αρκετό");
            }

            return transactionResult;
        }

        public void AddLoanPaymentTransactionHistory(string customerId, LoanTransaction loanTransaction, string debitAccountId, decimal newAvailableAmount)
        {
            var transaction = new Transaction();
            transaction.Amount = loanTransaction.Amount;
            transaction.Beneficiary = "Agile Bank";
            transaction.Currency = loanTransaction.Currency;
            transaction.CustomerId = customerId;
            transaction.Date = loanTransaction.Date;
            transaction.Details = "ΠΛΗΡΩΜΗ ΔΑΝΕΙΟΥ";
            transaction.LedgerBalance = newAvailableAmount;
            transaction.ProductId = debitAccountId;
            transaction.TransactionType = "debit";
            transactionHistoryService.AddTransaction(transaction);
        }
    }
}
