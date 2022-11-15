namespace Features.Tests
{
    [CollectionDefinition(nameof(ClientCollection))]
    public class ClientValidTest : IClassFixture<ClientTestsFixture>
    {
        private readonly ClientTestsFixture _clientFixture;

        public ClientValidTest(ClientTestsFixture clientFixture)
        {
            _clientFixture = clientFixture;
        }

        [Fact(DisplayName = "[Fixtures] - New Valid Client ")]
        [Trait("Category", "Client Trait Test")]
        public void Client_NewClient_MustBeValid()
        {
            //Arrange
            var client = _clientFixture.GenerateValidClient();

            //Act
            var result = client.IsValid();

            //Assert
            Assert.True(result);
            Assert.Empty(client.ValidationResult.Errors);
        }
    }
}
