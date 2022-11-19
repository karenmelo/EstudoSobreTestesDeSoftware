namespace Features.Tests
{
    [CollectionDefinition(nameof(ClientTestsBogusCollection))]
    public class ClientBogusTests : IClassFixture<ClientTestsBogusFixture>
    {
        private readonly ClientTestsBogusFixture _clientTestsFixture;

        public ClientBogusTests(ClientTestsBogusFixture clientTestsBogusFixture)
        {
            _clientTestsFixture = clientTestsBogusFixture;
        }

        [Fact(DisplayName = "New Valid Client")]
        [Trait("Category", "Client Tests Bogus")]
        public void Client_NewClient_MustBeValid()
        {
            //Arrange
            var client = _clientTestsFixture.GenerateValidClient();

            //Act
            var result = client.IsValid();

            //Assert
            Assert.True(result);
            Assert.Equal(0, client.ValidationResult?.Errors.Count);
        }

        [Fact(DisplayName = "New Invalid Client")]
        [Trait("Category", "Client Tests Bogus")]
        public void Client_NewClient_MustBeInvalid()
        {
            //Arrange
            var client = _clientTestsFixture.GenerateInvalidClient();

            //Act
            var result = client.IsValid();

            //Assert
            Assert.False(result);
            Assert.NotEqual(0, client.ValidationResult?.Errors.Count);
        }
    }
}
