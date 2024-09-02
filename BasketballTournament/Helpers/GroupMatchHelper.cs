using BasketballTournament.Common;
using BasketballTournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketballTournament.Helpers
{
    public class GroupMatchHelper
    {
        public List<Match> Matches = new();

        /// Simulate matches between all teams in group for all rounds
        public void SimulateGroupMatches(List<NationalTeam> teams)
        {
            Random random = new Random();
            var teamGroups = teams.GroupBy(x => x.Group).ToList();

            foreach (var group in teamGroups)
            {
                // Mix teams so that it cannot be predictable in which order teams will play in group
                var mixedTeams = group.ToList().OrderBy(x => random.Next()).ToList();

                for (var i = 0; i < mixedTeams.Count; i++)
                {
                    for (var j = i + 1; j < mixedTeams.Count; j++)
                    {
                        CommonHelper.CreateMatch(Matches, mixedTeams[i], mixedTeams[j], false);
                    }
                }
            }

            DefineRound();
            PrintRound();
        }

        /// Define round for created matches
        private void DefineRound()
        {
            var groupMatches = Matches.GroupBy(x => x.FirstTeam.Group).ToList();

            foreach (var groupMatch in groupMatches.ToList())
            {
                var round = RoundEnum.FirstRound;
                var groupMatchList = groupMatch.ToList();

                for (var i = 0; i < groupMatchList.Count; i++)
                {
                    groupMatchList[i].Round = round;

                    if (i < 2)
                    {
                        round++;
                    }
                    else if (i > 2)
                    {
                        round--;
                    }
                }
            }
        }

        private void PrintRound()
        {
            for (var i = RoundEnum.FirstRound; i < RoundEnum.QuarterFinals; i++)
            {
                Console.WriteLine($"\nGroup phase - {i.ToString()}:");
                var printCounter = 0;
                foreach (var match in Matches.Where(x => x.Round == i).OrderBy(x => x.FirstTeam.Group))
                {
                    if(printCounter % 2 == 0)  Console.WriteLine($"\tGroup {match.FirstTeam.Group}");
                    Console.WriteLine($"\t\t{match.ToString()}");
                    printCounter++;
                }
            }
        }
    }
}
