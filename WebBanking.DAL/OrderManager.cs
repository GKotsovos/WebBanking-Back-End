using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;

namespace WebBanking.DAL
{
    public class OrderManager
    {
        public TransferOrder GetTransferOrderById(long transferOrderId)
        {
            using (var bankContext = new BankingContext())
            {
                return bankContext.TransferOrder
                    .FirstOrDefault(transferOrder => transferOrder.Id == transferOrderId);
            }
        }

        public List<TransferOrder> GetAllCustomerTransferOrders(string customerId)
        {
            using (var bankContext = new BankingContext())
            {
                var transferOrders = bankContext.TransferOrder
                    .Where(transferOrder => transferOrder.CustomerId == customerId)
                    .ToList();

                if (transferOrders != null)
                {
                    return transferOrders;
                }
                else
                {
                    return null;
                }
            }
        }

        public void InsertTransferOrder(TransferOrder transferOrder)
        {
            using (var bankContext = new BankingContext())
            {
                bankContext.TransferOrder.Add(transferOrder);
                bankContext.SaveChanges();
            }
        }

        public void DeleteTransferOrder(TransferOrder transferOrder)
        {
            using (var bankContext = new BankingContext())
            {
                try
                {
                    bankContext.TransferOrder.Remove(transferOrder);
                    bankContext.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public PaymentOrder GetPaymentOrderById(long paymentOrderId)
        {
            using (var bankContext = new BankingContext())
            {
                return bankContext.PaymentOrder
                    .FirstOrDefault(paymentOrder => paymentOrder.Id == paymentOrderId);
            }
        }

        public List<PaymentOrder> GetAllCustomerPaymentOrders(string customerId)
        {
            using (var bankContext = new BankingContext())
            {
                var paymentOrders = bankContext.PaymentOrder
                    .Where(paymentOrder => paymentOrder.CustomerId == customerId)
                    .ToList();

                if (paymentOrders != null)
                {
                    return paymentOrders;
                }
                else
                {
                    return null;
                }
            }
        }

        public void InsertPaymentOrder(PaymentOrder paymentOrder)
        {
            using (var bankContext = new BankingContext())
            {
                bankContext.PaymentOrder.Add(paymentOrder);
                bankContext.SaveChanges();
            }
        }

        public void DeletePaymentOrder(PaymentOrder paymentOrder)
        {
            using (var bankContext = new BankingContext())
            {
                try
                {
                    bankContext.PaymentOrder.Remove(paymentOrder);
                    bankContext.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
