namespace TaskManagement.Utils
{
    public class Log
    {
        private readonly ILogger<Log> _logger;

        public Log(ILogger<Log> logger)
        {
            _logger = logger;
        }

        public void LogMessage(string message)
        {
            _logger.LogInformation(message);
        }
    }
}
