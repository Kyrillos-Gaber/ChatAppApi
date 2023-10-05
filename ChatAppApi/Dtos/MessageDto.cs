using System.ComponentModel.DataAnnotations;

namespace ChatAppApi.Dtos;

public class MessageDto
{
    [Required]
    public required string From {  get; set; }
 
    public string? To { get; set; }

    [Required]
    public required string content { get; set; }
}
