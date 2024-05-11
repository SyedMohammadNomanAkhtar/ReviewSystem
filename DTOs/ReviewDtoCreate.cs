using System.ComponentModel.DataAnnotations;

namespace ReviewSystem.DTOs
{
    public class ReviewDtoCreate
    {
        [Required]
        public string ReviewText { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ProductId { get; set; }
    }
}
