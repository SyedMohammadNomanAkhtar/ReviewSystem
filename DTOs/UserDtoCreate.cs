using System.ComponentModel.DataAnnotations;

namespace ReviewSystem.DTOs
{
    public class UserDtoCreate
    {
        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
