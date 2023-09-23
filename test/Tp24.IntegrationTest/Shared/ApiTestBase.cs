using Microsoft.AspNetCore.Mvc.Testing;
using Tp24.IntegrationTest.Fixtures;
using Tp24API;
using Xunit;

namespace Tp24.IntegrationTest.Shared;

[Collection("Tp24 Api Test Collection")]
public abstract class ApiTestBase
{
    protected readonly Tp24WebApplicationFactory<Program> _factory;

    public HttpClient Client;


    protected ApiTestBase(Tp24WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        Setup();
    }

    private void Setup()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        Client = client;
    }
}