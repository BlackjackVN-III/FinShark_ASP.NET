using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeddySmith.API.Models
{
  
    public class AppUser : IdentityUser
    {
       public ICollection<Portfollo> Portfollos { get; set; } = new List<Portfollo>();
    }
}
