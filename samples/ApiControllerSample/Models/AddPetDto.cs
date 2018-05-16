using System.ComponentModel.DataAnnotations;

namespace ApiControllerSample
{
    public class AddPetDto
    {
        [Range(0, 150)]
        public int Age { get; set; }

        public Category Category { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }
    }
}
