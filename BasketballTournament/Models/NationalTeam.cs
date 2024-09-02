using BasketballTournament.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BasketballTournament.Models
{
    public class NationalTeam
    {
        public string Team { get; set; }
        public string ISOCode { get; set; }
        public int FIBARanking { get; set; }
        public char Group { get; set; }
        public char? Hat { get; set; }
        public int Score { get; set; }
        public int ScoredPoints { get; set; }
        public int ConcededPoints { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int PointDifference => ScoredPoints - ConcededPoints;
        public int? GroupRanking { get; set; }
        public int? FinalRanking { get; set; }
        public int? MedalScore { get; set; }
        public bool InGame { get; set; } = true;

        public bool IsHigherFIBARanked(NationalTeam nationalTeam)
        {
            return this.FIBARanking > nationalTeam.FIBARanking;
        }

        public bool HasMoreWinnings(NationalTeam nationalTeam)
        {
            return this.Wins > nationalTeam.Wins;
        }

        public (NationalTeam, NationalTeam) OrderTeams(List<Match> matches, NationalTeam nationalTeam)
        {
            var match = CommonHelper.FindMatchOfTwoTeams(matches, this, nationalTeam);
            return match.FirstTeamScore > match.SecondTeamScore ? (match.FirstTeam, match.SecondTeam) : (match.SecondTeam, match.FirstTeam);
        }

        public List<NationalTeam> OrderTeams(List<Match> matches, NationalTeam firstTeam, NationalTeam secondTeam)
        {
            var teamMatches = CommonHelper.FindMatchesOfThreeTeams(matches, this, firstTeam, secondTeam);

            var thisTeamMatches = teamMatches.Where(x => x.FirstTeam == this || x.SecondTeam == this).ToList();
            var thisTeamScore = this.CalculateTeamScore(thisTeamMatches);

            var firstTeamMatches = teamMatches.Where(x => x.FirstTeam == firstTeam || x.SecondTeam == firstTeam).ToList();
            var firstTeamScore = firstTeam.CalculateTeamScore(firstTeamMatches);

            var secondTeamMatches = teamMatches.Where(x => x.FirstTeam == secondTeam || x.SecondTeam == secondTeam).ToList();
            var secondTeamScore = secondTeam.CalculateTeamScore(secondTeamMatches);

            var items = new List<(int, NationalTeam)>
            {
                (thisTeamScore, this),
                (firstTeamScore, firstTeam),
                (secondTeamScore, secondTeam)
            };

            var orderedTeams = items.OrderByDescending(item => item.Item1).Select(x => x.Item2).ToList();
            return orderedTeams;
        }

        public int CalculateTeamScore(List<Match> teamMatches)
        {
            var teamScore = 0;

            foreach (var match in teamMatches)
            {
                if (match.FirstTeam == this)
                {
                    teamScore += match.FirstTeamScore - match.SecondTeamScore;
                }
                else
                {
                    teamScore += match.SecondTeamScore - match.FirstTeamScore;
                }
            }

            return teamScore;
        }

        public override string ToString()
        {
            return $"{GroupRanking}. Team: {Team}; Wins: {Wins}; Losses: {Losses}; Score: {Score}; ScoredPoints: {ScoredPoints}; ConcededPoints {ConcededPoints}; PointDifference {PointDifference};";
        }
    }
}
