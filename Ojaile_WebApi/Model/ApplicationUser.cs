using Microsoft.AspNetCore.Identity;

namespace Ojaile_WebApi.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int Institution { get; set; }
        public DateTime Created { get; set; }
        public string? ProductName { get; set; }
    }
}
