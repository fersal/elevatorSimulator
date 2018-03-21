using System.Collections.Generic;
using System.IO;

namespace CS3260_Simulator_Team6
{
    public class WriteToFile
    {
        private const string LOG_FILE_NAME = "passengerLog.txt";
        private List<string> logs;

        public WriteToFile()
        {
            logs = new List<string>();
        }

        public void AddToLog(string log)
        {
            logs.Add(log);
        }

        public void WriteLogFile()
        {
            using (StreamWriter writeLog = new StreamWriter(LOG_FILE_NAME))
            {
                foreach(var line in logs)
                {
                    writeLog.WriteLine(line);
                }
            }
        }
    }
}
