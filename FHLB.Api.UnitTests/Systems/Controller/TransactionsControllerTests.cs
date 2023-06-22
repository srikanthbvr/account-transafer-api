using FHLB.Api.Controllers;
using FHLB.Api.Dtos;
using FHLB.Api.Entities;
using FHLB.Api.Repositories;
using FHLB.Api.UnitTests.MockData;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FHLB.Api.UnitTests;

public class TransactionsControllerTests
{
  private readonly Mock<IAccountsRepository> AccountsRepositoryStub = new();
  private readonly Mock<ITransactionsRepository> TransactionsRepositoryStub = new();

  [Fact]
  public async Task CreateTransactionAsync_WithNonExistingAccount_ReturnsNotFound()
  {
    // Arrange
    CreateTransactionDto transactionDto = new()
    {
      FromAccountId = 10232,
      ToAccountId = 100,
      Amount = 10000
    };

    var controller = new TransactionsController(AccountsRepositoryStub.Object, TransactionsRepositoryStub.Object);

    // Act
    var result = await controller.CreateTransactionAsync(transactionDto);

    // Assert
    Assert.IsType<NotFoundResult>(result.Result);
  }

  [Fact]
  public async Task CreateTransactionAsync_WithExistingAccount_ReturnsSuccessTransaction()
  {
    // Arrange
    var accounts = AccountsMockData.GetAccounts();
    var fromAccount = accounts.ElementAt(0);
    var toAccount = accounts.ElementAt(1);

    AccountsRepositoryStub.Setup(repo => repo.GetAccountAsync(fromAccount.Id))
        .ReturnsAsync(fromAccount);
    AccountsRepositoryStub.Setup(repo => repo.GetAccountAsync(toAccount.Id))
        .ReturnsAsync(toAccount);

    CreateTransactionDto transactionDto = new()
    {
      FromAccountId = fromAccount.Id,
      ToAccountId = toAccount.Id,
      Amount = 500
    };

    var controller = new TransactionsController(AccountsRepositoryStub.Object, TransactionsRepositoryStub.Object);

    // Act
    var result = await controller.CreateTransactionAsync(transactionDto);

    // Assert
    Assert.IsType<CreatedAtActionResult>(result.Result);
    var actualTransaction = (result.Result as CreatedAtActionResult).Value as TransactionDto;
    Assert.Equal("Success", actualTransaction.Status);
  }

  [Fact]
  public async Task CreateTransactionAsync_WithExistingAccount_ReturnsBadRequest()
  {
    // Arrange
    var accounts = AccountsMockData.GetAccounts();
    var fromAccount = accounts.ElementAt(0);
    var toAccount = accounts.ElementAt(1);

    AccountsRepositoryStub.Setup(repo => repo.GetAccountAsync(fromAccount.Id))
        .ReturnsAsync(fromAccount);
    AccountsRepositoryStub.Setup(repo => repo.GetAccountAsync(toAccount.Id))
        .ReturnsAsync(toAccount);

    CreateTransactionDto transactionDto = new()
    {
      FromAccountId = fromAccount.Id,
      ToAccountId = toAccount.Id,
      Amount = 50000
    };

    var controller = new TransactionsController(AccountsRepositoryStub.Object, TransactionsRepositoryStub.Object);

    // Act
    var result = await controller.CreateTransactionAsync(transactionDto);

    // Assert
    Assert.IsType<BadRequestObjectResult>(result.Result);
  }
}
