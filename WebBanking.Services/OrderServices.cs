using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;
using WebBanking.DAL;

namespace WebBanking.Services
{
    public class OrderServices
    {
        OrderManager orderManager;

        public OrderServices()
        {
            orderManager = new OrderManager();
        }

        public TransferOrder GetTransferOrder(long transferOrderId)
        {
            return orderManager.GetTransferOrderById(transferOrderId);
        }

        public List<TransferOrder> GetAllCustomerTransferOrders(string customerId)
        {
            return orderManager.GetAllCustomerTransferOrders(customerId).ToList();
        }

        public TransactionResult CreateTransferOrder(string customerId, TransferOrder transferOrder, string language)
        {
            var transactionResult = new TransactionResult(false, "");
            transferOrder.CustomerId = customerId;
            try
            {
                orderManager.InsertTransferOrder(transferOrder);
            }
            catch (Exception)
            {
                transactionResult.HasError = true;
                transactionResult.Message = language == "greek" ? "Σφάλμα κατά την δημιουργία της πάγιας εντολής μεταφοράς" :
                    "There was an error while creating the order";
            }
            return transactionResult;
        }

        public TransactionResult CancelTransferOrder(string customerId, long transferOrderId, string language)
        {
            var transactionResult = new TransactionResult(false, "");
            try
            {
                var transferOrder = GetTransferOrder(transferOrderId);
                orderManager.DeleteTransferOrder(transferOrder);
            }
            catch (Exception)
            {
                transactionResult.HasError = true;
                transactionResult.Message = language == "greek" ? "Σφάλμα κατά την ακύρωση της πάγιας εντολής μεταφοράς" :
                    "There was an error while canceling the order";
            }
            return transactionResult;
        }

        public PaymentOrder GetPaymentOrder(long paymentOrderId)
        {
            return orderManager.GetPaymentOrderById(paymentOrderId);
        }

        public List<PaymentOrder> GetAllCustomerPaymentOrders(string customerId)
        {
            return orderManager.GetAllCustomerPaymentOrders(customerId).ToList();
        }

        public TransactionResult CreatePaymentOrder(string customerId, PaymentOrder paymentOrder, string language)
        {
            var transactionResult = new TransactionResult(false, "");
            paymentOrder.CustomerId = customerId;
            paymentOrder.PreviousExecutionDate = paymentOrder.ExpirationDate;
            try
            {
                orderManager.InsertPaymentOrder(paymentOrder);
            }
            catch (Exception)
            {
                transactionResult.HasError = true;
                transactionResult.Message = language == "greek" ? "Σφάλμα κατά την δημιουργία της πάγιας εντολής πληρωμής" :
                    "There was an error while creating the order";
            }
            return transactionResult;
        }

        public TransactionResult CancelPaymentOrder(string customerId, long paymentOrderId, string language)
        {
            var transactionResult = new TransactionResult(false, "");
            try
            {
                var paymentOrder = GetPaymentOrder(paymentOrderId);
                orderManager.DeletePaymentOrder(paymentOrder);
            }
            catch (Exception)
            {
                transactionResult.HasError = true;
                transactionResult.Message = language == "greek" ? "Σφάλμα κατά την ακύρωση της πάγιας εντολής πλήρωμής" :
                    "There was an error while canceling the payment order";
            }
            return transactionResult;
        }
    }
}
