namespace FunnelWeb.Tasks
{
    public class TaskStep
    {
        private readonly int? progressEstimate;
        private readonly string logMessage;

        public TaskStep(string logMessageFormat, params object[] args) : this(null, logMessageFormat, args)
        {
        }

        public TaskStep(int? progressEstimate, string logMessageFormat, params object[] args)
        {
            this.progressEstimate = progressEstimate;
            logMessage = string.Format(logMessageFormat, args);
        }

        public int? ProgressEstimate
        {
            get { return progressEstimate; }
        }

        public string LogMessage
        {
            get { return logMessage; }
        }
    }
}