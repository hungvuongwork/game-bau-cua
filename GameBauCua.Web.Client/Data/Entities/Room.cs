using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GameBauCua.Web.Client.Data.Entities
{
    [Table("Rooms")]
    public class Room
    {
        [Key]
        [StringLength(64)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        public int NumberOfPlayers { get; set; }

        [Required]
        public long MinimumBet { get; set; }

        [Required]
        public long ExpectedMaximumBet { get; set; }

        public bool IsFull { get; set; }

        public ICollection<RoundPlay> RoundPlays { get; set; }

        public IList<RoomDetail> RoomDetails { get; set; }
    }
}
