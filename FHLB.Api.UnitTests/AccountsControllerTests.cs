using FHLB.Api.Controllers;
using FHLB.Api.Dtos;
using FHLB.Api.Entities;
using FHLB.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FHLB.Api.UnitTests;

public class AccountsControllerTests
{
    private readonly Mock<IAccountsRepository> AccountsRepositoryStub = new();
    private readonly Mock<ITransactionsRepository> TransactionsRepositoryStub = new();

    private readonly Random rand = new();

    [Fact]
    // public void UnitOfWork_StateUnderTest_ExpectedBehavior()
    public void GetAccount_WithUnexistingAccount_ReturnsNotFound()
    {
        // Arrange
        AccountsRepositoryStub.Setup(repo => repo.GetAccount(It.IsAny<int>())).Returns(null as Account);
        var controller = new AccountsController(AccountsRepositoryStub.Object, TransactionsRepositoryStub.Object);
        
        // Act
        var result = controller.GetAccount(1);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public void CreateTransaction_WithSufficientFunds_ReturnsCreatedTransaction()
    {
        //Arrange
        var fromAccount = CreateRandomAccount();
        fromAccount.AccountBalance = 1000;

        var toAccount = CreateRandomAccount();
        AccountsRepositoryStub.Setup(repo => repo.CreateAccount(fromAccount));
        AccountsRepositoryStub.Setup(repo => repo.CreateAccount(toAccount));

        var controller = new AccountsController(AccountsRepositoryStub.Object, TransactionsRepositoryStub.Object);
        var transaction = CreateTransactionDto(fromAccount.Id, toAccount.Id, 500);
        //Act
        var result = controller.CreateTransaction(transaction);

        //Assert
        Assert.IsType<TransactionDto>(result.Value);
        var dto = (result as ActionResult<TransactionDto>).Value;
        Assert.Equal("Success", dto.Status);
    }

    private static CreateTransactionDto CreateTransactionDto(int fromAccountId, int toAccountId, int amount)
    {
        return new()
        {
            FromAccountId = fromAccountId,
            ToAccountId = toAccountId,
            Amount = amount
        };
    }

    private Account CreateRandomAccount()
    {
        return new()
        {
            Id = rand.Next(1000),
            AccountName = Guid.NewGuid().ToString(),
            AccountBalance = rand.Next(1000)
        };
    }
}