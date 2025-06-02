using AuditeventService.Models;

namespace AuditeventService.Data
{
    public interface IEventData
    {
        void AddEvent(AuditEvent eventData);
        IEnumerable<AuditEvent> GetEvents(string? serviceName, EventType? eventType, DateTime? from, DateTime? to, int page, int pageSize);
        AuditEvent? GetEventById(Guid id);
        List<string> ReplayEvents(List<Guid> eventIds);
    }
}
