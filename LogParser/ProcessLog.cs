using System.Diagnostics;

namespace CloudWalkProject
{
    public class ProcessLog
    {
        /// <summary>
        /// Function to process the Log report and save into a JSON file.
        /// </summary>
        /// <param name="filePath">It should contains the full file path of the Quake log file</param>
        /// <param name="saveJsonfile">Location and name of the JSON file where the file should be saved</param>
        /// <returns>It returns a boolean confirming if the file could be founded by the function</returns>
        public static bool ProcessReport(string filePath, string saveJsonfile)
        {
            List<string> lines = new List<string>();
            if (File.Exists(filePath))
            {
                lines = ReturnListLog(filePath);
                LogParser(lines, saveJsonfile);
                return true;
            } 
            else
            {
                return false;
            }
        }
        private static List<string> ReturnListLog(string filePath)
        {
            List<string> lines = new();
            try
            {
                var logFile = File.ReadLines(filePath);
                foreach (var line in logFile)
                {
                    lines.Add(line);
                }
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }
            return lines;
        }

        private static void LogParser(List<string> logFile, string saveJsonfile)
        {
            foreach (var line in logFile)
            {
                CreateReport.GenerateJson(line, logFile.Count(), saveJsonfile);
            }
        }
    }
    public class ReportAnaliser
    {
        public int lineNumber { get; set; }
        public bool newGame { get; set; }
        public int gameCounter { get; set; }
        public bool gameWasShutdown { get; set; }
        public bool gameRoundGenerated { get; set; }
    }

    public class GameDetails
    {
        public int total_Kills { get; set; }
        public List<string>? players { get; set; }
        public Dictionary<string, int>? kills { get; set; }
        public Dictionary<string, int>? kills_by_means { get; set; }
    }
}