using System.Net;
using System.Text;
using Newtonsoft.Json;
using Tp24.Api;
using Tp24.Application.Features.Receivable.Commands;
using Tp24.Application.Features.Receivable.Responses;
using Tp24.Common.Wrappers;
using Tp24.IntegrationTest.Factories;
using Tp24.IntegrationTest.Fixtures;
using Tp24.IntegrationTest.Shared;
using Tp24.IntegrationTest.TestData.Profiles;
using Xunit;

namespace Tp24.IntegrationTest.IsolatedTests;

public class ReceivableControllerTests : ApiTestBase
{
    public ReceivableControllerTests(Tp24WebApplicationFactory<Program> webApplicationFactory) : base(
        webApplicationFactory)
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
    [InlineData("ThisReferenceIsWayTooLongToBeAcceptedByTheValidationRulesAndShouldResultInAnError", "USD",
        "Reference should not exceed 50 characters.")]
    [InlineData("ValidRef", "", "Currency code should be exactly 3 characters long.")]
    [InlineData("ValidRef", "USDE", "Currency code should be exactly 3 characters long.")]
    public async Task AddReceivable_ValidationScenarios_ReturnsExpectedValidationErrors(
        string reference, string currencyCode, string expectedErrorMessage)
    {
        var addReceivableCommand =
            ReceivableTestDataFactory.CreateAddReceivablesCommand(reference: reference, currencyCode: currencyCode);

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
        var addReceivableCommand = ReceivableTestDataFactory.CreateAddReceivablesCommand();

        var json = JsonConvert.SerializeObject(addReceivableCommand);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PostAsync("api/v1/receivable", content);

        // Assert
        var responseContent = await response.Content.ReadAsStringAsync();

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var apiResponse = JsonConvert.DeserializeObject<Result<Guid>>(responseContent);

        Assert.True(apiResponse is { Succeeded: true });
        Assert.NotEqual(apiResponse.Data, Guid.Empty);
        Assert.Equal("Receivable added successfully.", apiResponse.Messages.FirstOrDefault());
    }


    [Fact]
    public async Task AddReceivablesBatch_GivenValidData_BatchInsertsReceivables()
    {
        // Arrange
        var addReceivablesBatchCommand = ReceivableTestDataFactory.CreateAddReceivablesBatchCommand();

        var json = JsonConvert.SerializeObject(addReceivablesBatchCommand);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PostAsync("api/v1/receivable/batch", content);

        // Assert
        var responseContent = await response.Content.ReadAsStringAsync();

        var apiResponse = JsonConvert.DeserializeObject<Result<AddReceivablesBatchResponse>>(responseContent);

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        Assert.True(apiResponse is { Succeeded: true });

        Assert.NotNull(apiResponse.Data);
        Assert.Equal(0, apiResponse.Data.ExistingCount);
        Assert.Equal(0, apiResponse.Data.DuplicatesCount);
        Assert.Equal(addReceivablesBatchCommand.Receivables.Count, apiResponse.Data.SuccessCount);
        Assert.Equal(addReceivablesBatchCommand.Receivables.Count, apiResponse.Data.TotalProcessed);
    }


    [Fact]
    public async Task AddReceivablesBatch_WithDuplicateReferencesInSameBatch_FiltersOutDuplicates()
    {
        // Arrange
        var receivables = new List<ReceivableDto>
        {
            ReceivableTestDataFactory.CreateReceivableDto(reference: "D-1", debtorReference: "D-123"),
            ReceivableTestDataFactory.CreateReceivableDto(reference: "D-2", debtorReference: "D-456"),
            ReceivableTestDataFactory.CreateReceivableDto(reference: "D-1", debtorReference: "D-123")
        };

        var addReceivablesBatchCommand = new AddReceivablesBatchCommand
        {
            Receivables = receivables
        };

        var json = JsonConvert.SerializeObject(addReceivablesBatchCommand);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PostAsync("api/v1/receivable/batch", content);

        // Assert
        var responseContent = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonConvert.DeserializeObject<Result<AddReceivablesBatchResponse>>(responseContent);

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        Assert.True(apiResponse is { Succeeded: true });

        Assert.NotNull(apiResponse.Data);
        Assert.Equal(0, apiResponse.Data.ExistingCount);
        Assert.Equal(1, apiResponse.Data.DuplicatesCount);
        Assert.Equal(2, apiResponse.Data.SuccessCount);
        Assert.Equal(3, apiResponse.Data.TotalProcessed);
    }

    [Fact]
    public async Task AddReceivablesBatch_WithSomeReceivablesAlreadyInSystem_PreventsReInsertion()
    {
        // Arrange
        var receivables = new List<ReceivableDto>
        {
            ReceivableTestDataFactory.CreateReceivableDto(reference: "PRE-1", debtorReference: "PRE-123"),
            ReceivableTestDataFactory.CreateReceivableDto(
                reference: ReceivableTestDataSeed.ReceivableReferences.ReceivableReference2, debtorReference: "PRE-456")
        };

        var addReceivablesBatchCommand = new AddReceivablesBatchCommand
        {
            Receivables = receivables
        };

        var json = JsonConvert.SerializeObject(addReceivablesBatchCommand);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await Client.PostAsync("api/v1/receivable/batch", content);

        // Assert
        var responseContent = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonConvert.DeserializeObject<Result<AddReceivablesBatchResponse>>(responseContent);

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        Assert.True(apiResponse is { Succeeded: true });

        Assert.NotNull(apiResponse.Data);
        Assert.Equal(1, apiResponse.Data.ExistingCount);
        Assert.Equal(0, apiResponse.Data.DuplicatesCount);
        Assert.Equal(1, apiResponse.Data.SuccessCount);
        Assert.Equal(2, apiResponse.Data.TotalProcessed);
    }
}