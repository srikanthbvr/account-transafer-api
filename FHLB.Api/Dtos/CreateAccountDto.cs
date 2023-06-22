using System.ComponentModel.DataAnnotations;

namespace FHLB.Api;

public record CreateAccountDto
{
    [Required]
    public required string AccountName { get; init; }
}
