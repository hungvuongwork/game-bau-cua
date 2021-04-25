using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameBauCua.Web.Client.ViewModels
{
    public class BetResultViewModel
    {
        public int RoundPlayId { get; set; }

        public string PlayerId { get; set; }

        public long YourBet { get; set; }

        public long ResultBet { get; set; }
    }
}
