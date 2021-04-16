using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GameBauCua.Web.Client.Data.Entities
{
    [Table("RoomDetails")]
    public class RoomDetail
    {
        [ForeignKey("Room")]
        public string RoomId { get; set; }

        public Room Room { get; set; }

        [ForeignKey("Player")]
        public string PlayerId { get; set; }

        public ApplicationUser Player { get; set; }

        public bool IsHost { get; set; }

        public string Color { get; set; }
    }
}
