using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Domain.Models
{
    /// <summary>
    /// Named ProjectTask to avoid confusion with the Task class in System.Threading.Tasks
    /// 
    public class ProjectTask
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Assignee { get; set; }
        public string Status { get; set; }

        [ConcurrencyCheck]
        public DateTime LastModified { get; set; }

        [Timestamp]
        public byte[] Version { get; set; } = Array.Empty<byte>();
    }
}
