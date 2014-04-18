using System;

namespace HighFlyers.GCS.Map
{
	public delegate void CoordinateEventHandler (object sender, CoordinateEventArgs e);

	public class Coordinate : ICloneable
	{
		public double Latitude{ get; set; }
		public double Longitude{ get; set; }

		public Coordinate (double latitude, double longitude)
		{
			Latitude = latitude;
			Longitude = longitude;
		}

		public object Clone ()
		{
			return new Coordinate (Latitude, Longitude);
		}

		public override bool Equals (object coordinate)
		{
			Coordinate c = coordinate as Coordinate;

			if (c == null) {
				return false;
			}

			return c.Latitude == Latitude && c.Longitude == Longitude;
		}

		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}
	}

	public class CoordinateEventArgs : EventArgs
	{
		public Coordinate Coordinate { get; private set; }
		public int Index { get; private set; }

		public CoordinateEventArgs (Coordinate coordinate, int index = -1) : base()
		{
			Coordinate = coordinate;
			Index = index;
		}
	}
}

