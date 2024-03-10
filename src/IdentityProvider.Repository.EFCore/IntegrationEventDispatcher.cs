using Autofac;
using IdentityProvider.Repository.EFCore.EFDataContext;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Module.CrossCutting.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityProvider.Repository.EFCore
{

    public interface IDomainEvent : INotification
    {
        DateTime OccurredOn { get; }
    }

    public interface IIntegrationEvent<out TEventType> : IIntegrationEventNotification
    {
        TEventType IntegrationEvent { get; }
    }

    public interface IIntegrationEventNotification : INotification
    {
        Guid Id { get; }
    }

    public class IntegrationEventDispatcher : IDomainEventsDispatcher
    {
        private readonly AppDbContext _context;
        private readonly IMediator _mediator;
        private readonly ILifetimeScope _scope;

        public IntegrationEventDispatcher(IMediator mediator, DbContext context)
        {
            _mediator = mediator;
            _context = context as AppDbContext;
            _scope = CompositionRoot.BeginLifetimeScope();
        }

        public async Task DispatchEventsAsync()
        {
            var domainEntities = _context.ChangeTracker
                .Entries<DomainEntity<long>>()
                .Where(x => x.Entity.UncommittedDomainEvents != null && x.Entity.UncommittedDomainEvents.Any()).ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.UncommittedDomainEvents)
                .ToList();

            var integrationEvents = new List<IIntegrationEvent<IDomainEvent>>();

            foreach (var intEvent in domainEvents)
            {
                var integrationEventType = typeof(IIntegrationEvent<>);
                var integrationEventWithGenericType =
                    integrationEventType.MakeGenericType(intEvent.GetType());
                var integrationEvent = _scope.ResolveOptional(integrationEventWithGenericType, new List<NamedParameter>
            {
                new("integrationEvent", intEvent)
            });

                if (integrationEvent != null)
                    integrationEvents.Add(integrationEvent as IIntegrationEvent<IDomainEvent>);
            }

            domainEntities
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            var tasks = domainEvents
                .Select(async domainEvent => { await _mediator.Publish(domainEvent); });

            await Task.WhenAll(tasks);

            // 2PC problem is solved by using an outbox table as a queue
            // each and every event that needs to be published so other microservices can react to it
            // is first inserted into the local Db (the outbox table) as a single transaction
            // then, a background job (Quartz) will read rows from the outbox table and attempt publishing then unto a message queue (or a kafka topic)
            // this is the only way in which we can guarantee delivery (at least once delivery guarantee) without resorting to distributed transactions which are, in most cases, a no go
            foreach (var integrationEvent in integrationEvents)
            {
                var type = integrationEvent.GetType().FullName;
                var data = JsonConvert.SerializeObject(integrationEvent);
                var outboxMessage = new OutboxMessage(
                    integrationEvent.IntegrationEvent.OccurredOn,
                    type,
                    data);
                await _context.OutboxMessages.AddAsync(outboxMessage);
            }
        }
    }
}
