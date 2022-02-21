using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.Models.Domain
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Create Time")]
        public DateTime Created { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Update Time")]
        public DateTime? Updated { get; set; }

        public virtual IdentityUser CreatedBy { get; set; }
        [Required]
        public string CreatedById { get; set; }

        public virtual TicketPriority TicketPriority { get; set; }
        [Required, Display(Name = "Ticket Priority")]
        //[ForeignKey("TicketPriority")]
        public int TicketPriorityId { get; set; }

        public virtual TicketStatus TicketStatus { get; set; }
        [Required, Display(Name = "Ticket Status")]
        //[ForeignKey("TicketStatus")]
        public int TicketStatusId { get; set; }

        public virtual Project Project { get; set; }
        [Required, Display(Name = "Project")]
        //[ForeignKey("Project")]
        public int ProjectId { get; set; }
   
    }
}
