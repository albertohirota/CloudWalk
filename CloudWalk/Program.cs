using System.Diagnostics;

namespace CloudWalkProject
{
    public class Program
    {
        static void Main(string[] args)
        {
            string whereToSaveJsonFile = @"C:\users\alber\Desktop\CloudWalkReport.json";
            string reportFile = @"C:\Users\alber\source\repos\CloudWalk\CloudWalk\Log\qgames.log";
            bool wasSucessfullyProcessed = ProcessLog.ProcessReport(reportFile,whereToSaveJsonFile);
            Debug.Write("Was sucessfully processed: " + wasSucessfullyProcessed.ToString());
        }
    }
}







