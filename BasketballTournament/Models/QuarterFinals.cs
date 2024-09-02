using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketballTournament.Models
{
    public class QuarterFinals
    {
        public List<Char> HatPair { get; set; }
        public List<NationalTeam> Teams { get; set; } = new();
    }
}
