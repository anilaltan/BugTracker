﻿namespace BugTracker.Models.ViewModels.Ticket
{
    public class TicketsListViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        public string? Project { get; set; }
        public string? TicketStatus { get; set; }
        public string? TicketPriority { get; set; }

        public string CreatedBy { get; set; }
    }
}
