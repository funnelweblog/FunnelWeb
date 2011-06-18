using System;
using System.Text;

namespace FunnelWeb.Model
{
    public class TaskState
    {
        private StringBuilder outputLogBuilder;

        public virtual int Id { get; protected set; }
        public virtual string TaskName { get; set; }
        public virtual string Arguments { get; set; }
        public virtual int ProgressEstimate { get; set; }
        public virtual TaskStatus Status { get; set; }
        public virtual DateTime Started { get; set; }
        public virtual DateTime Updated { get; set; }
        public virtual string OutputLog
        {
            get { return outputLogBuilder.ToString(); }
            set { outputLogBuilder = new StringBuilder(value); }
        }

        public virtual void Append(string logText, params object[] args)
        {
            Updated = DateTime.UtcNow;
            outputLogBuilder.AppendFormat("[{0}] ", Updated.ToString("yyyy-MM-dd HH:mm:ss"));
            outputLogBuilder.AppendFormat(logText, args);
            outputLogBuilder.AppendLine();
        }
    }
}
