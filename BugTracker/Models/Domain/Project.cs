using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models.Domain
{
    public class Project
    {
        public virtual List<IdentityUser> Users { get; set; }

        public Project(){ Users = new List<IdentityUser>();}
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Create Time")]
        public DateTime CreateTime { get; set; }
        public Boolean Archive { get; set; }
        public virtual List<Ticket> Ticket { get; set; }
    }
}
