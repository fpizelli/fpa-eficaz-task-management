using EficazAPI.Domain.Events;

namespace EficazAPI.Application.EventHandlers
{
    public interface IDomainEventHandler<in TDomainEvent> where TDomainEvent : IDomainEvent
    {
        Task HandleAsync(TDomainEvent domainEvent);
    }
}
