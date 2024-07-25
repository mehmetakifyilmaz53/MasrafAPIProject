namespace MasrafDeneme.Models
{
    public class TransactionAggregate
    {
        public int PersonId { get; set; }
        public string PersonName { get; set; }
        public decimal TotalAmount { get; set; }
        public int TransactionCount { get; set; }
    }
}
