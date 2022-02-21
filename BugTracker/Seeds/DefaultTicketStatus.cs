using Microsoft.EntityFrameworkCore;
using BugTracker.Models.Domain;
using BugTracker.Data;

namespace BugTracker.Seeds
{
    public static class DefaultTicketStatus
    {
        public static void SeedTicketStatus(IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.Migrate();
            if (context.TicketStatuses.Count() == 0)
            {
                context.TicketStatuses.AddRange(new List<TicketStatus>()
                {
                    new TicketStatus() { Name = "Open"},
                    new TicketStatus() { Name = "In Progress"},
                    new TicketStatus() { Name = "Close"}
                });
            }
            context.SaveChanges();
        }
    }
}
