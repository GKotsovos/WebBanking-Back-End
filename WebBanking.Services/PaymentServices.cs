using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;
using WebBanking.DAL;
using WebBanking.Services.HelperMethods;

namespace WebBanking.Services
{
    public class PaymentServices
    {
        PaymentManager paymentManager;
        TransferServices transferServices;
        Helper helper;

        public PaymentServices(TransferServices transferServices, AccountServices accountServices, CardServices cardServices, LoanServices loanServices)
        {
            paymentManager = new PaymentManager();
            this.transferServices = transferServices;
            helper = new Helper(accountServices, cardServices, loanServices);
        }

        public List<PaymentMethod> GetPaymentMethods()
        {
            return paymentManager.GetPaymentMethods();
        }

        public TransactionResult CheckDebt(IHasInstallment productForPayment, decimal paymentAmount)
        {
            var transactionResult = new TransactionResult(false, "");

            if ((productForPayment.NextInstallmentAmount < paymentAmount) && (productForPayment.Debt < paymentAmount))
            {
                transactionResult = new TransactionResult(true, "Το ποσό πληρωμής είναι μεγαλύτερο από το σύνολο οφειλών");
            }
            else if (productForPayment.NextInstallmentAmount < paymentAmount)
            {
                transactionResult = new TransactionResult(true, "Το ποσό πληρωμής είναι μεγαλύτερο από την τρέχων οφειλή");
            }

            return transactionResult;
        }

        public TransactionResult AgilePayment(TransactionDTO transaction)
        {
            var transactionResult = new TransactionResult(false, "");

            IHasBalances DebitProduct = helper.GetProduct(transaction.DebitProductIdType, transaction.DebitProductId, out transactionResult);
            transactionResult = transferServices.CheckDebitBalance(DebitProduct, transaction.Amount);
            IHasInstallment productForPayment = (IHasInstallment)helper.GetProduct(transaction.CreditProductIdType, transaction.CreditProductId, out transactionResult);
            if (!transactionResult.HasError)
            {
                transactionResult = CheckDebt(productForPayment, transaction.Amount);
                if (!transactionResult.HasError)
                {
                    Payment(DebitProduct, productForPayment, transaction.Amount, transaction.Expenses);
                    transactionResult = helper.UpdateProduct(transaction.CreditProductIdType, (IHasBalances)productForPayment);
                    if (!transactionResult.HasError)
                    {
                        transactionResult = helper.UpdateProduct(transaction.DebitProductIdType, DebitProduct);
                    }
                }
            }

            return transactionResult;
        }

        public TransactionResult ThirdPartyPayment(TransactionDTO transaction)
        {
            var transactionResult = new TransactionResult(false, "");

            IHasBalances DebitProduct = helper.GetProduct(transaction.DebitProductIdType, transaction.DebitProductId, out transactionResult);
            transactionResult = transferServices.CheckDebitBalance(DebitProduct, transaction.Amount);
            if (!transactionResult.HasError)
            {
                transactionResult = helper.UpdateProduct(transaction.DebitProductIdType, DebitProduct);
            }

            return transactionResult;
        }

        public void Payment(IHasBalances DebitProduct, IHasInstallment productForPayment, decimal paymentAmount, decimal expenses)
        {
            transferServices.DebitProductId(DebitProduct, paymentAmount, expenses);
            PayDebt(productForPayment, paymentAmount);
        }

        private void PayDebt(IHasInstallment productForPayment, decimal paymentAmount)
        {
            if (productForPayment.NextInstallmentAmount >= paymentAmount)
            {
                productForPayment.NextInstallmentAmount -= paymentAmount;
                productForPayment.Debt -= paymentAmount;
            }
            else
            {
                productForPayment.NextInstallmentAmount = 0;
                productForPayment.Debt -= paymentAmount;
                if (productForPayment is Loan)
                {
                    (productForPayment as Loan).RepaymentBalance -= paymentAmount;
                }
            }
        }
    }
}
