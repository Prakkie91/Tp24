using System.Net;
using System.Text;
using Newtonsoft.Json;
using Tp24.Application.Features.Receivable.Commands;
using Tp24.Application.Features.Receivable.Responses;
using Tp24.Common.Wrappers;
using Tp24.IntegrationTest.Fixtures;
using Tp24.IntegrationTest.Shared;
using Tp24API;
using Xunit;

namespace Tp24.IntegrationTest.IsolatedTests;

public class ReceivableControllerTests : ApiTestBase
{
    public ReceivableControllerTests(Tp24WebApplicationFactory<Program> webApplicationFactory) : base(webApplicationFactory)
    {
    }

    [Fact]
    public async Task GetReceivablesSummary_ReturnsExpectedData()
    {
        // Act
        var response = await Client.GetAsync("api/v1/receivable/summary");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonConvert.DeserializeObject<Result<ReceivableSummaryResponse>>(content);

        Assert.True(apiResponse is { Succeeded: true });
        Assert.Empty(apiResponse.Messages);

        // Further verify based on your models and expected data:
        var receivableSummary = apiResponse.Data;
        // Assertions on receivableSummary
    }

    [Theory]
    [InlineData("", "USD", "'Reference' must not be empty.")]
    [InlineData("ThisReferenceIsWayTooLongToBeAcceptedByTheValidationRulesAndShouldResultInAnError", "USD", "Reference should not exceed 50 characters.")]
    [InlineData("ValidRef", "", "Currency code should be exactly 3 characters long.")]
    [InlineData("ValidRef", "USDE", "Currency code should be exactly 3 characters long.")]
    public async Task AddReceivable_ValidationScenarios_ReturnsExpectedValidationErrors(
        string reference, string currencyCode, string expectedErrorMessage)
    {
        // Arrange
        var addReceivableCommand = new AddReceivablesCommand
        {
            Reference = reference,
            CurrencyCode = currencyCode,
            IssueDate = DateTime.Now,
            OpeningValue = 100m,
            PaidValue = 50m,
            DueDate = DateTime.Now.AddDays(10),
            Cancelled = false,
            DebtorName = "Test Debtor",
            DebtorReference = "TestRef",
            DebtorAddress1 = "123 Test Street",
            DebtorAddress2 = "Apt 4B",
            DebtorTown = "TestTown",
            DebtorState = "TestState",
            DebtorZip = "12345",
            DebtorCountryCode = "US",
            DebtorRegistrationNumber = "1234567890"
        };

        var json = JsonConvert.SerializeObject(addReceivableCommand);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PostAsync("api/v1/receivable", content);

        // Assert
        var responseContent = await response.Content.ReadAsStringAsync();
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var apiResponse = JsonConvert.DeserializeObject<Result>(responseContent);

        Assert.False(apiResponse.Succeeded);
        Assert.Contains(expectedErrorMessage, apiResponse.Messages);
    }

    
    [Fact]
    public async Task AddReceivable_GivenValidData_StoresReceivable()
    {
        // Arrange
        var addReceivableCommand = new AddReceivablesCommand
        {
            // Fill with sample data, could also be from a TestDataFactory
            // ...
        };
        var json = JsonConvert.SerializeObject(addReceivableCommand);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PostAsync("api/v1/receivable", content);

        // Assert

        var responseContent = await response.Content.ReadAsStringAsync();
        
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var apiResponse = JsonConvert.DeserializeObject<Result>(responseContent);

        Assert.True(apiResponse is { Succeeded: true });
        Assert.Empty(apiResponse.Messages);
    }
}
