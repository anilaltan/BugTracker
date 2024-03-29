﻿namespace BugTracker.Models
{
    public class DashboardViewModel
    {
        public int NumberOfProjects { get; set; }
        public int NumberOfTickets { get; set; }
        public int NumberOfTicketsOpen { get; set; }
        public int NumberOfTicketsResolved { get; set; }
        public int NumberOfTicketsRejected { get; set; }
        public int NumberOfTicketsCreated { get; set; }
        public int NumberOfTicketsAssigned { get; set; }
    }
}
