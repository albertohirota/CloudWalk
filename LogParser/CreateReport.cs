using Newtonsoft.Json;
using System.Diagnostics;

namespace LogParser
{
    public static class CreateReport
    {
        internal static Game game = new Game();
        internal static int _totalKills = 0;
        internal static Dictionary<string, int> _playerRanking = new Dictionary<string, int>();
        internal static Dictionary<string, int> _weapon = new Dictionary<string, int>();
        internal static Dictionary<string, object> games = new Dictionary<string, object>();
        internal static List<Dictionary<string, object>> values = new List<Dictionary<string, object>>();

        public static void GenerateJson(string line, int lastLine)
        {
            AnalysingLine(line);

            if (game.newGame == false && game.gameCounter > 0)
            {
                var gameRound = new GameDetails();
                gameRound.total_Kills = _totalKills;
                gameRound.player = ReturnPlayerList();
                gameRound.kills = ReturnRankingPlayerList();
                gameRound.kills_by_means = ReturnWeaponDeath();
                games.Add("game_" + game.gameCounter.ToString(), gameRound);
            }
            
            if(lastLine == game.lineNumber)
            {
                values.Add(games);
                Debug.WriteLine(JsonConvert.SerializeObject(values));
            }
            game.newGame = true;
        }

        internal static void AnalysingLine(string checkLine)
        {
            game.lineNumber++;
            if (checkLine.Contains("InitGame:"))
                ApplyingStartingRules();

            if (checkLine.Contains("ShutdownGame:"))
                ApplyingShutdownRules();

            if (checkLine.Contains("Kill:"))
                AnalysingKill(checkLine);
        }
        
        internal static void AnalysingKill(string killLine)
        {
            _totalKills++;
            string killer = ReturnKillerString(killLine);
            string killed = ReturnKilledPersonString(killLine);
            string weapon = ReturnKillerWeaponString(killLine);
            UpdatingKillsReport(killer, killed, weapon);
        }

        internal static void UpdatingKillsReport(string killer, string killed, string weapon)
        {
            if (killer.Contains("world"))
            {
                if (_playerRanking.ContainsKey(killed))
                    _playerRanking[killed]--;
                else
                    _playerRanking.Add(killed, -1);
            }
            else
            {
                if (_playerRanking.ContainsKey(killer))
                    _playerRanking[killer]++;
                else
                    _playerRanking.Add(killer, 1);
                
                UpdatingKilledReport(killed);
            }

            if (_weapon.ContainsKey(weapon))
                _weapon[weapon]++;
            else
                _weapon.Add(weapon, 1);
        }

        internal static void UpdatingKilledReport(string killed)
        {
            if (!_playerRanking.ContainsKey(killed))
                _playerRanking.Add(killed, 0);
        }
        internal static void ApplyingShutdownRules()
        {
            game.newGame = false;
            game.gameWasShutdown = true;
        }

        internal static void ApplyingStartingRules()
        {
            if (game.gameWasShutdown)
            {
                _totalKills = 0;
                _playerRanking.Clear();
                game.newGame = true;
                game.gameCounter++;
                game.gameWasShutdown = false;
            }
        }

        internal static List<string> ReturnPlayerList()
        {
            List<string> list = new List<string>();
            foreach(KeyValuePair<string, int> player in _playerRanking)
            {
                list.Add(player.Key);
            }
            return list;
        }

        internal static Dictionary<string,int> ReturnRankingPlayerList()
        {
            Dictionary<string,int> dict = new Dictionary<string, int>();
            foreach (KeyValuePair<string, int> player in _playerRanking)
            {
                dict.Add(player.Key, player.Value);
            }
            var newDict = dict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x=> x.Value);
            return newDict;
        }

        internal static Dictionary<string, int> ReturnWeaponDeath()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach (KeyValuePair<string, int> player in _weapon)
            {
                dict.Add(player.Key, player.Value);
            }
            var newDict = dict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            return newDict;
        }

        internal static string ReturnKilledPersonString(string killLine)
        {
            int stringStartPosition = killLine.IndexOf(" killed ");
            int stringEndPosition = killLine.IndexOf(" by ");
            string killed = killLine.Substring(stringStartPosition +8, stringEndPosition-stringStartPosition-8);
            return killed;
        }

        internal static string ReturnKillerWeaponString(string killLine)
        {
            int stringEndPosition = killLine.IndexOf(" by ");
            string weapon = killLine.Substring(stringEndPosition+4);
            return weapon;
        }
        internal static string ReturnKillerString(string killLine)
        {
            string updatedString = killLine.Remove(0,14);
            int stringStartPosition = updatedString.IndexOf(": ");
            int stringEndPosition = updatedString.IndexOf(" killed ");
            string killer = updatedString.Substring(stringStartPosition + 2, stringEndPosition - stringStartPosition - 2);
            return killer;
        }
    }

    public class Game
    {
        public int lineNumber { get; set; }
        public bool newGame { get; set; }
        public int gameCounter { get; set; }
        public bool gameWasShutdown { get; set; }
    }

    public class GameDetails 
    {
        public int total_Kills { get; set; }
        public List<string>? player { get; set; }
        public Dictionary<string, int>? kills { get; set; }
        public Dictionary<string, int>? kills_by_means { get; set; }
    }
}
