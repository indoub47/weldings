using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Weldings
{
    public static class LogWriter
    {
        private static string fileName = string.Empty;

        private static string FileName
        {
            get
            {
                if (fileName == string.Empty)
                {
                    fileName = Path.Combine(
                        Properties.Settings.Default.OutputPath, 
                        Properties.Settings.Default.LogFileName);
                    if (!File.Exists(fileName))
                    {
                        File.Create(fileName);
                    }
                }
                return fileName;
            }
        }

        public static void Log(Exception exception)
        {
            try
            {
                using (StreamWriter w = File.AppendText(LogWriter.FileName))
                {
                    LogWriter.WriteLog(exception, w);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void Log(string logText)
        {
            try
            {
                using (StreamWriter w = File.AppendText(LogWriter.FileName))
                {
                    LogWriter.WriteLog(logText, w);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(logText);
                Console.WriteLine(ex.Message);
            }
        }

        private static void WriteLog(Exception exception, TextWriter writer)
        {
            StringBuilder sb = new StringBuilder();
            exceptionRecord(exception, sb);
            try
            {
                writer.Write("\r\nException Log Entry: ");
                writer.WriteLine("{0} {1}",
                    DateTime.Now.ToShortDateString(),
                    DateTime.Now.ToLongTimeString());
                writer.WriteLine(sb);
                writer.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
            }
        }

        private static void WriteLog(string logText, TextWriter writer)
        {
            try
            {
                writer.Write("\r\nLog Entry: ");
                writer.WriteLine("{0} {1}",
                    DateTime.Now.ToShortDateString(),
                    DateTime.Now.ToLongTimeString());
                writer.WriteLine(" : ");
                writer.WriteLine(" : " + logText);
                writer.WriteLine("------------------------------------");
            }
            catch (Exception ex)
            {
            }
        }

        private static void exceptionRecord(Exception exception, StringBuilder sb)
        {
            if (exception.InnerException != null)
            {
                 exceptionRecord(exception.InnerException, sb);
            }

            sb.AppendFormat("\t\t------- Exception Type: {0}", exception.GetType()).AppendLine();
            sb.AppendFormat("Message: {0}", exception.Message).AppendLine();
            sb.AppendFormat("Source: {0}", exception.Source).AppendLine();
            sb.AppendFormat("Stack trace: {0}", exception.StackTrace).AppendLine();
            sb.AppendFormat("Target: {0}", exception.TargetSite).AppendLine();
        }
    }

}
