using MediatR;

namespace Features.Clients
{
    public  class ClientEmailNotification : INotification
    {
        public string Origin { get; private set; }
        public string Destiny { get; private set; }
        public string Subject { get; private set; }
        public string Message { get; private set; }

        public ClientEmailNotification(string origin, string destiny, string subject, string message)
        {
            Origin = origin;
            Destiny = destiny;
            Subject = subject;
            Message = message;
        }
    }
}
