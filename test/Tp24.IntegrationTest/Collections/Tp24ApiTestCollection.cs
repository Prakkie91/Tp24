using Tp24.Api;
using Tp24.IntegrationTest.Fixtures;
using Xunit;

namespace Tp24.IntegrationTest.Collections;

[CollectionDefinition("Tp24 Api Test Collection")]
public class Tp24ApiTestCollection : ICollectionFixture<Tp24WebApplicationFactory<Program>>
{
}