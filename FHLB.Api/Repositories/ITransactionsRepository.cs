namespace FHLB.Api.Entities;

public interface ITransactionsRepository
{
    Transaction CreateTransaction(Transaction transaction);

    IEnumerable<Transaction> GetDebitTransactions(int id);

    IEnumerable<Transaction> GetCreditTransactions(int id);

    Transaction GetTransaction(int id);
    IEnumerable<Transaction> GetTransactions();
}
