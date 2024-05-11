using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewSystem.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        public string ReviewText { get; set;}
        public string ReviewSentiment { get; set;}

        
        [ForeignKey("User")]
        public int UserId { get; set;}
        public User User { get; set;}

        [ForeignKey("Product")]
        public int ProductId { get; set;}
        public Product Product { get; set;}
    }
}
