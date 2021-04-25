using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameBauCua.Web.Client.ViewModels
{
    public class RoundPlayViewModel
    {
        public int Id { get; set; }

        public int RoundNumber { get; set; }

        public int MinimumBet { get; set; }

        public int MaximumBet { get; set; }

        public string WinMascots { get; set; }

        public bool IsFinished { get; set; }

        public string RoomId { get; set; }

        public IEnumerable<RoundPlayDetailViewModel> RoundPlayDetails { get; set; }
    }
}
