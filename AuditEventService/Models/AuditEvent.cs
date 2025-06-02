using System.Text.Json.Serialization;

namespace AuditeventService.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    public class AuditEvent
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Timestamp is required.")]
        public DateTime Timestamp { get; set; }

        [Required(ErrorMessage = "Service name is required.")]
        [StringLength(100, ErrorMessage = "Service name cannot exceed 100 characters.")]
        public string ServiceName { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required(ErrorMessage = "Event type is required.")]
        public EventType EventType { get; set; }

        [Required(ErrorMessage = "Payload cannot be empty.")]
        public string Payload { get; set; }
    }


    public enum EventType
    {
        USER_LOGGED_IN,
        USER_LOGGED_OUT,
        FILE_UPLOADED,
        FILE_DELETED,
        DATA_MODIFIED,
        PERMISSION_CHANGED
    }

}
