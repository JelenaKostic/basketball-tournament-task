using BasketballTournament.Common;
using BasketballTournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketballTournament.Helpers
{
    public class HatHelper
    {
        public List<SemiFinals> GroupsForSemiFinals = new();

        #region hat
        /// Defining hat for each team depending on final ranking
        public void DefineHatForTeams(List<NationalTeam> teams)
        {
            Dictionary<char, List<int>> hatDict = new Dictionary<char, List<int>>
            {
                { 'D', new List<int> { 1, 2 } },
                { 'E', new List<int> { 3, 4 } },
                { 'F', new List<int> { 5, 6 } },
                { 'G', new List<int> { 7, 8 } }
            };

            // Order teams by final ranking and define hat for all of teams
            foreach (var hat in hatDict)
            {
                teams.Where(x => hat.Value.Contains(x.FinalRanking.GetValueOrDefault())).OrderBy(x => x.FinalRanking).ToList().ForEach(x => x.Hat = hat.Key);
            }

            PrintHats(teams);
        }


        /// Checking if teams already played together in group round
        private bool HaveTeamsPlayedTogether(List<Match> matches, NationalTeam firstTeam, NationalTeam secondTeam)
        {
            return matches.Where(x => (x.FirstTeam == firstTeam || x.SecondTeam == firstTeam)
                            && (x.FirstTeam == secondTeam || x.SecondTeam == secondTeam)).Any();
        }

        private void PrintHats(List<NationalTeam> teams)
        {
            Console.WriteLine($"Hats:");
            var printCounter = 0;
            foreach (var team in teams.Where(x => x.Hat.HasValue).OrderBy(x => x.Hat.Value).ThenBy(x => x.FinalRanking))
            {
                if(printCounter % 2 == 0) Console.WriteLine($"\tHat {team.Hat.Value}");
                Console.WriteLine("\t\t" + team.Team);
                printCounter++;
            }
        }

        #endregion

        #region quarterfinals

        /// Simulate matches between teams for quarter finals depending on hat
        public void SimulateQuarterFinalsMatches(List<Match> matches, List<NationalTeam> teams, List<List<char>> hatPairs)
        {
            var quarterFinalsPairs = new List<QuarterFinals>();

            for (var i = 0; i < hatPairs.Count; i++)
            {
                quarterFinalsPairs.AddRange(CreateQuarterFinalsPairs(matches, teams, hatPairs[i]));
            }

            // Create groups for semi finals based on groups for quarter finals
            CreateSemiFinalsPairs(quarterFinalsPairs, hatPairs);

            // Simulate quarter finals matches
            foreach (var pair in quarterFinalsPairs)
            {
                CommonHelper.CreateMatch(matches, pair.Teams[0], pair.Teams[1], true, RoundEnum.QuarterFinals);
            }

            PrintQuarterFinalsMatches(matches);
        }

        /// Create semi finals pairs based on groups for quarter finals
        /// Winners of defined team pairs will play together in semi finals
        private void CreateSemiFinalsPairs(List<QuarterFinals> quarterFinalsPairs, List<List<char>> hatPairs)
        {
            var random = new Random();

            // Mix quarter finals teams so  so that it cannot be predictable in which order teams will play
            var firstHatTeam = quarterFinalsPairs.Where(x => x.HatPair == hatPairs[0]).OrderBy(x => random.Next()).ToList();
            var secondHatTeam = quarterFinalsPairs.Where(x => x.HatPair == hatPairs[1]).OrderBy(x => random.Next()).ToList();

            for (var i = 0; i < firstHatTeam.Count; i++)
            {
                GroupsForSemiFinals.Add(new SemiFinals() { FirstHatPair = firstHatTeam[i].Teams, SecondHatPair = secondHatTeam[i].Teams });
            }
        }

        /// Creating pairs for quarter finals with checking that teams didn't play together in group phase
        private List<QuarterFinals> CreateQuarterFinalsPairs(List<Match> matches, List<NationalTeam> teams, List<char> hatPair)
        {
            Random random = new Random();

            var firstHatTeam = teams.Where(x => x.Hat.HasValue && x.Hat.Value == hatPair[0]).OrderBy(x => random.Next()).ToList();
            var secondHatTeam = teams.Where(x => x.Hat.HasValue && x.Hat.Value == hatPair[1]).OrderBy(x => random.Next()).ToList();

            if (!HaveTeamsPlayedTogether(matches, firstHatTeam[0], secondHatTeam[0]))
            {
                if (!HaveTeamsPlayedTogether(matches, firstHatTeam[1], secondHatTeam[1]))
                {
                    return new List<QuarterFinals>() 
                    { 
                        new QuarterFinals() { HatPair = hatPair, Teams = new List<NationalTeam> { firstHatTeam[0], secondHatTeam[0] } },
                        new QuarterFinals() { HatPair = hatPair, Teams = new List<NationalTeam> { firstHatTeam[1], secondHatTeam[1] } }
                    };
                }
            }

            return new List<QuarterFinals>()
            {
                new QuarterFinals() { HatPair = hatPair, Teams = new List<NationalTeam> { firstHatTeam[0], secondHatTeam[1] } },
                new QuarterFinals() { HatPair = hatPair, Teams = new List<NationalTeam> { firstHatTeam[1], secondHatTeam[0] } }
            };
        }

        private void PrintQuarterFinalsMatches(List<Match> matches)
        {
            Console.WriteLine($"\nQuarter Finals:");
            foreach (var match in matches.Where(x => x.Round > RoundEnum.ThirdRound).OrderBy(x => x.FirstTeam.Hat))
            {
                Console.WriteLine("\t" + match.ToString());
            }
        }

        #endregion

        #region semifinals

        /// Simulate semi finals matches between teams
        public void SimulateSemiFinalsMatches(List<Match> matches, List<NationalTeam> teams)
        {
            foreach(var pair in GroupsForSemiFinals)
            {
                CommonHelper.CreateMatch(matches, pair.FirstHatPair.Where(x => x.InGame == true).First(), pair.SecondHatPair.Where(x => x.InGame == true).First(), true, RoundEnum.SemiFinals);
            }

            PrintSemiFinalsMatches(matches);
        }

        private void PrintSemiFinalsMatches(List<Match> matches)
        {
            Console.WriteLine($"\nSemi Finals:");
            foreach (var match in matches.Where(x => x.Round > RoundEnum.QuarterFinals))
            {
                Console.WriteLine("\t" + match.ToString());
            }
        }

        #endregion

        #region third place match

        /// Simulate match for third place
        public void SimulateThirdPlaceMatch(List<Match> matches, List<NationalTeam> teams)
        {
            // Getting semi finals matches
            var semiFinalsMatches = matches.Where(x => x.Round == RoundEnum.SemiFinals).ToList();

            // Getting teams that lost in semi finals matches
            var losingTeams = semiFinalsMatches.Select(x => x.FirstTeamScore < x.SecondTeamScore ? x.FirstTeam : x.SecondTeam).ToList();

            CommonHelper.CreateMatch(matches, losingTeams[0], losingTeams[1], true, RoundEnum.ThirdPlace);

            losingTeams.ForEach(x => x.InGame = false);

            PrintThirdPlacePhasesMatch(matches);
        }

        private void PrintThirdPlacePhasesMatch(List<Match> matches)
        {
            Console.WriteLine($"\nThird Place Match:");
            var match = matches.Where(x => x.Round > RoundEnum.SemiFinals).First();
            Console.WriteLine("\t" + match.ToString());
        }

        #endregion

        #region finals

        /// Simulate final match
        public void SimulateFinalsMatches(List<Match> matches, List<NationalTeam> teams)
        {
            // Getting teams that are still in the game
            var winnerTeams = teams.Where(x => x.InGame).ToList();

            CommonHelper.CreateMatch(matches, winnerTeams[0], winnerTeams[1], true, RoundEnum.Finals);

            PrintFinalsMatches(matches);
        }

        private void PrintFinalsMatches(List<Match> matches)
        {
            Console.WriteLine($"\nFinals:");
            var finalMatch = matches.Where(x => x.Round > RoundEnum.ThirdPlace).First();
            Console.WriteLine("\t" + finalMatch.ToString());


            Console.WriteLine($"\nMedal:");
            var thirdPlaceMatch = matches.Where(x => x.Round == RoundEnum.ThirdPlace).First();

            Console.WriteLine($"\t 1. {(finalMatch.FirstTeamScore > finalMatch.SecondTeamScore ? finalMatch.FirstTeam.Team : finalMatch.SecondTeam.Team)}");
            Console.WriteLine($"\t 2. {(finalMatch.FirstTeamScore > finalMatch.SecondTeamScore ? finalMatch.SecondTeam.Team : finalMatch.FirstTeam.Team)}");
            Console.WriteLine($"\t 3. {(thirdPlaceMatch.FirstTeamScore > thirdPlaceMatch.SecondTeamScore ? thirdPlaceMatch.FirstTeam.Team : thirdPlaceMatch.SecondTeam.Team)}");
        }

        #endregion
    }
}
