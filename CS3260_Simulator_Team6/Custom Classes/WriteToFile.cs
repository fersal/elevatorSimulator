using System.Collections.Generic;
using System.IO;

namespace CS3260_Simulator_Team6
{
    public class WriteToFile
    {
        private const string LOG_FILE_NAME = "passengerLog.txt";
        private List<string> logs;

        /// <summary>
        /// Purpose: Display the parameters passed to the method on the Console
        /// </summary>
        /// <param name="ival">a positive integer value</param>
        /// <param name="dv">a floating point value in the range of -15.00 and +350.50</param>
        /// <param name="sv">a string of length >= 1</param>
        /// Purpose: Brief sentence describing the purpose of this method.
        /// Parameters (pre-conditions/post-conditions): List the method's parameters names and
        /// constrain’s and any returned values
        /// Returns: What does the method return NOTE: NOT required if method returns void
        /// -----------------------------------------------------------------
        public WriteToFile()
        {
            logs = new List<string>();
        }

        /// <summary>
        /// Purpose: Display the parameters passed to the method on the Console
        /// </summary>
        /// <param name="ival">a positive integer value</param>
        /// <param name="dv">a floating point value in the range of -15.00 and +350.50</param>
        /// <param name="sv">a string of length >= 1</param>
        /// Purpose: Brief sentence describing the purpose of this method.
        /// Parameters (pre-conditions/post-conditions): List the method's parameters names and
        /// constrain’s and any returned values
        /// Returns: What does the method return NOTE: NOT required if method returns void
        /// -----------------------------------------------------------------
        public void AddToLog(string log)
        {
            logs.Add(log);
        }

        /// <summary>
        /// Purpose: Display the parameters passed to the method on the Console
        /// </summary>
        /// <param name="ival">a positive integer value</param>
        /// <param name="dv">a floating point value in the range of -15.00 and +350.50</param>
        /// <param name="sv">a string of length >= 1</param>
        /// Purpose: Brief sentence describing the purpose of this method.
        /// Parameters (pre-conditions/post-conditions): List the method's parameters names and
        /// constrain’s and any returned values
        /// Returns: What does the method return NOTE: NOT required if method returns void
        /// -----------------------------------------------------------------
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
