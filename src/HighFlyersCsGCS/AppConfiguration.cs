using System;

namespace HighFlyers.GCS
{
	public sealed class AppConfiguration
	{
		static AppConfiguration instance = new AppConfiguration ();
		const string fileName = "settings.xml";
		GLib.KeyFile settings;

		AppConfiguration ()
		{
			settings = (System.IO.File.Exists (fileName)) ? 
				new GLib.KeyFile (fileName, GLib.KeyFileFlags.KeepComments) :
					new GLib.KeyFile ();
		}


		private T SafeGet<T> (Func< string, string, T> function, string groupName, string key, Func<T> retMethod)
		{
			if (settings.HasGroup (groupName) && settings.HasKey (groupName, key))
				return function (groupName, key);

			return retMethod ();
		}

		public static AppConfiguration Instance {
			get { return instance; }
		}

		public void Save ()
		{
			settings.Save (fileName);
		}

		public string GetString (string groupName, string key)
		{
			return SafeGet (settings.GetString, groupName, key, () => string.Empty);
		}

		public void SetString (string groupName, string key, string value)
		{
			settings.SetString (groupName, key, value);
		}

		public void SetInt (string groupName, string key, int value)
		{
			settings.SetInteger (groupName, key, value);
		}

		public int GetInt (string groupName, string key)
		{
			return SafeGet (settings.GetInteger, groupName, key, () => 0);
		}

	}
}

