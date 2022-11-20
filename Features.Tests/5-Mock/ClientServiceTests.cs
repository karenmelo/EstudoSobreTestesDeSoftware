using Features.Clients;
using MediatR;
using Moq;

namespace Features.Tests
{
    [Collection(nameof(ClientTestsBogusCollection))]
    public class ClientServiceTests
    {
        readonly ClientTestsBogusFixture _clientTestsFixture;

        public ClientServiceTests(ClientTestsBogusFixture clientTestsFixture)
        {
            _clientTestsFixture = clientTestsFixture;
        }

        [Fact(DisplayName = "Add Success Client")]
        [Trait("Category", "Client Service Mock Tests")]
        public void ClientService_Add_MustExecuteWithSuccess()
        {
            //Arrange
            var client = _clientTestsFixture.GenerateValidClient();
            var clientRepo = new Mock<IClientRepository>();
            var mediatr = new Mock<IMediator>();
            var clientService = new ClientService(clientRepo.Object, mediatr.Object);

            //Act
            clientService.Add(client);

            //Assert
            Assert.True(client.IsValid()); //apenas reafirmando a linha do act visto que, o metodo add valida o client
            clientRepo.Verify(r => r.Add(client), Times.Once);
            mediatr.Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);

        }

        [Fact(DisplayName = "Add Failed Client")]
        [Trait("Category", "Client Service Mock Tests")]
        public void ClientService_Add_MustFailBecauseInvalidClient()
        {
            //Arrange
            var client = _clientTestsFixture.GenerateInvalidClient();
            var clientRepo = new Mock<IClientRepository>();
            var mediatr = new Mock<IMediator>();
            var clientService = new ClientService(clientRepo.Object, mediatr.Object);

            //Act
            clientService.Add(client);

            //Assert
            Assert.False(client.IsValid()); //apenas reafirmando a linha do act visto que, o metodo add valida o client
            clientRepo.Verify(r => r.Add(client), Times.Never);
            mediatr.Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Never);
        }

        [Fact(DisplayName = "Get Actives Clients")]
        [Trait("Category", "Client Service Mock Tests")]
        public void ClientService_GetAllActives_MustReturnOnlyActivesClients()
        {
            //Arrange
            var clientRepo = new Mock<IClientRepository>();
            var mediatr = new Mock<IMediator>();

            clientRepo.Setup(c => c.GetAll())
                      .Returns(_clientTestsFixture.GetVariedClients());

            var clientService = new ClientService(clientRepo.Object, mediatr.Object);

            //Act
            var clients = clientService.GetAllActives();

            //Assert
            clientRepo.Verify(r => r.GetAll(), Times.Once);
            Assert.True(clients.Any());
            Assert.False(clients.Count(c => !c.Active) > 0);
        }

    }
}
