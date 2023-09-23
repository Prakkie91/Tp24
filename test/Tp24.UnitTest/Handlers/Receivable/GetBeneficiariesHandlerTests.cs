using Moq;
using Tp24.Application.Features.Receivable.Handlers;
using Tp24.Application.Features.Receivable.Queries;
using Tp24.Core.Interfaces.Repositories;
using Tp24.UnitTest.Factories;
using Xunit;

namespace Tp24.UnitTest.Handlers.Receivable;

[Collection("Handler Collection")]
public class GetReceivableSummaryQueryHandlerTests
{
    private readonly GetReceivableSummaryQueryHandler _handler;
    private readonly Mock<IReceivableRepository> _receivableRepositoryMock;

    public GetReceivableSummaryQueryHandlerTests()
    {
        _receivableRepositoryMock = new Mock<IReceivableRepository>();
        _handler = new GetReceivableSummaryQueryHandler(_receivableRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_SuccessfullyFetchesAndMapsReceivableSummary()
    {
        // Arrange
        var summary = ReceivableTestDataFactory.CreateReceivablesSummary();
        _receivableRepositoryMock.Setup(r => r.GetReceivablesSummaryAsync()).ReturnsAsync(summary);

        // Act
        var result = await _handler.Handle(new GetReceivableSummaryQuery(), CancellationToken.None);

        // Assert
        Assert.True(result.Succeeded);
        var response = result.Data;
        Assert.Equal(summary.TotalReceivables, response.TotalReceivables);
        Assert.Equal(summary.OpenInvoiceCount, response.OpenInvoiceCount);
        Assert.Equal(summary.ClosedInvoiceCount, response.ClosedInvoiceCount);
        Assert.Equal(summary.TotalOpeningValue, response.TotalOpeningValue);
    }
}