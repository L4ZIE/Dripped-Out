namespace infrastructure.Entities
{
    public class UserCart
    {
        public int UserId { get; set; }
        
        public int ProductId { get; set; }
        
        public int ColorId { get; set; }
        
        public int SizeId { get; set; }
        
        public int Quantity { get; set; }
    }
}