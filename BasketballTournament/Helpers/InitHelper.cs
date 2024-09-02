using BasketballTournament.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace BasketballTournament.Helpers
{
    public class InitHelper
    {
        public NationalTeam TeamKanada = new() { Team = "Kanada", ISOCode = "CAN", FIBARanking = 7, Group = 'A' };
        public NationalTeam TeamAustralija = new() { Team = "Australija", ISOCode = "AUS", FIBARanking = 5, Group = 'A' };
        public NationalTeam TeamGrcka = new() { Team = "Grčka", ISOCode = "GRE", FIBARanking = 14, Group = 'A' };
        public NationalTeam TeamSpanija = new() { Team = "Španija", ISOCode = "ESP", FIBARanking = 2, Group = 'A' };

        public NationalTeam TeamNemacka = new() { Team = "Nemačka", ISOCode = "GER", FIBARanking = 3, Group = 'B' };
        public NationalTeam TeamFrancuska = new() { Team = "Francuska", ISOCode = "FRA", FIBARanking = 9, Group = 'B' };
        public NationalTeam TeamBrazil = new() { Team = "Brazil", ISOCode = "BRA", FIBARanking = 12, Group = 'B' };
        public NationalTeam TeamJapan = new() { Team = "Japan", ISOCode = "JPN", FIBARanking = 26, Group = 'B' };

        public NationalTeam TeamSjedinjeneDrzave = new() { Team = "Sjedinjene Države", ISOCode = "USA", FIBARanking = 1, Group = 'C' };
        public NationalTeam TeamSrbija = new() { Team = "Srbija", ISOCode = "SRB", FIBARanking = 4, Group = 'C' };
        public NationalTeam TeamJuzniSudan = new() { Team = "Južni Sudan", ISOCode = "SSD", FIBARanking = 34, Group = 'C' };
        public NationalTeam TeamPuertoRiko = new() { Team = "Puerto Riko", ISOCode = "PRI", FIBARanking = 16, Group = 'C' };

        public List<NationalTeam> Teams = new();

        public List<List<char>> hatPairs = new() { new List<char> { 'D', 'G' }, new List<char> { 'E', 'F' } };
        public List<List<char>> hatPairsSemiFinals = new() { new List<char> { 'D', 'E' }, new List<char> { 'F', 'G' } };

        public InitHelper()
        {
            Teams = new() 
            { 
                TeamKanada, TeamAustralija, TeamGrcka, TeamSpanija, 
                TeamNemacka, TeamFrancuska, TeamBrazil, TeamJapan, 
                TeamSjedinjeneDrzave, TeamSrbija, TeamJuzniSudan, TeamPuertoRiko 
            };
        }
    }
}
