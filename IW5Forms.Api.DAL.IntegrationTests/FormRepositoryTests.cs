namespace IW5Forms.Api.DAL.IntegrationTests;

public class FormRepositoryTests
{
    private readonly IDatabaseFixture dbFixture;

    public FormRepositoryTests()
    {
        dbFixture = new InMemoryDatabaseFixture();
    }

    [Fact]
    public void GetById_Returns_Requested_Form_Including_Questions()
    {
        // Arrange
    }
}