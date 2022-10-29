using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LogParser
{
    public static class CreateReport
    {
        internal static int gameCounter = 0;
        internal static bool newGame = true;
        internal static int lineNumber = 0;
        internal static int _totalKills = 0;
        internal static List<string> _players = new List<string>();
        internal static List<Dictionary<string, object>> values = new List<Dictionary<string, object>>();
        internal static Dictionary<string, object> playerRanking = new Dictionary<string, object>();
        internal static Dictionary<string, object> games = new Dictionary<string, object>();
        internal static List<string> verifyLine = new List<string>();

        public static void GenerateJson(string line, int lastLine)
        {
            AnalysingLine(line);

            if (newGame == false && gameCounter > 0)
            {
                var game = new GameDetails();
                game.total_Kills = _totalKills;
                games.Add("game_" + gameCounter.ToString(), game);
                var x;
            }
            
            if(lastLine == lineNumber)
            {
                values.Add(games);
                Debug.WriteLine("LineNumber: " + verifyLine.Count().ToString() +" - "+verifyLine);
                Debug.WriteLine(JsonConvert.SerializeObject(values));
            }
            newGame = true;
        }

        internal static void AnalysingLine(string checkLine)
        {
            if (checkLine.Contains("InitGame:"))
            {
                _totalKills = 0;
                newGame = true;
            }
            if (checkLine.Contains("ShutdownGame:"))
            {
                newGame = false;
                gameCounter++;
            }
            if (checkLine.Contains("Kill:"))
                AnalysingKill(checkLine);

            lineNumber++;
        }
        
        internal static void AnalysingKill(string killLine)
        {
            verifyLine.Add(killLine);
            _totalKills++;
        }
    }

    public class GameDetails 
    {
        public int total_Kills { get; set; }
        public string[]? player { get; set; }
        public string? kills { get; set; }
        public string? kills_by_means { get; set; }
    }

    public class Kills
    {

    }

    public class Kills_By_Means
    {
        public int MOD_UNKNOWN;
        public int MOD_SHOTGUN;
        public int MOD_GAUNTLET;
        public int MOD_MACHINEGUN;
        public int MOD_GRENADE;
        public int MOD_GRENADE_SPLASH;
        public int MOD_ROCKET;
        public int MOD_ROCKET_SPLASH;
        public int MOD_PLASMA;
        public int MOD_PLASMA_SPLASH;
        public int MOD_RAILGUN;
        public int MOD_LIGHTNING;
        public int MOD_BFG;
        public int MOD_BFG_SPLASH;
        public int MOD_WATER;
        public int MOD_SLIME;
        public int MOD_LAVA;
        public int MOD_CRUSH;
        public int MOD_TELEFRAG;
        public int MOD_FALLING;
        public int MOD_SUICIDE;
        public int MOD_TARGET_LASER;
        public int MOD_TRIGGER_HURT;
        public int MOD_NAIL;
        public int MOD_CHAINGUN;
        public int MOD_PROXIMITY_MINE;
        public int MOD_KAMIKAZE;
        public int MOD_JUICED;
        public int MOD_GRAPPLE;
    }
}
