using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GameBauCua.Web.Client.Data.Entities
{
    [Table("RoundPlayDetails")]
    public class RoundPlayDetail
    {
        [ForeignKey("RoundPlay")]
        public int RoundPlayId { get; set; }

        public RoundPlay RoundPlay { get; set; }

        [ForeignKey("Player")]
        public string PlayerId { get; set; }

        public ApplicationUser Player { get; set; }

        public int MascotBet { get; set; }

        public long YourBet { get; set; }
    }
}
