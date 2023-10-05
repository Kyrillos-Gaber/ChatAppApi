using System.ComponentModel.DataAnnotations;

namespace chatapi.Dtos;

public class UserDto
{
    [Required, StringLength(15, MinimumLength = 3)] 
    public required string Name { get; set;}
}
