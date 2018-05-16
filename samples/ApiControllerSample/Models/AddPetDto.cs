using System.ComponentModel.DataAnnotations;

namespace ApiControllerSample
{
    public class AddPetDto([Range(0, 150)] int Age, Category category, [StringLength(50, MinimumLength = 2)] Name);
}
