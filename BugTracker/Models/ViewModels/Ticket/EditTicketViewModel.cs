using BugTracker.Models.Domain;
using BugTracker.Models.ViewModels.Project;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models.ViewModels.Ticket
{
    public class EditTicketViewModel
    {
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Description { get; set; }

        public DateTime? Updated { get; set; }

        [Required]
        public int ProjectId { get; set; }
        public List<ProjectIdName>? ProjectNameList { get; set; }

        //[Required]
        //public int TicketTypeId { get; set; }
        //public List<TicketType> TicketTypeNameList { get; set; }

        [Required]
        public int TicketPriorityId { get; set; }
        public List<TicketPriority>? TicketPriorityNameList { get; set; }

        [Required]
        public int TicketStatusId { get; set; }
        public List<TicketStatus>? TicketStatusNameList { get; set; }
    }
}
