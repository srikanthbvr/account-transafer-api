namespace FHLB.Api.Entities;

public record Account
{
  public int Id { get; init; }
  public string? AccountName { get; init; }
  public int AccountBalance { get; set; }
  public void Deposit(int amount)
  {
    AccountBalance += amount;
  }
  public void Withdraw(int amount)
  {
    AccountBalance -= amount;
  }

}
