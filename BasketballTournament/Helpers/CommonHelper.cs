using BasketballTournament.Common;
using BasketballTournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketballTournament.Helpers
{
    public static class CommonHelper
    {
        /// Generate random score for teams
        public static (int, int) SimulateScore(NationalTeam firstTeam, NationalTeam secondTeam)
        {
            var random = new Random();
            var firstTeamScore = 0;
            var secondTeamScore = 0;

            if (firstTeam.IsHigherFIBARanked(secondTeam))
            {
                (firstTeamScore, secondTeamScore) = GenerateScoreForMatch(firstTeam, secondTeam);
            }
            else
            {
                (secondTeamScore, firstTeamScore) = GenerateScoreForMatch(secondTeam, firstTeam);
            }

            // Ensure that teams will have different number of points
            if (firstTeamScore == secondTeamScore)
            {
                secondTeamScore += random.Next(1, 5);
            }

            return (firstTeamScore, secondTeamScore);
        }

        /// Generate random score for team including its FIBA ranking and number of wins in calculation
        /// Minimal score is 60
        /// If generated score is equal to 0 that means that this team surrended match
        private static int GenerateScore(int ranking, bool isHigherRankedTeam = true, bool hasMoreWinnings = true)
        {
            var random = new Random();
            var randomScore = random.Next(70, 110) - ranking;

            // Increasing score to higher ranking team in order to promote it for winning
            ranking += isHigherRankedTeam ? random.Next(5, 10) : random.Next(-10, -5);

            // Increasing score to better scored team in order to promote it for winning
            ranking += hasMoreWinnings ? random.Next(1, 5) : random.Next(-5, -1);

            if (randomScore < 60)
            {
                randomScore = 60 + random.Next(0, 5);
            }

            return randomScore;
        }

        private static (int, int) GenerateScoreForMatch(NationalTeam higherRankedTeam, NationalTeam lowerRankedTeam)
        {
            var random = new Random();

            // Randomly determine if one team forfeits the game
            bool higherRankedTeamForfeits = random.Next(0, 1000) < 1; // 0.1% chance of forfeiting
            bool lowerRankedTeamForfeits = random.Next(0, 100) < 1; // 1% chance of forfeiting

            if (higherRankedTeamForfeits)
            {
                return (0, 20);
            }

            if (lowerRankedTeamForfeits)
            {
                return (20, 0);
            }

            return (GenerateScore(higherRankedTeam.FIBARanking, true), GenerateScore(lowerRankedTeam.FIBARanking, false));

        }

        /// Finding match between two teams
        public static Match FindMatchOfTwoTeams(List<Match> matches, NationalTeam firstTeam, NationalTeam secondTeam)
        {
            return matches.Where(x => (x.FirstTeam == firstTeam || x.SecondTeam == firstTeam)
                            && (x.FirstTeam == secondTeam || x.SecondTeam == secondTeam)).First();
        }

        /// Finding common matches of three team
        public static List<Match> FindMatchesOfThreeTeams(List<Match> matches, NationalTeam firstTeam, NationalTeam secondTeam, NationalTeam thirdTeam)
        {
            return new List<Match>
            {
                FindMatchOfTwoTeams(matches, firstTeam, secondTeam),
                FindMatchOfTwoTeams(matches, firstTeam, thirdTeam),
                FindMatchOfTwoTeams(matches, secondTeam, thirdTeam)
            };
        }

        /// Create match for two teams and simulate scores
        public static void CreateMatch(List<Match> matches, NationalTeam firstTeam, NationalTeam secondTeam, bool isEliminatingRound = false, RoundEnum? round = null)
        {
            var score = CommonHelper.SimulateScore(firstTeam, secondTeam);

            firstTeam.ScoredPoints += score.Item1;
            firstTeam.ConcededPoints += score.Item2;

            secondTeam.ScoredPoints += score.Item2;
            secondTeam.ConcededPoints += score.Item1;

            if (score.Item1 > score.Item2)
            {
                SaveScore(firstTeam, secondTeam, score.Item2 == 0, isEliminatingRound);
            }
            else
            {
                SaveScore(secondTeam, firstTeam, score.Item1 == 0, isEliminatingRound);
            }

            var newMatch = new Match { FirstTeam = firstTeam, SecondTeam = secondTeam, FirstTeamScore = score.Item1, SecondTeamScore = score.Item2, Round = round };
            matches.Add(newMatch);
        }

        /// Updating score and wins/losses for each team
        private static void SaveScore(NationalTeam winningTeam, NationalTeam losingTeam, bool losingTeamSurrended, bool isEliminatingRound = false)
        {
            winningTeam.Score += Constants.MATCH_WON_POINTS;
            winningTeam.Wins++;

            losingTeam.Score += losingTeamSurrended ? Constants.MATCH_SURRENDED_POINTS : Constants.MATCH_LOST_POINTS;
            losingTeam.Losses++;
            if (isEliminatingRound) losingTeam.InGame = false;
        }
    }
}
