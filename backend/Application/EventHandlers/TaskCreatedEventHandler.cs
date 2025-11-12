using EficazAPI.Application.Ports.AuditLogs;
using EficazAPI.Domain.Entities;
using EficazAPI.Domain.Events;
using EficazAPI.Domain.ValueObjects;

namespace EficazAPI.Application.EventHandlers
{
    public class TaskCreatedEventHandler : IDomainEventHandler<TaskCreatedEvent>
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IAuditUnitOfWork _unitOfWork;

        public TaskCreatedEventHandler(
            IAuditLogRepository auditLogRepository,
            IAuditUnitOfWork unitOfWork)
        {
            _auditLogRepository = auditLogRepository ?? throw new ArgumentNullException(nameof(auditLogRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task HandleAsync(TaskCreatedEvent domainEvent)
        {
            if (domainEvent == null)
                throw new ArgumentNullException(nameof(domainEvent));

            var action = AuditAction.TaskCreated;
            var auditLog = new AuditLog(
                domainEvent.TaskId,
                action,
                oldValue: null,
                newValue: $"Task '{domainEvent.Title}' created with priority score {domainEvent.PriorityScore:F2}",
                domainEvent.UserId);

            await _auditLogRepository.AddAsync(auditLog);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
