using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GameBauCua.Web.Client.Data.Entities
{
    [Table("RoundPlays")]
    public class RoundPlay
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int RoundNumber { get; set; }

        [Required]
        public int MinimumBet { get; set; }

        [Required]
        public int MaximumBet { get; set; }

        public string WinMascots { get; set; }

        public bool IsWaitingPlayerReady { get; set; }

        [ForeignKey("Room")]
        public string RoomId { get; set; }

        public Room Room { get; set; }

        public IList<RoundPlayDetail> RoundPlayDetails { get; set; }
    }
}
