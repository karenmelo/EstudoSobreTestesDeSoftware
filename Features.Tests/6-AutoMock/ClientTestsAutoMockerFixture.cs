using Bogus;
using Bogus.DataSets;
using Features.Clients;
using Moq.AutoMock;

namespace Features.Tests
{
    [CollectionDefinition(nameof(ClientTestsAutoMockerCollection))]
    public class ClientTestsAutoMockerCollection : ICollectionFixture<ClientTestsAutoMockerFixture>
    { }

    public class ClientTestsAutoMockerFixture : IDisposable
    {
        public ClientService ClientService;
        public AutoMocker Mocker;

        public Client GenerateValidClient()
        {
            return GenerateClients(1, true).FirstOrDefault();
        }

        public IEnumerable<Client> GetVariedClients()
        {
            var clients = new List<Client>();

            clients.AddRange(GenerateClients(50, true).ToList());
            clients.AddRange(GenerateClients(50, false).ToList());

            return clients;
        }

        private IEnumerable<Client> GenerateClients(int quantity, bool active)
        {
            var gender = new Faker().PickRandom<Name.Gender>();

            //var email = new Faker().Internet.Email("karen", "melo", "hotmail");
            //var clientFaker = new Faker<Client>();
            //clientFaker.RuleFor(c => c.Name, (f, c) => f.Name.FirstName());

            var clients = new Faker<Client>()
            .CustomInstantiator(f => new Client(
                Guid.NewGuid(),
                f.Name.FirstName(gender),
                f.Name.LastName(gender),
                f.Date.Past(80, DateTime.Now.AddYears(-18)),
                DateTime.Now,
                "",
                active
                ))
            .RuleFor(c => c.Email, (f, c) => f.Internet.Email(c.Name.ToLower(), c.LastName.ToLower()));

            return clients.Generate(quantity);
        }

        public Client GenerateInvalidClient()
        {
            var gender = new Faker().PickRandom<Name.Gender>();

            var client = new Faker<Client>()
                .CustomInstantiator(f => new Client(
                    Guid.NewGuid(),
                    f.Name.FirstName(gender),
                    f.Name.LastName(gender),
                    f.Date.Past(1, DateTime.Now.AddYears(1)),
                    DateTime.Now,
                    "",
                    false
                    ));

            return client;
        }

        public ClientService GetClientService()
        {
            Mocker = new AutoMocker();
            ClientService = Mocker.CreateInstance<ClientService>();

            return ClientService;
        }

        public void Dispose()
        {
        }
    }

}
