
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiControllerSample
{
    public class EditPetDto(
        [Range(0, 150)] int Age, 
        Category Category, 
        bool HasVaccinations,
        [StringLength(50, MinimumLength = 2)] Name,
        List<Tag> Tags,
        string Status);
}
