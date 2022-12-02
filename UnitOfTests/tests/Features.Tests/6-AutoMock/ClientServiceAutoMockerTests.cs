using Features.Clients;
using MediatR;
using Moq;
using Moq.AutoMock;

namespace Features.Tests
{
    [Collection(nameof(ClientTestsBogusCollection))]
    public class ClientServiceAutoMockerTests
    {
        readonly ClientTestsBogusFixture _clientTestsFixture;

        public ClientServiceAutoMockerTests(ClientTestsBogusFixture clientTestsFixture)
        {
            _clientTestsFixture = clientTestsFixture;
        }

        [Fact(DisplayName = "Add Success Client")]
        [Trait("Category", "Client Service AutoMock Tests")]
        public void ClientService_Add_MustExecuteWithSuccess()
        {
            //Arrange
            var client = _clientTestsFixture.GenerateValidClient();
            var mocker = new AutoMocker();
            var clientService = mocker.CreateInstance<ClientService>();

            //Act
            clientService.Add(client);

            //Assert
            Assert.True(client.IsValid()); //apenas reafirmando a linha do act visto que, o metodo add valida o client
            mocker.GetMock<IClientRepository>().Verify(r => r.Add(client), Times.Once);
            mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);

        }

        [Fact(DisplayName = "Add Failed Client")]
        [Trait("Category", "Client Service AutoMock Tests")]
        public void ClientService_Add_MustFailBecauseInvalidClient()
        {
            //Arrange
            var client = _clientTestsFixture.GenerateInvalidClient();
            var mocker = new AutoMocker();
            var clientService = mocker.CreateInstance<ClientService>();

            //Act
            clientService.Add(client);

            //Assert
            Assert.False(client.IsValid()); //apenas reafirmando a linha do act visto que, o metodo add valida o client
            mocker.GetMock<IClientRepository>().Verify(r => r.Add(client), Times.Never);
            mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Never);
        }

        [Fact(DisplayName = "Get Actives Clients")]
        [Trait("Category", "Client Service AutoMock Tests")]
        public void ClientService_GetAllActives_MustReturnOnlyActivesClients()
        {
            //Arrange
            var mocker = new AutoMocker();
            var clientService = mocker.CreateInstance<ClientService>();

            mocker.GetMock<IClientRepository>().Setup(c => c.GetAll())
                      .Returns(_clientTestsFixture.GetVariedClients());

            //Act
            var clients = clientService.GetAllActives();

            //Assert
            mocker.GetMock<IClientRepository>().Verify(r => r.GetAll(), Times.Once);
            Assert.True(clients.Any());
            Assert.False(clients.Count(c => !c.Active) > 0);
        }

    }
}
