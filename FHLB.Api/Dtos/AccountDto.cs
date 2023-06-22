using System.ComponentModel.DataAnnotations;

namespace FHLB.Api.Dtos;

public record AccountDto
{
  public int Id { get; init; }
  [Required]
  [MinLength(5, ErrorMessage = "Account name must be at least 5 characters long.")]
  [MaxLength(50, ErrorMessage = "Account name cannot be longer than 50 characters.")]
  public required string AccountName { get; init; }

  public string? AccountType { get; set; }
  public int AccountBalance { get; set; }
}
