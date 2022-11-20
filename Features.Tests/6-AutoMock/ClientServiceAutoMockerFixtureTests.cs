using Features.Clients;
using MediatR;
using Moq;

namespace Features.Tests
{
    [Collection(nameof(ClientTestsAutoMockerCollection))]
    public class ClientServiceAutoMockerFixtureTests
    {
        private readonly ClientTestsAutoMockerFixture _clientTestsFixture;
        private ClientService _clientService;

        public ClientServiceAutoMockerFixtureTests(ClientTestsAutoMockerFixture clientTestsFixture)
        {
            _clientTestsFixture = clientTestsFixture;
            _clientService = _clientTestsFixture.GetClientService();
        }

        [Fact(DisplayName = "Add Success Client")]
        [Trait("Category", "Client Service AutoMockFixture Tests")]
        public void ClientService_Add_MustExecuteWithSuccess()
        {
            //Arrange
            var client = _clientTestsFixture.GenerateValidClient();

            //Act
            _clientService.Add(client);

            //Assert
            Assert.True(client.IsValid()); //apenas reafirmando a linha do act visto que, o metodo add valida o client
            _clientTestsFixture.Mocker.GetMock<IClientRepository>().Verify(r => r.Add(client), Times.Once);
            _clientTestsFixture.Mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);

        }

        [Fact(DisplayName = "Add Failed Client")]
        [Trait("Category", "Client Service AutoMockFixture Tests")]
        public void ClientService_Add_MustFailBecauseInvalidClient()
        {
            //Arrange
            var client = _clientTestsFixture.GenerateInvalidClient();

            //Act
            _clientService.Add(client);

            //Assert
            Assert.False(client.IsValid()); //apenas reafirmando a linha do act visto que, o metodo add valida o client
            _clientTestsFixture.Mocker.GetMock<IClientRepository>().Verify(r => r.Add(client), Times.Never);
            _clientTestsFixture.Mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Never);
        }

        [Fact(DisplayName = "Get Actives Clients")]
        [Trait("Category", "Client Service AutoMockFixture Tests")]
        public void ClientService_GetAllActives_MustReturnOnlyActivesClients()
        {
            //Arrange            
            _clientTestsFixture.Mocker.GetMock<IClientRepository>().Setup(c => c.GetAll())
                                      .Returns(_clientTestsFixture.GetVariedClients());

            //Act
            var clients = _clientService.GetAllActives();

            //Assert
            _clientTestsFixture.Mocker.GetMock<IClientRepository>().Verify(r => r.GetAll(), Times.Once);
            Assert.True(clients.Any());
            Assert.False(clients.Count(c => !c.Active) > 0);
        }

    }
}
