
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiControllerSample
{
    public class Pet(
        int Id,
        [Range(0, 150)] int Age,
        Category Category, 
        bool HasVaccinations,
        [StringLength(50, MinimumLength = 2)] Name,
        List<Image> Images = new List<Image>(),
        List<Tag> Tags = new List<Tag>(),
        string Status);

    public Pet With(
        int Id = this.Id,
        [Range(0, 150)] int Age = this.Age,
        Category Category = this.Category,
        bool HasVaccinations = this.HasVaccinations,
        [StringLength(50, MinimumLength = 2)] Name = this.Name,
        List<Image> Images = this.Images,
        List<Tag> Tags = this.Tags,
        string Status = this.Status)
}