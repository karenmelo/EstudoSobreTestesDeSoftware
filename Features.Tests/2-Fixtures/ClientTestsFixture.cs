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
            var client = new Client(
                Guid.NewGuid(),
                "Karen",
                "Melo",
                DateTime.Now.AddYears(-32),
                DateTime.Now,
                "karen@karenmelo.com",
                true);

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
