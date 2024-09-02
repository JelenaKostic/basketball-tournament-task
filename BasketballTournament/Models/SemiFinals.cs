using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketballTournament.Models
{
    public class SemiFinals
    {
        public List<NationalTeam> FirstHatPair { get; set; } = new();
        public List<NationalTeam> SecondHatPair { get; set; } = new();
    }
}
