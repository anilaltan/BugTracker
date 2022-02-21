using BugTracker.Models.Domain;
using BugTracker.Models.ViewModels.Project;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models.ViewModels.Ticket
{
    public class CreateTicketViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime Created { get; set; }

        [Required]
        public int ProjectId { get; set; }
        public List<ProjectIdName>? ProjectIdNameList { get; set; }

        [Required]
        public int TicketPriorityId { get; set; }
        public List<TicketPriority>? TicketPriorityNameList { get; set; }
    }
}
