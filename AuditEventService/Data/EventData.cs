namespace AuditeventService.Data
{
    using System.Collections.Concurrent;
    using AuditeventService.Models;

    public class EventData : IEventData
    {
        private readonly ConcurrentDictionary<Guid, AuditEvent> _events = new();

        public void AddEvent(AuditEvent eventData) => _events[eventData.Id] = eventData;

        public IEnumerable<AuditEvent> GetEvents(string? serviceName, EventType? eventType, DateTime? from, DateTime? to, int page, int pageSize)
        {
            var query = _events.Values.AsEnumerable();

            if (!string.IsNullOrEmpty(serviceName))
                query = query.Where(e => e.ServiceName == serviceName);

            if (eventType.HasValue)
                query = query.Where(e => e.EventType == eventType.Value);

            if (from.HasValue)
                query = query.Where(e => e.Timestamp >= from.Value);

            if (to.HasValue)
                query = query.Where(e => e.Timestamp <= to.Value);

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public AuditEvent? GetEventById(Guid id) => _events.TryGetValue(id, out var auditEvent) ? auditEvent : null;

        public List<string> ReplayEvents(List<Guid> eventIds)
        {
            var eventsToReplay = _events.Values.Where(e => eventIds.Contains(e.Id)).ToList();
            List<string> replayLogs = new();

            foreach (var evt in eventsToReplay)
            {
                string logMessage = $"Replaying event: {evt.Id} | Type: {evt.EventType} | Payload: {evt.Payload}";
                Console.WriteLine(logMessage);
                replayLogs.Add(logMessage);
            }

            return replayLogs;
        }
    }

}
