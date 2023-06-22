using System.ComponentModel.DataAnnotations;

namespace FHLB.Api;

public record CreateAccountDto
{
    [Required]
    [MinLength(5, ErrorMessage = "Account name must be at least 5 characters long.")]
    [MaxLength(50, ErrorMessage = "Account name cannot be longer than 50 characters.")]
    public required string AccountName { get; init; }
}
