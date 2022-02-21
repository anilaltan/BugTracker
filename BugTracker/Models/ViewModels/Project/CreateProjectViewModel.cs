using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models.ViewModels.Project
{
    public class CreateProjectViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public bool Archive { get; set; }

    }
}
