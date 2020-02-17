using System;
using System.IO;
using System.Reflection;

namespace MyWebScraper
{

    public class LogWriter
    {
        private string m_exePath = string.Empty;
        public enum format
        {
            Default,
            SingleLine
        }
        /// <summary>
        /// .txt is automatically appended to the filename
        /// </summary>
        /// <param name="logMessage"></param>
        /// <param name="filename"></param>
        public LogWriter(string logMessage, string filename)
        {
            LogWrite(logMessage, filename , format.Default);
        }
        public void LogWrite(string logMessage, string filename, format format)
        {
            m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                using (StreamWriter w = File.AppendText(m_exePath + "\\" + filename + ".txt"))
                {
                    switch (format)
                    {
                        case format.Default:
                            {
                                LogDefault(logMessage, w);
                                break;
                            }
                        case format.SingleLine:
                            {
                                LogSingleLine(logMessage, w);
                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void LogDefault(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :");
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
            }
        }

        public void LogSingleLine(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.WriteLine("{0} {1} {2}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString(), logMessage);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
