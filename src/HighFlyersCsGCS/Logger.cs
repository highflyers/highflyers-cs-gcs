using System;

namespace HighFlyers.GCS
{
	public enum LogLevel
	{
		Debug,
		Info,
		Warning,
		Error
	}

	public delegate void LogEventHandler (object sender, LogEventArgs e);

	public class Logger
	{
		static Logger instance = new Logger ();

		LogLevel log_threshold = LogLevel.Debug;

		public event LogEventHandler LogOccured;

		Logger ()
		{
		}

		public static Logger Instance {
			get { return instance; }
		}

		public void SetThreshold (LogLevel threshold)
		{
			log_threshold = threshold;
		}

		public void Log (LogLevel level, string message)
		{
			if (level < log_threshold)
				return;

			string msg = String.Format("[{0}][{1}]: {2}", level.ToString ().ToUpper (), DateTime.Today, message);
			Console.WriteLine (msg);

			if (LogOccured != null)
				LogOccured (this, new LogEventArgs (message, msg));
		}
	}

	public class LogEventArgs : EventArgs
	{
		public string RawMessage {
			get;
			private set;
		}

		public string FormattedMessage {
			get;
			private set;
		}

		public LogEventArgs (string raw, string formatted)
		{
			RawMessage = raw;
			FormattedMessage = formatted;
		}
	}
}

