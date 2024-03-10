using Newtonsoft.Json;

namespace Module.CrossCutting.Domain
{
    public class DomainEventBase : IDomainEvent
    {
        public DomainEventBase()
        {
            OccurredOn = DateTime.Now;
        }

        public DateTime OccurredOn { get; }
    }

    public abstract class DomainEventBase<T> : IIntegrationEvent<T> where T : IDomainEvent
    {
        public DomainEventBase(T integrationEvent)
        {
            Id = Guid.NewGuid();
            IntegrationEvent = integrationEvent;
        }

        [JsonIgnore] public T IntegrationEvent { get; }

        public Guid Id { get; }
    }
}