using Microsoft.AspNetCore.Identity;

namespace MyApp.Entities.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Nombre { get; set; }
    }
}
