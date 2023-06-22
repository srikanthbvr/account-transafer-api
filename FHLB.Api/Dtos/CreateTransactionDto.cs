using System.ComponentModel.DataAnnotations;

namespace FHLB.Api;

public record CreateTransactionDto
{
    [Required]
    public int FromAccountId { get; init; }
    [Required]
    public int ToAccountId { get; init; }
    [Required]
    [Range(1, int.MaxValue)]
    public int Amount { get; init; }
}
