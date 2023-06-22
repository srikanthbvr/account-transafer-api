using System.ComponentModel.DataAnnotations;

namespace FHLB.Api.Dtos;

public record AccountDto
{
  public int Id { get; init; }
  [Required]
  public required string AccountName { get; init; }
  public int AccountBalance { get; set; }
}
