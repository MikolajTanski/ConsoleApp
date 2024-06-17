using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Helpers
{
    public class LogHelper
    {
        private readonly string _logFilePath;
        private readonly List<string> _logEntries;

        public LogHelper(string logFilePath)
        {
            _logFilePath = logFilePath;
            _logEntries = new List<string>();
            InitializeLogFile();
        }

        private void InitializeLogFile()
        {
            if (!File.Exists(_logFilePath))
            {
                using (var writer = new StreamWriter(_logFilePath, append: true))
                {
                    writer.WriteLine("Type,Name,Schema,ParentName,ParentType,ParentSchema,Title,Description,CustomField1,CustomField2,CustomField3,ErrorMessage,LineNumber");
                }
            }
        }

        public void LogError(string message, int? lineNumber, string lineContent)
        {
            var logEntry = $"{message};{lineNumber};{lineContent}";
            _logEntries.Add(logEntry);
        }

        public void WriteLogEntries()
        {
            try
            {
                using (var writer = new StreamWriter(_logFilePath, append: true))
                {
                    foreach (var logEntry in _logEntries)
                    {
                        writer.WriteLine(logEntry);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error writing log entries: {ex.Message}");
            }
        }
    }
}
