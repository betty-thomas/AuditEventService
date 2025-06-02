using AuditeventService.Data;
using AuditeventService.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuditEventService.Controllers
{
    
    [ApiController]
    [Route("api/events")]
    public class AuditEventController : ControllerBase
    {
        private readonly IEventData _eventData;

        public AuditEventController(IEventData eventData)
        {
            _eventData = eventData;
        }
        // POST /events → Store a new audit event
        [HttpPost]
        public IActionResult CreateEvent([FromBody] AuditEvent eventData)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            eventData.Id = Guid.NewGuid();
            eventData.Timestamp = DateTime.UtcNow;
            _eventData.AddEvent(eventData);

            return Ok(new { message = "Event created successfully", events = eventData });
        }

        // GET /events → Fetch stored events with optional filters + Paging
        [HttpGet]
        public IActionResult GetEvents([FromQuery] string? serviceName, [FromQuery] EventType? eventType, [FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var pagedEvents = _eventData.GetEvents(serviceName, eventType, from, to, page, pageSize);
            return Ok(new { page, pageSize, events = pagedEvents });
        }

        // GET /events/{id} → Get a single event by ID
        [HttpGet("{id}")]
        public IActionResult GetEventById(Guid id)
        {
            var auditEvent = _eventData.GetEventById(id);
            if (auditEvent == null)
                return NotFound(new { message = $"Event with ID {id} not found." });
            return auditEvent != null ? Ok(auditEvent) : NotFound($"Event with ID {id} not found.");
        }

        // POST /events/replay → Simulate replay of audit events
        [HttpPost("replay")]
        public IActionResult ReplayEvents([FromBody] List<Guid> eventIds)
        {
            if (eventIds == null || !eventIds.Any())
                return BadRequest("Event ID list cannot be empty.");

            var replayLogs = _eventData.ReplayEvents(eventIds);
            if (!replayLogs.Any())
                return NotFound("No matching events found.");
            return Ok(new { message = $"Replayed {replayLogs.Count} events.", logs = replayLogs });
        }
    }
}
