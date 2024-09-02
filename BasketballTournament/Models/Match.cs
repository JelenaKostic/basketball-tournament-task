using BasketballTournament.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketballTournament.Models
{
    public class Match
    {
        public NationalTeam FirstTeam { get; set; }
        public NationalTeam SecondTeam { get; set; }
        public int FirstTeamScore { get; set; }
        public int SecondTeamScore { get; set; }
        public RoundEnum? Round { get; set; }

        public override string ToString()
        {
            return $"{FirstTeam.Team} : {SecondTeam?.Team} ({FirstTeamScore}:{SecondTeamScore})";
        }
    }
}
