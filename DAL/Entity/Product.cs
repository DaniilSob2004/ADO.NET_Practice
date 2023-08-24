namespace DAL.Entity
{
    public class Product
    {
        public string Name { get; set; } = null!;
        public float Price { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; } = null!;
    }
}
