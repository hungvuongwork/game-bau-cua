using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameBauCua.Web.Client.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(128)]
        public string FullName { get; set; }

        [StringLength(1024)]
        public string Address { get; set; }

        [Required]
        public long YourCapital { get; set; }

        public IList<RoomDetail> RoomDetails { get; set; }

        public IList<RoundPlayDetail> RoundPlayDetails { get; set; }
    }
}
