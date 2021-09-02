using Microsoft.AspNetCore.Identity;

namespace UniversitySystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
