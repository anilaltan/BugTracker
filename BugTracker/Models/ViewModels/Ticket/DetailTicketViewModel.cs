using BugTracker.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.Models.ViewModels
{
    public class DetailTicketViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        [Required]
        public string Project { get; set; }
        [Required]
        public string TicketPriority { get; set; }
        public string TicketStatus { get; set; }

        public string CreatedBy { get; set; }
        
    }

}