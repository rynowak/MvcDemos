
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiControllerSample
{
    public class EditPetDto
    {
        [Range(0, 150)]
        public int Age { get; set; }

        public Category Category { get; set; }

        public bool HasVaccinations { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        public List<Tag> Tags { get; set; }

        [Required]
        public string Status { get; set; }
    }
}
