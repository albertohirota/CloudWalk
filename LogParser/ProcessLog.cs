using LogParser;
using System.Diagnostics;
using System.IO;

namespace CloudWalkProject
{
    public class ProcessLog
    {
        static IDictionary<string, object> game = new Dictionary<string, object>();
        public static void ProcessReport(string filePath)
        {
            List<string> lines = new List<string>();
            if (File.Exists(filePath))
            {
                lines = ReturnReadLog(filePath);
                LogParser(lines);
            }
        }
        private static List<string> ReturnReadLog(string filePath)
        {
            List<string> lines = new();
            try
            {
                var logFile = File.ReadLines(filePath);
                foreach(var line in logFile)
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
        private static void LogParser(List<string> logFile)
        {
            foreach (var line in logFile)
            {
                //Debug.Write(line);
                CreateReport.GenerateJson(line, logFile.Count());
            }
        }
    }
}