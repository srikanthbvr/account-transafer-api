namespace FHLB.Api;

public record TransactionDto
{
    public int Id { get; init; }
    public int FromAccountId { get; init; }
    public int ToAccountId { get; init; }
    public int Amount { get; init; }
    public string? Status { get; set; }
    public string? ErrorMessage { get; set; }
    public string? TransactionType { get; set; }
    public DateTime Date { get; init; }
}
