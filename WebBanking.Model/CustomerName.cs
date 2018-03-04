namespace WebBanking.Model
{
    public class CustomerName
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public CustomerName(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
        }
    }
}
