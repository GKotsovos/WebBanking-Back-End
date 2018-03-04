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
