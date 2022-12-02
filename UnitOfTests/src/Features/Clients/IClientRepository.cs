using Features.Core;

namespace Features.Clients
{
    public interface IClientRepository : IRepository<Client>
    {

        Client GetToEmail(string email);
    }
}
