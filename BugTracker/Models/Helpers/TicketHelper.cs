using BugTracker.Data;
using BugTracker.Models.Domain;
using System.Collections.Generic;

namespace BugTracker.Models.Helpers
{
    public class TicketHelper
    {
        private ApplicationDbContext _db;

        public TicketHelper(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<Ticket> GetAllTickets()
        {
            return _db.Tickets.Where(p => p.Project.Archive != true).ToList();
        }

        public List<Ticket> GetTicketsByOwnerId(string userId)
        {
            return _db.Tickets.Where(p => p.CreatedById == userId).Where(p => p.Project.Archive != true).ToList();
        }

        public List<TicketPriority> GetTicketPriorityNames()
        {
            return _db.TicketPriorities.ToList();
        }

        public List<TicketStatus> GetTicketStatusNames()
        {
            return _db.TicketStatuses.ToList();
        }

        public Ticket GetFirstTicketByTid(int? tId)
        {
            var result = _db.Tickets.Where(p => p.Id == tId);
            if (result.Count() == 0)
            {
                return null;
            }
            return result.First();
        }
    }
}
