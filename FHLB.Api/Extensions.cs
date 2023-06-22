using FHLB.Api.Dtos;
using FHLB.Api.Entities;

namespace FHLB.Api;

public static class Extensions
{
    public static AccountDto AsDto(this Account account)
    {
        return new AccountDto
        {
            Id = account.Id,
            AccountName = account.AccountName,
            AccountBalance = account.AccountBalance,
            AccountType = account.AccountType
        };
    }

    public static TransactionDto AsDto(this Transaction transaction)
    {
        return new TransactionDto
        {
            Id = transaction.Id,
            FromAccountId = transaction.FromAccountId,
            ToAccountId = transaction.ToAccountId,
            Amount = transaction.Amount,
            Status = transaction.Status,
            ErrorMessage = transaction.ErrorMessage,
            Date = transaction.Date
        };
    }
}
