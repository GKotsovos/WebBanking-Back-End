using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;
using WebBanking.DAL;

namespace WebBanking.Services
{
    public class PaymentServices
    {
        AccountServices accountServices;
        CardServices cardServices;
        LoanServices loanServices;
        public PaymentServices(AccountServices accountServices, CardServices cardServices, LoanServices loanServices)
        {
            this.accountServices = accountServices;
            this.cardServices = cardServices;
            this.loanServices = loanServices;
        }

        public TransactionResult CreditCardPayment(CardTransaction cardTransaction, IHasBalances debitAccount)
        {
            TransactionResult transactionResult = new TransactionResult(false, "");
            if (debitAccount != null)
            {
                var creditCard = cardServices.GetCreditCardById(cardTransaction.CardId);
                if (creditCard != null)
                {
                    if (debitAccount.AvailableBalance >= (cardTransaction.Amount + cardTransaction.Expenses))
                    {
                        transactionResult = PayCreditCard(creditCard, cardTransaction.Amount);
                        if (!transactionResult.HasError)
                        {
                            accountServices.DebitAccount(debitAccount, cardTransaction.Amount, cardTransaction.Expenses);
                            cardServices.UpdateCreditCard(creditCard);
                        }
                    }
                    else
                    {
                        transactionResult = new TransactionResult(true, "Το υπόλοιπο του λογαριασμού χρέωσης δεν είναι αρκετό");
                    }
                }
                else
                {
                    transactionResult = new TransactionResult(true, "Η πιστωτική κάρτα δε βρέθηκε");
                }
            }
            else
            {
                transactionResult = new TransactionResult(true, "Ο λογαριασμός χρέωσης δε βρέθηκε");
            }

            return transactionResult;
        }

        private TransactionResult PayCreditCard(CreditCard creditCard, decimal paymentAmount)
        {
            var transactionResult = new TransactionResult(false, "");

            if ((creditCard.NextInstallmentAmount < paymentAmount) && (creditCard.Debt < paymentAmount))
            {
                transactionResult = new TransactionResult(true, "Το ποσό πληρωμής είναι μεγαλύτερο από το σύνολο οφειλών");
            }
            else if (creditCard.Debt < paymentAmount)
            {
                transactionResult = new TransactionResult(true, "Το ποσό πληρωμής είναι μεγαλύτερο από την τρέχων οφειλή");
            }
            else if (creditCard.NextInstallmentAmount >= paymentAmount)
            {
                creditCard.NextInstallmentAmount -= paymentAmount;
                creditCard.Debt -= paymentAmount;
            }
            else
            {
                creditCard.NextInstallmentAmount = 0;
                creditCard.Debt -= paymentAmount;
            }

            return transactionResult;
        }

        public TransactionResult LoanPayment(LoanTransaction loanTransaction, IHasBalances debitAccount)
        {
            TransactionResult transactionResult = new TransactionResult(false, "");
            if (debitAccount != null)
            {
                var loan = loanServices.GetLoanById(loanTransaction.LoanId);
                if (loan != null)
                {
                    if (debitAccount.AvailableBalance >= loanTransaction.Amount)
                    {
                        transactionResult = PayLoan(loan, loanTransaction.Amount);
                        if (!transactionResult.HasError)
                        {
                            accountServices.DebitAccount(debitAccount, loanTransaction.Amount, 0);
                            loanServices.LoanUpdate(loan);
                        }
                    }
                    else
                    {
                        transactionResult = new TransactionResult(true, "Το υπόλοιπο του λογαριασμού χρέωσης δεν είναι αρκετό");
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

        private TransactionResult PayLoan(Loan loan, decimal payentAmount)
        {
            var transactionResult = new TransactionResult(false, "");

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

            return transactionResult;
        }
    }
}
