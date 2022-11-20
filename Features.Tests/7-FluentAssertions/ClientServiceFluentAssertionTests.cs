using Features.Clients;
using FluentAssertions;
using FluentAssertions.Extensions;
using MediatR;
using Moq;

namespace Features.Tests
{
    [Collection(nameof(ClientTestsAutoMockerCollection))]
    public class ClientServiceFluentAssertionTests
    {
        private readonly ClientTestsAutoMockerFixture _clientTestsAutoMockerFixture;
        readonly ClientService _clientService;

        public ClientServiceFluentAssertionTests(ClientTestsAutoMockerFixture clientTestsAutoMockerFixture)
        {
            _clientTestsAutoMockerFixture = clientTestsAutoMockerFixture;
            _clientService = _clientTestsAutoMockerFixture.GetClientService();
        }

        [Fact(DisplayName = "Add Success Client")]
        [Trait("Category", "Client Service Fluent Assertion Tests")]
        public void ClientService_Add_MustSuccessExecute()
        {
            //Arrange
            var client = _clientTestsAutoMockerFixture.GenerateValidClient();

            //Act
            _clientService.Add(client);

            //Assert
            client.IsValid().Should().BeTrue();

            _clientTestsAutoMockerFixture.Mocker.GetMock<IClientRepository>().Verify(r => r.Add(client), Times.Once);
            _clientTestsAutoMockerFixture.Mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
        }

        [Fact(DisplayName = "Add Failed Client")]
        [Trait("Category", "Client Service Fluent Assertion Tests")]
        public void ClientService_Add_MustFailInvalidClient()
        {
            //Arrange
            var client = _clientTestsAutoMockerFixture.GenerateInvalidClient();

            //Act
            _clientService.Add(client);

            //Assert
            client.IsValid().Should().BeFalse();
            client.ValidationResult.Errors.Should().HaveCountGreaterThanOrEqualTo(1);

            _clientTestsAutoMockerFixture.Mocker.GetMock<IClientRepository>().Verify(r => r.Add(client), Times.Never);
            _clientTestsAutoMockerFixture.Mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Never);
        }

        [Fact(DisplayName = "Get Actives Clients")]
        [Trait("Category", "Client Service Fluent Assertion Tests")]
        public void ClientService_GetAllActives_MustReturnOnlyActivesClients()
        {
            //Arrange
            _clientTestsAutoMockerFixture.Mocker.GetMock<IClientRepository>()
                        .Setup(c => c.GetAll())
                        .Returns(_clientTestsAutoMockerFixture.GetVariedClients());

            //Act
            var clients = _clientService.GetAllActives();

            //Assert
            clients.Should().HaveCountGreaterThan(1).And.OnlyHaveUniqueItems();
            clients.Should().NotContain(c => !c.Active);

            _clientTestsAutoMockerFixture.Mocker.GetMock<IClientRepository>().Verify(r => r.GetAll(), Times.Once);


            // A utilizacao se da melhor no teste de INTEGRACAO
            _clientService.ExecutionTimeOf(c => c.GetAllActives())
                          .Should()
                          .BeLessThanOrEqualTo(50.Milliseconds(), "é executado milhares de vezes por segundo.");
        }
    }
}
