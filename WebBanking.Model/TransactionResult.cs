using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanking.Model
{
    public class TransactionResult
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
        public TransactionResult(bool hasError, string message)
        {
            this.HasError = hasError;
            this.Message = message;
        }
    }
}
