﻿using System.Collections.Generic;
using WebBanking.Model;
using WebBanking.DAL;
using WebBanking.Services.HelperMethods;

namespace WebBanking.Services
{
    public class PaymentServices
    {
        PaymentManager paymentManager;
        TransferServices transferServices;
        TransactionServices transactionServices;
        Helper helper;

        public PaymentServices(TransferServices transferServices, AccountServices accountServices, CardServices cardServices, LoanServices loanServices, TransactionServices transactionServices)
        {
            paymentManager = new PaymentManager();
            this.transferServices = transferServices;
            this.transactionServices = transactionServices;
            helper = new Helper(accountServices, cardServices, loanServices);
        }

        public List<PaymentMethod> GetPaymentMethods()
        {
            return paymentManager.GetPaymentMethods();
        }

        public TransactionResult CheckDebt(IHasInstallment productForPayment, decimal paymentAmount, string language)
        {
            var transactionResult = new TransactionResult(false, "");

            if ((productForPayment.NextInstallmentAmount < paymentAmount) && (productForPayment.Debt < paymentAmount))
            {
                string errorMessage = language == "greek" ? "Το ποσό πληρωμής είναι μεγαλύτερο από το σύνολο οφειλών" : "The payment amount is bigger than the total debt";
                transactionResult = new TransactionResult(true, errorMessage);
            }
            else if (productForPayment.NextInstallmentAmount < paymentAmount)
            {
                string errorMessage = language == "greek" ? "Το ποσό πληρωμής είναι μεγαλύτερο από την τρέχων οφειλή" : "The payment amount is bigger than the current installment";
                transactionResult = new TransactionResult(true, errorMessage);
            }

            return transactionResult;
        }

        public TransactionResult AgilePayment(string customerId, string title, TransactionDTO transaction, string language)
        {
            var transactionResult = new TransactionResult(false, "");

            IHasBalances debitProduct = helper.GetProduct(transaction.DebitProductType, transaction.DebitProductId, out transactionResult, language);
            transactionResult = transferServices.CheckDebitBalance(debitProduct, transaction.Amount, language);
            IHasInstallment productForPayment = (IHasInstallment)helper.GetProduct(transaction.CreditProductType, transaction.CreditProductId, out transactionResult, language);
            if (!transactionResult.HasError)
            {
                transactionResult = CheckDebt(productForPayment, transaction.Amount, language);
                if (!transactionResult.HasError)
                {
                    Payment(debitProduct, productForPayment, transaction.Amount, transaction.Expenses);
                    transactionResult = helper.UpdateProduct(transaction.CreditProductType, (IHasBalances)productForPayment, language);
                    if (!transactionResult.HasError)
                    {
                        transactionResult = helper.UpdateProduct(transaction.DebitProductType, debitProduct, language);
                        if (!transactionResult.HasError)
                        {
                            transactionServices.LogTransaction(customerId, title, debitProduct.AvailableBalance, transaction);
                        }
                    }
                }
            }

            return transactionResult;
        }

        public TransactionResult ThirdPartyPayment(string customerId, TransactionDTO transaction, string language)
        {
            var transactionResult = new TransactionResult(false, "");

            IHasBalances debitProduct = helper.GetProduct(transaction.DebitProductType, transaction.DebitProductId, out transactionResult, language);
            transactionResult = transferServices.CheckDebitBalance(debitProduct, transaction.Amount, language);
            if (!transactionResult.HasError)
            {
                transferServices.DebitProduct(debitProduct, transaction.Amount, transaction.Expenses);
                transactionResult = helper.UpdateProduct(transaction.DebitProductType, debitProduct, language);
                if (!transactionResult.HasError)
                {
                    transactionServices.LogTransaction(customerId, "ΗΛΕΚΤΡΟΝΙΚΗ ΠΛΗΡΩΜΗ", debitProduct.AvailableBalance, transaction);
                }
            }

            return transactionResult;
        }

        public void Payment(IHasBalances debitProduct, IHasInstallment productForPayment, decimal paymentAmount, decimal expenses)
        {
            transferServices.DebitProduct(debitProduct, paymentAmount, expenses);
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
