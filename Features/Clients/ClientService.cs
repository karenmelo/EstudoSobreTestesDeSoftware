using MediatR;

namespace Features.Clients
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMediator _mediator;

        public ClientService(IClientRepository clientRepository, IMediator mediator)
        {
            _clientRepository = clientRepository;
            _mediator = mediator;
        }

        public IEnumerable<Client> GetAllActives()
        {
            return _clientRepository.GetAll().Where(c => c.Active);
        }

        public void Add(Client client) {
            if (!client.IsValid())
                return;
            _clientRepository.Add(client);
            _mediator.Publish(new ClientEmailNotification("admin@me.com", client.Email, "Ola", "Bem-vindo!"));
        }

        public void Update(Client client) { 
            if(!client.IsValid())
                return ;
            _clientRepository.Update(client);
            _mediator.Publish(new ClientEmailNotification("admin@me.com", client.Email, "Mudancas", "De uma olhada!"));
        }

        public void Disable(Client client)
        {
            if(!client.IsValid())
                return;

            client.Disable();
            _clientRepository.Update(client);
            _mediator.Publish(new ClientEmailNotification("admin@me.com", client.Email, "Ate breve!", "Ate mais tarde!"));
        }

        public void Remove(Client client) {
            _clientRepository.Remove(client.Id);
            _mediator.Publish(new ClientEmailNotification("admin@me.com", client.Email, "Adeus", "Tenha uma boa jornada!"));
        }

        public void Dispose()
        {
            _clientRepository.Dispose();
        }
    }
}
