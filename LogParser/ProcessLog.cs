using System.Diagnostics;
using System.IO;

namespace CloudWalkProject
{
    public class ProcessLog
    {
        public static void Process(string filePath)
        {
            if (File.Exists(filePath))
                ReadLog(filePath);
        }
        public static void ReadLog(string filePath)
        {
            string data;
            StreamReader streamReader = null;
            try
            {
                // Read the log
                streamReader = new StreamReader(filePath);
                data = streamReader.ReadLine();

                while (streamReader != null)
                {
                    Debug.Write(data);
                    data = streamReader.ReadLine();
                }
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
            }
            finally
            {
                streamReader.Close();
            }
        }
        public static void LogParser(string filePath)
        {
            
            
        }
    }

    


}