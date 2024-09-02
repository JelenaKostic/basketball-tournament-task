using BasketballTournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketballTournament.Helpers
{
    public class RankHelper
    {
        #region group ranking

        /// Calculate team rank in group depending on score
        public void CalculateGroupRankings(List<Match> matches, List<NationalTeam> teams)
        {
            Console.WriteLine("\nFinal score in groups:");

            foreach (var group in teams.GroupBy(x => x.Group).ToList())
            {
                RankTeamsInGroup(matches, group.ToList());
                Console.WriteLine($"\tGroup {group.Select(x => x.Group).First()}");
                Console.WriteLine("\t\t" + string.Join("\n\t\t", group.OrderBy(x => x.GroupRanking).Select(x => x.ToString())));
            }
        }

        /// Rank each team in group from 1 to 4
        /// Highest scored team will be higher ranked in group
        private void RankTeamsInGroup(List<Match> matches, List<NationalTeam> teams)
        {
            var orderedTeams = teams.OrderByDescending(x => x.Score).ToList();
            var distinctScores = teams.DistinctBy(x => x.Score).ToList().Count;

            // All teams have different score
            if (distinctScores == teams.Count)
            {
                for (var i = 0; i < orderedTeams.Count; i++)
                {
                    orderedTeams[i].GroupRanking = i + 1;
                }
            }
            // Two teams have same score, rank is calculated depending on score from their match
            else if (distinctScores == teams.Count - 1)
            {
                for (var i = 0; i < orderedTeams.Count; i++)
                {
                    if (i == orderedTeams.Count - 1 || orderedTeams[i].Score > orderedTeams[i + 1].Score)
                    {
                        orderedTeams[i].GroupRanking = i + 1;
                    }
                    else
                    {
                        RankTwoTeamsWithSameScore(matches, orderedTeams[i], orderedTeams[i + 1], i);
                        i++;
                    }
                }
            }
            // Three teams have same score
            // Or first two and second two teams have same score
            // Group rank is calculated depending on point difference
            else if (distinctScores == teams.Count - 2)
            {
                for (var i = 0; i < orderedTeams.Count; i++)
                {
                    if (i == orderedTeams.Count - 1 || orderedTeams[i].Score > orderedTeams[i + 1].Score)
                    {
                        orderedTeams[i].GroupRanking = i + 1;
                    }
                    else if (orderedTeams[i].Score == orderedTeams[i + 1].Score)
                    {
                        // Three teams have same score
                        if (i < orderedTeams.Count - 1 && orderedTeams[i].Score == orderedTeams[i + 2].Score)
                        {
                            RankThreeTeamsWithSameScore(matches, orderedTeams[i], orderedTeams[i + 1], orderedTeams[i + 2], i);
                            i += 2;
                        }
                        // First two and second two teams have same score
                        else
                        {
                            RankTwoTeamsWithSameScore(matches, orderedTeams[i], orderedTeams[i + 1], i);
                            RankTwoTeamsWithSameScore(matches, orderedTeams[i + 2], orderedTeams[i + 3], i + 2);

                            i += 3;
                        }
                    }
                }
            }
        }

        /// Comparing score on team1 and team2 match
        private void RankTwoTeamsWithSameScore(List<Match> matches, NationalTeam firstTeam, NationalTeam secondTeam, int groupRank)
        {
            var orderedTeams = firstTeam.OrderTeams(matches, secondTeam);
            RankTeams(orderedTeams.Item1, orderedTeams.Item2, groupRank);
        }

        /// Ranking teams in group
        private void RankTeams(NationalTeam higherRankedTeam, NationalTeam lowerRankedTeam, int groupRank)
        {
            higherRankedTeam.GroupRanking = groupRank + 1;
            lowerRankedTeam.GroupRanking = groupRank + 2;
        }

        /// Calculating rank depending on score difference between three teams
        private void RankThreeTeamsWithSameScore(List<Match> matches, NationalTeam firstTeam, NationalTeam secondTeam, NationalTeam thirdTeam, int rank)
        {
            var orderedTeams = firstTeam.OrderTeams(matches, secondTeam, thirdTeam);

            foreach(var team in orderedTeams)
            {
                team.GroupRanking = rank + 1;
                rank++;
            }
        }

        #endregion

        #region final ranking

        /// Calculating final rank for teams that have group ranking 1, 2 or 3
        public void RankTeams(List<NationalTeam> teams)
        {
            var ranking = 1;
            var firstThreeGroups = teams.Where(x => x.GroupRanking < 4).GroupBy(x => x.GroupRanking).ToList();

            foreach (var team in firstThreeGroups.OrderBy(x => x.Key).ToList())
            {
                var orderedTeams = team.OrderByDescending(x => x.Score).ThenByDescending(x => x.PointDifference).ThenByDescending(x => x.ScoredPoints).ToList();

                for (var i = 0; i < orderedTeams.Count; i++)
                {
                    orderedTeams[i].FinalRanking = ranking;
                    ranking++;
                }
            }

            // Excluding teams that don't have final ranking and team with final ranking 9
            foreach (var item in teams.Where(x => !x.FinalRanking.HasValue || x.FinalRanking == 9))
            {
                item.InGame = false;
            };
        }
        #endregion
    }
}
