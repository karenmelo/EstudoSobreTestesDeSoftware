namespace Features.Tests
{
    [CollectionDefinition(nameof(ClientCollection))]
    public class ClientInvalidTest : IClassFixture<ClientTestsFixture>
    {
        private readonly ClientTestsFixture _clientFixture;

        public ClientInvalidTest(ClientTestsFixture clientFixture)
        {
            _clientFixture = clientFixture;
        }

        [Fact(DisplayName = "[Fixtures] - New Invalid Client ")]
        [Trait("Category", "Client Trait Test")]
        public void Client_NewClient_MustBeInValid()
        {
            //Arrange
            var client = _clientFixture.GenerateInvalidClient();

            //Act
            var result = client.IsValid();

            //Assert
            Assert.False(result);
            Assert.NotEmpty(client.ValidationResult.Errors);
        }
    }
}
