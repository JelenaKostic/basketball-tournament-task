using BasketballTournament.Helpers;
using BasketballTournament.Models;

var initHelper = new InitHelper();
var matchHelper = new GroupMatchHelper();
var rankHelper = new RankHelper();
var hatHelper = new HatHelper();

// Group phase and rankings
matchHelper.SimulateGroupMatches(initHelper.Teams);
rankHelper.CalculateGroupRankings(matchHelper.Matches, initHelper.Teams);
rankHelper.RankTeams(initHelper.Teams);

hatHelper.DefineHatForTeams(initHelper.Teams);

// Quarter finals
hatHelper.SimulateQuarterFinalsMatches(matchHelper.Matches, initHelper.Teams, initHelper.hatPairs);

// Semi finals
hatHelper.SimulateSemiFinalsMatches(matchHelper.Matches, initHelper.Teams);

// Third place match
hatHelper.SimulateThirdPlaceMatch(matchHelper.Matches, initHelper.Teams);

// Finals
hatHelper.SimulateFinalsMatches(matchHelper.Matches, initHelper.Teams);

Console.ReadLine();