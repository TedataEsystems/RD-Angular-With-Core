
using Microsoft.AspNetCore.Identity;

namespace RD.Domain.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public int CorporationId { get; set; }

    }
}
