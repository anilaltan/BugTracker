namespace BugTracker.Models.ViewModels.Project
{
    public class ProjectsListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfMembers { get; set; }
        public int NumberOfTickets { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
