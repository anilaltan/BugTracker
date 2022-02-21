using BugTracker.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BugTracker.Models.Domain;

namespace BugTracker.Seeds
{
    public static class DefaultTicketPriority
    {
        public static void SeedTicketPriority(IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.Migrate();
            if(!context.TicketPriorities.Any())
            {
                context.TicketPriorities.AddRange(new List<TicketPriority>()
                {
                    new TicketPriority() { Name = "High"},
                    new TicketPriority() { Name = "Medium"},
                    new TicketPriority() { Name = "Low"}
                });
            }
            context.SaveChanges();
        }
    }
}
