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

        //public IEnumerable<Client> GetAllActives()
        //{
        //    //return _clientRepository.GetAll().where(c => c.Active);
        //}
    }
}
