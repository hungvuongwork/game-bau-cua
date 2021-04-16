using GameBauCua.Web.Client.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameBauCua.Web.Client.ViewModels
{
    public class RoundPlayDetailViewModel
    {
        public int RoundPlayId { get; set; }

        public int MascotBet { get; set; }

        public long YourBet { get; set; }

        public string PlayerId { get; set; }

        public string PlayerFullName { get; set; }
    }
}
