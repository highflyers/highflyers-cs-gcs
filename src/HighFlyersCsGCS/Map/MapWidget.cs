using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HighFlyers.GCS.Map
{
	public abstract class MapWidget : Gtk.Widget
	{
		List<Coordinate> waypoints = new List<Coordinate> ();
		List<Coordinate> path = new List<Coordinate> ();

		#region Waypoints
		public virtual void AddWaypoint (Coordinate coordinate)
		{
			waypoints.Add (coordinate);
			OnWaypointEventCalled (WaypointAdded, new CoordinateEventArgs (coordinate, waypoints.Count - 1));
		}

		public virtual void RemoveWaypoint (Coordinate coordinate)
		{
			waypoints.Remove (coordinate);
			OnWaypointEventCalled (WaypointRemoved, new CoordinateEventArgs (coordinate));
		}

		public virtual void RemoveWaypoint (int index)
		{
			var evArg = new CoordinateEventArgs (waypoints [index]);
			waypoints.RemoveAt (index);
			OnWaypointEventCalled (WaypointRemoved, evArg);
		}

		public virtual void MoveWaypoint (int index, Coordinate newCoordinate)
		{
			waypoints [index] = newCoordinate;
			OnWaypointEventCalled (WaypointModified, new CoordinateEventArgs (newCoordinate, index));
		}

		public virtual void MoveWaypoint (Coordinate previousCoordinate, Coordinate newCoordinate)
		{
			int index = waypoints.FindIndex (coordinate => coordinate.Equals (previousCoordinate));
			MoveWaypoint (index, newCoordinate);
		}

		public Coordinate GetWaypoint (int index)
		{
			return waypoints [index].Clone () as Coordinate;
		}

		public void ClearWaypoints ()
		{
			while (waypoints.Count > 0) {
				RemoveWaypoint (0);
			}
		}

		public ReadOnlyCollection<Coordinate> GetWaypointList ()
		{
			return waypoints.AsReadOnly ();
		}
		#endregion Waypoints

		#region Path
		public void AddPathpoint (Coordinate coordinate)
		{
			path.Add (coordinate);
			OnWaypointEventCalled (PathpointAdded, new CoordinateEventArgs (coordinate, path.Count));
		}

		public void ClearPath ()
		{
			path.Clear ();

			if (PathCleared != null) {
				PathCleared (this, null);
			}
		}

		public bool PathpointExists (Coordinate coordinate, double epsilon = 0.0)
		{
			if (epsilon == 0.0) {
				return waypoints.FindIndex (c => c.Equals (coordinate)) != -1;
			}

			epsilon = Math.Pow (epsilon, 2.0);

			foreach (Coordinate c in waypoints) {
				if (Math.Pow (c.Latitude - coordinate.Latitude, 2.0) + Math.Pow (c.Longitude - coordinate.Longitude, 2.0) <= epsilon)
					return true;
			}

			return false;
		}

		public ReadOnlyCollection<Coordinate> GetPath ()
		{
			return path.AsReadOnly ();
		}
		#endregion Path

		#region Map
		public abstract void LoadMap (string uri);
		public abstract void JumpTo(Coordinate coordinate);
		public abstract Coordinate GetCurrentMapLocation ();
		public abstract bool PathPointFollowerMode{ get; set; }
		#endregion Map

		#region Events
		public event CoordinateEventHandler WaypointAdded;
		public event CoordinateEventHandler WaypointRemoved;
		public event CoordinateEventHandler WaypointModified;

		public event CoordinateEventHandler PathpointAdded;
		public event EventHandler PathCleared;
		#endregion Events

		void OnWaypointEventCalled(CoordinateEventHandler handler, CoordinateEventArgs args)
		{
			if (handler != null) {
				handler (this, args);
			}
		}
	}
}

