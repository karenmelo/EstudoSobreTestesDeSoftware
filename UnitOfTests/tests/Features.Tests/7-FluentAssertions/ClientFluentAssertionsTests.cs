using FluentAssertions;
using Xunit.Abstractions;

namespace Features.Tests
{
    [Collection(nameof(ClientTestsAutoMockerCollection))]
    public class ClientFluentAssertionsTests
    {
        private readonly ClientTestsAutoMockerFixture _clientTestsAutoMockerFixture;
        private readonly ITestOutputHelper _outputHelper;

        public ClientFluentAssertionsTests(ClientTestsAutoMockerFixture clientTestsAutoMockerFixture, ITestOutputHelper outputHelper)
        {
            _clientTestsAutoMockerFixture = clientTestsAutoMockerFixture;
            _outputHelper = outputHelper;
        }

        [Fact(DisplayName = "New Valid Client")]
        [Trait("Category", "Client Fluent Assertion Tests")]
        public void Client_NewClient_MustBeValid()
        {
            //Arrange
            var client = _clientTestsAutoMockerFixture.GenerateValidClient();

            //Act
            var result = client.IsValid();

            //Assert
            //Assert.True(result);
            //Assert.Equal(0, client.ValidationResult.Errors.Count);

            //Assert
            result.Should().BeTrue();
            client.ValidationResult.Errors.Should().HaveCount(0);
        }

        [Fact(DisplayName = "New Invalid Client")]
        [Trait("Category", "Client Fluent Assertion Tests")]
        public void Client_NewClient_MustBeInvalid()
        {
            //Arrange
            var client = _clientTestsAutoMockerFixture.GenerateInvalidClient();

            //Act
            var result = client.IsValid();


            //Assert
            result.Should().BeFalse();
            client.ValidationResult.Errors.Should().HaveCountGreaterThanOrEqualTo(1, "Deve possuir erros de validacao.");
            _outputHelper.WriteLine($"Foram encontrados {client.ValidationResult.Errors.Count} erros nesta validacao.");
        }

    }
}
