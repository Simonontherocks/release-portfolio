using System;
using System.IO;

namespace CineVault.BusinessLayer.Services
{
    public class LoggerService
    {
        private readonly string _logFilePath;

        public LoggerService(string logFilePath = null)
        {
            if (string.IsNullOrWhiteSpace(logFilePath))
            {                
                string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs"); // Als geen pad opgegeven is: gebruik standaard logs-map in applicatiemap

                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                _logFilePath = Path.Combine(logDirectory, "log.txt");
            }
            else
            {                
                string directory = Path.GetDirectoryName(logFilePath); // Als een specifiek pad werd meegegeven (bijv. via DI of config)
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                _logFilePath = logFilePath;
            }
        }

        public void Log(string message,    
            [System.Runtime.CompilerServices.CallerMemberName] string methodName = "", // Haalt automatisch de naam van de methode op die deze log aanroept    
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "", // Haalt automatisch het volledige bestandspad op van het bestand dat deze methode aanroept    
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0) // Haalt automatisch het regelnr. op waar Log() werd aangeroepen
        {            
            string className = Path.GetFileNameWithoutExtension(sourceFilePath); // Haalt de bestandsnaam (zonder extensie) op van het bestand dat de log aanroept (dus de klasse)            
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {className}.{methodName} (line {sourceLineNumber}): {message}"; // Vormt de uiteindelijke logboodschap            
            File.AppendAllText(_logFilePath, logMessage + Environment.NewLine); // Voegt de boodschap toe aan het logbestand op de eerder ingestelde locatie
        }


    }
}
