using Bogus;
using Bogus.DataSets;
using Features.Clients;

namespace Features.Tests
{
    [CollectionDefinition(nameof(ClientCollection))]
    public class ClientCollection : ICollectionFixture<ClientTestsFixture>
    {

    }

    public class ClientTestsFixture : IDisposable
    {
        public Client GenerateValidClient()
        {
            var gender = new Faker().PickRandom<Name.Gender>();
            //var email = new Faker().Internet.Email;
            //var clientFaker = new Faker<Client>();
            //clientFaker.RuleFor(c => c.Name, (f, c) => f.Name.FirstName());

            var client = new Faker<Client>("pt_BR").CustomInstantiator(f => new Client(
                Guid.NewGuid(),
                f.Name.FirstName(gender),
                f.Name.LastName(gender),
                f.Date.Past(80, DateTime.Now.AddYears(-18)),
                DateTime.Now,
                "",
                true
                )).RuleFor(c => c.Email, (f, c) => f.Internet.Email(c.Name.ToLower(), c.LastName.ToLower()));




            return client;
        }

        public Client GenerateInvalidClient()
        {
            var client = new Client(
              Guid.NewGuid(),
              "Karen",
              "Melo",
              DateTime.Now,
              DateTime.Now,
              "karen@karenmelo.com",
              true);

            return client;
        }

        public void Dispose()
        {
        }
    }
}
