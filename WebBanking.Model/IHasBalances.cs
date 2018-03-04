namespace WebBanking.Model
{
    public interface IHasBalances
    {
        string Id { get; set; }
        decimal AvailableBalance { get; set; }
        decimal LedgerBalance { get; set; }
    }
}
