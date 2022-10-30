using Newtonsoft.Json;
using System.Diagnostics;

namespace CloudWalkProject
{
    public static class CreateReport
    {
        private static ReportAnaliser report = new ReportAnaliser();
        private static int _totalKills = 0;
        private static Dictionary<string, int> _playerRanking = new Dictionary<string, int>();
        private static Dictionary<string, int> _weapon = new Dictionary<string, int>();
        private static Dictionary<string, object> games = new Dictionary<string, object>();

        /// <summary>
        /// Function that will create a JSON file
        /// </summary>
        /// <param name="line"> It should include the list to be read...</param>
        /// <param name="lastLine"> It is expected to be informed what is the last line of the log file.</param>
        /// <param name="saveJsonfile"> Location where the log file will be saved... </param>
        internal static void GenerateJson(string line, int lastLine, string saveJsonfile)
        {
            AnalysingLine(line);

            if (report.newGame == false && report.gameCounter > 0)
                CreatingJsonGameRound();

            if (lastLine == report.lineNumber && report.gameRoundGenerated)
            {
                using (StreamWriter file = File.CreateText(saveJsonfile))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, games);
                }
            }
            report.newGame = true;
        }

        /// <summary>
        /// Function to validate if the specific file exists. It will loop for 10 seconds looking for the file.
        /// </summary>
        /// <param name="filePath"> File name and path parameter is required for the method. </param>
        /// <returns></returns>
        public static bool CheckFileExists(string filePath)
        {
            int i = 0;
            bool fileExists;
            do
            {
                Thread.Sleep(1000);
                fileExists = File.Exists(filePath);
                i++;
            }
            while (i < 10 && !fileExists);

            return fileExists;
        }

        /// <summary>
        /// Function to create the JSON structure for each game round.
        /// </summary>
        private static void CreatingJsonGameRound()
        {
            var gameRound = new GameDetails();
            gameRound.total_Kills = _totalKills;
            gameRound.players = ReturnPlayerList();
            gameRound.kills = ReturnRankingPlayerList();
            gameRound.kills_by_means = ReturnWeaponDeath();
            games.Add("game_" + report.gameCounter.ToString(), gameRound);
            report.gameRoundGenerated = true;
        }

        /// <summary>
        /// Function to check filter each line of the log
        /// </summary>
        /// <param name="checkLine"> A String of the line required to be analyzed </param>
        private static void AnalysingLine(string checkLine)
        {
            report.lineNumber++;
            if (checkLine.Contains("InitGame:"))
                ApplyingStartingRules();

            if (checkLine.Contains("ShutdownGame:"))
                ApplyingShutdownRules();

            if (checkLine.Contains("Kill:"))
                AnalysingKillLine(checkLine);
        }

        private static void AnalysingKillLine(string killLine)
        {
            _totalKills++;
            string killer = ReturnKillerString(killLine);
            string killed = ReturnKilledPersonString(killLine);
            string weapon = ReturnKillerWeaponString(killLine);
            UpdatingKillsReport(killer, killed, weapon);
        }

        private static void UpdatingKillsReport(string killer, string killed, string weapon)
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

        private static void UpdatingKilledReport(string killed)
        {
            if (!_playerRanking.ContainsKey(killed))
                _playerRanking.Add(killed, 0);
        }

        private static void ApplyingShutdownRules()
        {
            report.newGame = false;
            report.gameWasShutdown = true;
        }

        private static void ApplyingStartingRules()
        {
            if (report.gameWasShutdown)
            {
                _totalKills = 0;
                _playerRanking.Clear();
                _weapon.Clear();
                report.newGame = true;
                report.gameCounter++;
                report.gameWasShutdown = false;
            }
        }

        private static List<string> ReturnPlayerList()
        {
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, int> player in _playerRanking)
            {
                list.Add(player.Key);
            }
            return list;
        }

        private static Dictionary<string, int> ReturnRankingPlayerList()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach (KeyValuePair<string, int> player in _playerRanking)
            {
                dict.Add(player.Key, player.Value);
            }
            var newDict = dict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            return newDict;
        }

        private static Dictionary<string, int> ReturnWeaponDeath()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach (KeyValuePair<string, int> player in _weapon)
            {
                dict.Add(player.Key, player.Value);
            }
            var newDict = dict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            return newDict;
        }

        private static string ReturnKilledPersonString(string killLine)
        {
            int stringStartPosition = killLine.IndexOf(" killed ");
            int stringEndPosition = killLine.IndexOf(" by ");
            string killed = killLine.Substring(stringStartPosition + 8, stringEndPosition - stringStartPosition - 8);
            return killed;
        }

        private static string ReturnKillerWeaponString(string killLine)
        {
            int stringEndPosition = killLine.IndexOf(" by ");
            string weapon = killLine.Substring(stringEndPosition + 4);
            return weapon;
        }

        private static string ReturnKillerString(string killLine)
        {
            string updatedString = killLine.Remove(0, 14);
            int stringStartPosition = updatedString.IndexOf(": ");
            int stringEndPosition = updatedString.IndexOf(" killed ");
            string killer = updatedString.Substring(stringStartPosition + 2, stringEndPosition - stringStartPosition - 2);
            return killer;
        }
    }
}
