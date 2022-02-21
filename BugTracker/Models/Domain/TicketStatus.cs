using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models.Domain
{
    public class TicketStatus
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
