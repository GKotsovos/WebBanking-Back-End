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
                        transactionResult = PayInstallment(creditCard, cardTransaction.Amount);
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
                        transactionResult = PayInstallment(loan, loanTransaction.Amount);
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


        private TransactionResult PayInstallment(IHasInstallment product, decimal paymentAmount)
        {
            var transactionResult = new TransactionResult(false, "");

            if ((product.NextInstallmentAmount < paymentAmount) && (product.Debt < paymentAmount))
            {
                transactionResult = new TransactionResult(true, "Το ποσό πληρωμής είναι μεγαλύτερο από το σύνολο οφειλών");
            }
            else if (product.Debt < paymentAmount)
            {
                transactionResult = new TransactionResult(true, "Το ποσό πληρωμής είναι μεγαλύτερο από την τρέχων οφειλή");
            }
            else if (product.NextInstallmentAmount >= paymentAmount)
            {
                product.NextInstallmentAmount -= paymentAmount;
                product.Debt -= paymentAmount;
            }
            else
            {
                product.NextInstallmentAmount = 0;
                product.Debt -= paymentAmount;
                if (product is Loan)
                {
                    (product as Loan).RepaymentBalance -= paymentAmount;
                }
            }

            return transactionResult;
        }
    }
}
