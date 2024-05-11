using System.ComponentModel.DataAnnotations;

namespace ReviewSystem.DTOs
{
    public class ProductDtoCreate
    {
        [Required]
        public string Name { get; set; }

        [StringLength(100)]
        [Required]
        public string Description { get; set; }
    }
}
