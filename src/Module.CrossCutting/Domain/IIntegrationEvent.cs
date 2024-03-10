using MediatR;

namespace Module.CrossCutting.Domain;
public interface IIntegrationEvent<out TEventType> : IIntegrationEventNotification
{
    TEventType IntegrationEvent { get; }
}

public interface IIntegrationEventNotification : INotification
{
    Guid Id { get; }
}