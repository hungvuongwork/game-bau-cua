using GameBauCua.Web.Client.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameBauCua.Web.Client.ViewModels
{
    public class RoomDetailViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int NumberOfPlayers { get; set; }

        public long MinimumBet { get; set; }

        public long ExpectedMaximumBet { get; set; }

        public bool IsFull { get; set; }

        public ApplicationUser Player { get; set; }
    }
}
