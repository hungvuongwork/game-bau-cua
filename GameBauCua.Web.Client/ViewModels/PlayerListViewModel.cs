using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameBauCua.Web.Client.ViewModels
{
    public class PlayerListViewModel
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public bool IsHost { get; set; }

        public long YourCapital { get; set; }

        public string Color { get; set; }
    }
}
