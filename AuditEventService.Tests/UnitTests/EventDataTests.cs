using AuditeventService.Data;
using AuditeventService.Models;
namespace AuditEventService.Tests.UnitTests
{
    internal class EventDataTests
    {
        private EventData _eventData;

        [SetUp]
        public void Setup()
        {
            _eventData = new EventData();
        }

        [Test]
        public void AddEvent_ShouldStoreEvent()
        {
            var eventData = new AuditEvent
            {
                ServiceName = "TestService",
                EventType = EventType.USER_LOGGED_IN,
                Payload = "{}"
            };

            _eventData.AddEvent(eventData);
            var retrievedEvent = _eventData.GetEventById(eventData.Id);
            Assert.That(retrievedEvent, Is.Not.Null);
            Assert.That(retrievedEvent.ServiceName, Is.EqualTo(eventData.ServiceName));
        }

        [Test]
        public void GetEventById_ShouldReturnNull_IfNotExists()
        {
            var retrievedEvent = _eventData.GetEventById(Guid.NewGuid());
            Assert.That(retrievedEvent, Is.Null);
        }

        [Test]
        public void GetEvents_ShouldReturnFilteredResults()
        {
            var event1 = new AuditEvent { Id = Guid.NewGuid(), ServiceName = "AuthService", EventType = EventType.USER_LOGGED_IN, Payload = "{}" };
            var event2 = new AuditEvent { Id = Guid.NewGuid(), ServiceName = "PaymentService", EventType = EventType.USER_LOGGED_OUT, Payload = "{}" };

            _eventData.AddEvent(event1);
            _eventData.AddEvent(event2);

            var filteredEvents = _eventData.GetEvents("AuthService", null, null, null, 1, 10);
           
            Assert.That(filteredEvents.Count(), Is.EqualTo(1));
            Assert.That(filteredEvents.First().ServiceName, Is.EqualTo("AuthService"));
        }

        [Test]
        public void ReplayEvents_ShouldReturnLogs()
        {
            var event1 = new AuditEvent { ServiceName = "OrderService", EventType = EventType.USER_LOGGED_IN, Payload = "{}" };
            _eventData.AddEvent(event1);

            var replayLogs = _eventData.ReplayEvents(new List<Guid> { event1.Id });

            Assert.That(replayLogs.Count, Is.EqualTo(1));
            Assert.That(replayLogs[0], Does.Contain("Replaying event"));
        }
    }
}
