using Microsoft.VisualStudio.TestTools.UnitTesting;
using CloudWalkProject;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Dynamic;
using System.Data;
using System.Security.Principal;
using System.Diagnostics;

namespace CloudWalkProject.Tests
{
    [TestClass()]
    public class IntegrationTests
    {
        public const string jsonSaveFile = @"C:\users\alber\Desktop\IntegrationTestsReport.json";
        public const string reportFile = @"C:\Users\alber\source\repos\CloudWalk\UnitTest\TestLog\TestLogSample.txt";
        public string json = "";

        private void RunProcessLog()
        {
            bool processRan = ProcessLog.ProcessReport(reportFile, jsonSaveFile);
            bool fileExists = CreateReport.CheckFileExists(jsonSaveFile);
            Debug.Write("Json file exists: " + fileExists);
            json = File.ReadAllText(jsonSaveFile);
            Debug.Write("Process ran well: "+ processRan.ToString());
        }

        [TestMethod()]
        public void GenerateJson_ValidateGameNumber_True()
        {
            RunProcessLog();
            Dictionary<string, object>? game = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            
            int games = game!.Count;

            Assert.AreEqual(2, games);
        }

        [TestMethod()]
        public void GenerateJson_ValidateTotalKillsGame1_True()
        {
            RunProcessLog();
            Dictionary<string, object>? game = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            string? game1 = game!["game_1"].ToString();

            GameDetails? gameDetails = JsonConvert.DeserializeObject<GameDetails>(game1!);
            int kills = gameDetails!.total_Kills;

            Assert.AreEqual(9, kills);
        }

        [TestMethod()]
        public void GenerateJson_ValidateTotalKillsGame2_True()
        {
            RunProcessLog();
            Dictionary<string, object>? game = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            string? game1 = game!["game_2"].ToString();

            GameDetails? gameDetails = JsonConvert.DeserializeObject<GameDetails>(game1!);
            int kills = gameDetails!.total_Kills;

            Assert.AreEqual(4, kills);
        }

        [TestMethod()]
        public void GenerateJson_ValidatePlayersGame1_True()
        {
            RunProcessLog();
            var expected = new List<string>();
            expected.AddRange(new[] { "Isgalamido", "Mocinha"});
            Dictionary<string, object>? game = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            string? game1 = game!["game_1"].ToString();

            GameDetails? gameDetails = JsonConvert.DeserializeObject<GameDetails>(game1!);
            List<string>? players = gameDetails!.players;

            CollectionAssert.AreEqual(expected, players);
        }

        [TestMethod()]
        public void GenerateJson_ValidatePlayersGame2_True()
        {
            RunProcessLog();
            var expected = new List<string>();
            expected.AddRange(new[] { "Isgalamido", "Mocinha", "Zeh", "Dono da Bola" });
            Dictionary<string, object>? game = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            string? game1 = game!["game_2"].ToString();

            GameDetails? gameDetails = JsonConvert.DeserializeObject<GameDetails>(game1!);
            List<string>? players = gameDetails!.players;

            CollectionAssert.AreEqual(expected, players);
        }

        [TestMethod()]
        public void GenerateJson_ValidateKillsGame1_True()
        {
            RunProcessLog();
            var expected = new Dictionary<string, int>()
            {
                {"Mocinha",0 },
                {"Isgalamido",-3}
            };
            Dictionary<string, object>? game = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            string? game1 = game!["game_1"].ToString();

            GameDetails? gameDetails = JsonConvert.DeserializeObject<GameDetails>(game1!);
            Dictionary<string,int>? kills = gameDetails!.kills;

            CollectionAssert.AreEqual(expected, kills);
        }

        [TestMethod()]
        public void GenerateJson_ValidateKillsGame2_True()
        {
            RunProcessLog();
            var expected = new Dictionary<string, int>()
            {
                {"Isgalamido",1},
                {"Mocinha",0},
                {"Dono da Bola",-1},
                {"Zeh",-2}
            };
            Dictionary<string, object>? game = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            string? game1 = game!["game_2"].ToString();

            GameDetails? gameDetails = JsonConvert.DeserializeObject<GameDetails>(game1!);
            Dictionary<string, int>? kills = gameDetails!.kills;

            CollectionAssert.AreEqual(expected, kills);
        }

        [TestMethod()]
        public void GenerateJson_ValidateWeaponsGame1_True()
        {
            RunProcessLog();
            var expected = new Dictionary<string, int>()
            {
                {"MOD_TRIGGER_HURT",6},
                {"MOD_ROCKET_SPLASH",3}
            };
            Dictionary<string, object>? game = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            string? game1 = game!["game_1"].ToString();

            GameDetails? gameDetails = JsonConvert.DeserializeObject<GameDetails>(game1!);
            Dictionary<string, int>? weapons = gameDetails!.kills_by_means;

            CollectionAssert.AreEqual(expected, weapons);
        }

        [TestMethod()]
        public void GenerateJson_ValidateWeaponsGame2_True()
        {
            RunProcessLog();
            var expected = new Dictionary<string, int>()
            {
                {"MOD_TRIGGER_HURT",2},
                {"MOD_ROCKET",1},
                {"MOD_FALLING",1 }
            };
            Dictionary<string, object>? game = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            string? game1 = game!["game_2"].ToString();

            GameDetails? gameDetails = JsonConvert.DeserializeObject<GameDetails>(game1!);
            Dictionary<string, int>? weapons = gameDetails!.kills_by_means;

            CollectionAssert.AreEqual(expected, weapons);
        }

        [TestMethod()]
        public void ALoadInvalidLogFile()
        {
            string filePath = @"C:\Users\alber\source\repos\CloudWalk\UnitTest\TestLog\InvalidLog.txt";
            string jsonFilePath = @"C:\users\alber\Desktop\InvalidLogReport.json";
            if (File.Exists(jsonFilePath))
            {
                File.Delete(jsonFilePath);
            }
            bool jsonGenerated = ProcessLog.ProcessReport(filePath, jsonFilePath);
            bool fileExists = CreateReport.CheckFileExists(jsonFilePath);

            Assert.AreNotEqual(jsonGenerated, fileExists);
        }

        [TestMethod()]
        public void LoadInexistentLogFile()
        {
            string filePath = @"C:\InvalidLog.txt";
            string jsonFilePath = @"C:\users\alber\Desktop\UnexistentReport.json";
            bool jsonGenerated = ProcessLog.ProcessReport(filePath,jsonFilePath );
            bool fileExists = CreateReport.CheckFileExists(jsonFilePath);

            Assert.AreEqual(jsonGenerated, fileExists);
        }

        [TestMethod()]
        public void LoadValidFile()
        {
            bool jsonGenerated = ProcessLog.ProcessReport(reportFile, jsonSaveFile);
            bool fileExists = CreateReport.CheckFileExists(jsonSaveFile);

            Assert.AreEqual(jsonGenerated, fileExists);
        }
    }
}
