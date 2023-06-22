namespace FHLB.Api.Entities;

public record Transaction
{
  public int Id { get; init; }

  public int FromAccountId { get; init; }

  public int ToAccountId { get; init; }

  public int Amount { get; init; }

  public string? Status { get; set; }

  public DateTime Date { get; init; }
}
