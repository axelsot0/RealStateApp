using Microsoft.AspNetCore.Identity;

namespace RealStateApp.Infraestructure.Identity.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
