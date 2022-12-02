namespace Features.Clients
{
    public interface IClientService
    {
        IEnumerable<Client> GetAllActives();
        void Add(Client client);
        void Update(Client client);
        void Remove(Client client);
        void Disable(Client client);
    }
}
