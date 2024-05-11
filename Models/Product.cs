namespace ReviewSystem.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<Review> Reviews { get; set; }

    }
}
