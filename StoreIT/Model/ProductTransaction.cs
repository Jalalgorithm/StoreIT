namespace StoreIT.Model
{
    public class ProductTransaction
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; } = "";

        public int UnitQuantity { get; set; }
        public User User { get; set; }
        public Product Product { get; set; }

    }
}
