using Store.Core.Messages;

namespace Store.Core.DomainObjects
{
    public abstract class Entity
    {
        public Guid Id { get; set; }

        private List<Event> _notifications;
        public IReadOnlyCollection<Event> Notifications => _notifications.AsReadOnly();

        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public void AddEvent(Event evento)
        {
            _notifications = _notifications ?? new List<Event>();
            _notifications.Add(evento);
        }

        public void RemoveEvent(Event eventItem)
        {
            _notifications?.Remove(eventItem);
        }

        public void ClearEvents()
        {
            _notifications?.Clear();
        }
    }
}
