using Features.Clients;

namespace Features.Tests
{
    public class ClientTests
    {
        [Fact(DisplayName = "New Client Valid")]
        [Trait("Category", "Client Trait Test")]
        public void Client_NewClient_MustBeValid()
        {
            //Arrange
            var client = new Client(
                Guid.NewGuid(),
                "Karen",
                "Melo",
                DateTime.Now.AddYears(-32),
                DateTime.Now,
                "karen@karenmelo.com",
                true);

            //Act
            var result = client.IsValid();

            //Assert
            Assert.True(result);
            Assert.Empty(client.ValidationResult.Errors);   
        }

        [Fact(DisplayName = "New Client Invalid")]
        [Trait("Category", "Client Trait Test")]
        public void Client_NewClient_MustBeInValid()
        {
            //Arrange
            var client = new Client(
                Guid.NewGuid(),
                "Karen",
                "Melo",
                DateTime.Now,
                DateTime.Now,
                "karen@karenmelo.com",
                true);

            //Act
            var result = client.IsValid();

            //Assert
            Assert.False(result);
            Assert.NotEmpty(client.ValidationResult.Errors);
        }
    }
}
