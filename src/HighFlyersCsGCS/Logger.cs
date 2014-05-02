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

	public class Logger
	{
		static Logger instance = new Logger ();

		LogLevel log_threshold = LogLevel.Warning;

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

		public void Log (string message, LogLevel level)
		{
			if (level < log_threshold)
				return;

			string msg = String.Format("[{0}][{1}]: {2}", level.ToString ().ToUpper (), DateTime.Today, message);
			Console.WriteLine (msg);
		}
	}
}

