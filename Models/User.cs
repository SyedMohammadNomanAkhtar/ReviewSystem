namespace ReviewSystem.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public List<Review> Reviews { get; set; }
    }
}
