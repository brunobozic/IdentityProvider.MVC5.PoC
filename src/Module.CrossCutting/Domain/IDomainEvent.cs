using MediatR;

namespace Module.CrossCutting.Domain;
public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}