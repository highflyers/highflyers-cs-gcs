using Gdk;
using Cairo;
using System;

namespace HighFlyers.GCS.Map
{
	public  class FileMapWidget : MapWidget
	{
		private string fileName;
		private Coordinate startPoint; // = Coordinate(0,0);       // coordinates in top-left corner	
		private Coordinate stopPoint;  //= Coordinate(0,0);       // coordinates in bottom-right corner
		private double pixPerGradX;
		private double pixPerGradY;
		public Gtk.Image mapImage;		//temporaily
		private Gtk.Image miniImage;

		double mouse_x;
		double mouse_y;

		double end_mouse_x;
		double end_mouse_y;

		double drag_position_x;
		double drag_position_y;

		bool button1_clicked;
		private bool mapLoaded = false;

		Surface waypoints;
		double offset;

		public FileMapWidget () : base ()
		{
			drag_position_x = 0;
			drag_position_y = 0;
			LoadMap ("../Debug/interfaces/images/russia-map");			//temporaly



			AddEvents ((int)EventMask.PointerMotionMask | (int)EventMask.ButtonPressMask
			           | (int)EventMask.ButtonReleaseMask);
		}

		public override void LoadMap(string file)
		{
			if (file == null)
				return;

			/*if (!System.IO.File.Exists(file + ".gif"))				
				throw new Exception("File: " + file + " doesn't exists");

			if (!System.IO.File.Exists(file + ".txt"))
				throw new Exception("File: " + file + ".txt doesn't exists");
			*/
			fileName = file;


			System.IO.StreamReader Stremread = new System.IO.StreamReader(file + ".txt");

			startPoint = new Coordinate(
				Convert.ToDouble(Stremread.ReadLine()),
				Convert.ToDouble(Stremread.ReadLine()));

			stopPoint = new Coordinate(
				Convert.ToDouble(Stremread.ReadLine()),
				Convert.ToDouble(Stremread.ReadLine()));

			Pixbuf mine = new Pixbuf (file + ".gif");			

			mapImage = new Gtk.Image (mine);
			miniImage = new Gtk.Image (mine);


			pixPerGradX = (startPoint.Longitude - stopPoint.Longitude) / mapImage.Pixbuf.Width;
			pixPerGradY = -(startPoint.Latitude - stopPoint.Latitude) / mapImage.Pixbuf.Height;

			mapLoaded = true;
		}

		void SetPosition(double x_pos, double y_pos)
		{
			if (button1_clicked == false) 
			{
				drag_position_x += x_pos;
				drag_position_y += y_pos;
				end_mouse_x = 0;
				end_mouse_y = 0;
				mouse_x = 0;
				mouse_y = 0;
			}
		}

		protected override bool OnMotionNotifyEvent (EventMotion evnt)
		{	
			return true;
		}



		protected override bool OnButtonPressEvent(EventButton evnt)
		{
			uint but;
			but = evnt.Button;
			if (but == 1) 
			{
				button1_clicked = true;
				mouse_x = evnt.X;
				mouse_y = evnt.Y;
			}
			if (evnt.Type == EventType.TwoButtonPress) 			//Here is doubleclick event!!!!
			{
				double mx, my;
				mx = mouse_x + (-drag_position_x);			
				my = mouse_y + (-drag_position_y);
				AddWaypoint(PixelToCoordinate(new PointD(mx, my)));
			}
			return true;
		}

		protected override bool OnButtonReleaseEvent(EventButton evnt)
		{
			uint but;
			but = evnt.Button;
			if (but == 1) 
			{
				button1_clicked = false;
				end_mouse_x = evnt.X;
				end_mouse_y = evnt.Y;
			}
			drag_position_x += end_mouse_x - mouse_x;
			drag_position_y += end_mouse_y - mouse_y;
			end_mouse_x = 0;
			end_mouse_y = 0;
			mouse_x = 0;
			mouse_y = 0;
			QueueDraw ();
			return true;
		}


		private void Resize(Cairo.Context cr)					//tested work fine
		{
			Coordinate map_position = GetCurrentMapLocation ();
			for (int i = 0; i < GetPath().Count; ++i) 	//it resize only on pathpoint (you can't add waypoint beyond the map and see it before it flies there)
			{
				var list = GetPath();
				Coordinate temp = list [i];
				if (temp.Latitude < startPoint.Latitude || temp.Longitude < startPoint.Longitude 
					|| temp.Latitude > stopPoint.Latitude || temp.Longitude > stopPoint.Longitude) 	//have to resize sufrace and change start and stop coordinates.
				{
					const double resize_value = 0.5;
					Coordinate test = new Coordinate (resize_value * (stopPoint.Latitude - startPoint.Latitude), resize_value * (stopPoint.Longitude - startPoint.Longitude));

					offset = CoordinateToPixel (test).X  ;

					using (var target = cr.GetTarget ()) 
					{
						waypoints = target.CreateSimilar (Content.ColorAlpha, (int)(this.mapImage.Pixbuf.Width + 2*CoordinateToPixel(test).X), 
						                                  (int)(this.mapImage.Pixbuf.Height + 2*CoordinateToPixel(test).Y) );			// there is *2 because we have to add this much to 
																																		// to both side of map
					}
					startPoint.Latitude -= test.Latitude;
					startPoint.Longitude -= test.Longitude;
					stopPoint.Latitude += test.Latitude;
					stopPoint.Longitude += test.Longitude;
				}
			}
			JumpTo (map_position);
		}


		#region Drawing

		protected override bool OnDrawn (Cairo.Context cr)
		{
			double proportion = (double)this.mapImage.Pixbuf.Width / (double)this.mapImage.Pixbuf.Height;		//needed for resize
			Gdk.CairoHelper.SetSourcePixbuf (cr, mapImage.Pixbuf, drag_position_x  +offset , drag_position_y + (offset / proportion) );			
			cr.Paint();

			//if canvas is too small to draw all waypoints resize it
			Resize (cr);
			DrawWaypoints (cr);
			DrawPathPoint (cr);

			return true;  
		} 

		private void DrawWaypoints(Cairo.Context cr)
		{

			using (var target = cr.GetTarget ()) 
			{
				if (waypoints == null) 
				{
					waypoints = target.CreateSimilar (Content.ColorAlpha, this.mapImage.Pixbuf.Width , this.mapImage.Pixbuf.Height );
				}
			}

			using (Context cr_overlay = new Context (waypoints)) 
			{
				int i = 0;

				cr.SetSourceSurface (waypoints, (int)(drag_position_x), (int)(drag_position_y));


				cr_overlay.SelectFontFace ("Courier", FontSlant.Normal, FontWeight.Bold);		
				cr_overlay.SetFontSize (16);

				if (GetWaypointList ().Count > 1) 
				{
					cr_overlay.LineWidth = 2;
					for(int j = 0; j < GetWaypointList ().Count-1; ++j)
					{
						cr_overlay.SetSourceRGB (0.5, 0.5, 0.5);  
						Coordinate way1 = GetWaypoint (j);
						Coordinate way2 = GetWaypoint (j+1);
						cr_overlay.MoveTo(CoordinateToPixel(way1));
						cr_overlay.LineTo(CoordinateToPixel(way2));
						cr_overlay.Stroke ();
					}
				}

				foreach(Coordinate c in GetWaypointList())
				{
					PointD cords = CoordinateToPixel (c);
					cr_overlay.SetSourceRGB (0.5, 0.5, 0.5);
					cr_overlay.Arc (cords.X,cords.Y, 12, 0, 2 * Math.PI);
					cr_overlay.Fill ();
					cords.X -= 5;				//to draw in center of circle
					cords.Y += 4;
					if (i >= 10) {
						cords.X -= 4;			//if number is 2-digit diffrent center
					}
					cr_overlay.MoveTo (cords);
					cr_overlay.SetSourceRGB (1.0, 1.0, 1.0);
					cr_overlay.TextPath (Convert.ToString (i));
					cr_overlay.Fill ();
					++i;
				}

			}
			cr.Paint ();
		}


		private void DrawPathPoint(Cairo.Context cr)		//simply drawing of path with red line
		{
			using (Context cr_path = new Context (waypoints)) 
			{
				if (GetPath().Count > 1) 
				{
					cr_path.LineWidth = 1;
					for(int i = 0; i < GetPath().Count-1; ++i)
					{
						cr_path.SetSourceRGB (1.0, 0.0, 0.0); 
						var list = GetPath();
						Coordinate way1 = list [i];
						Coordinate way2 = list [i+1];
						cr_path.MoveTo(CoordinateToPixel(way1));
						cr_path.LineTo(CoordinateToPixel(way2));
						cr_path.Stroke ();
					}
				}
			}
		}

		#endregion

		public PointD CoordinateToPixel(Coordinate p)
		{
			if (pixPerGradX == 0 || pixPerGradY == 0) {
				return new PointD (0, 0);
			}
			else {
				return new PointD
				(-Convert.ToInt32 ((p.Longitude - startPoint.Longitude) / pixPerGradX),
				 Convert.ToInt32 ((p.Latitude - startPoint.Latitude) / pixPerGradY));
			}
		}

		public Coordinate PixelToCoordinate(PointD p)
		{
			return new Coordinate(
				startPoint.Latitude + p.Y * pixPerGradY,
				startPoint.Longitude - p.X * pixPerGradX);
		}

		public override void JumpTo(Coordinate coordinate)		//centers map on the given coordinate
		{
			PointD temp = CoordinateToPixel (coordinate);
			drag_position_x = -(temp.X - Window.Width / 2);
			drag_position_y = -(temp.Y - Window.Height / 2);
			QueueDraw ();
		}

		public override Coordinate GetCurrentMapLocation ()		//gets the center of the current map location in gps coordinates
		{
			int width = Window.Width;
			int height = Window.Height;
			PointD temp = new PointD (width / 2 - drag_position_x, height / 2 - drag_position_y);
			Coordinate result = PixelToCoordinate (temp);
			return result;
		}

		public override bool PathPointFollowerMode 
		{ 
			get{ return PathPointFollowerMode;} 

			set{ this.PathPointFollowerMode = value;} 
		} 

		void OnPathpointAddedEventCalled(CoordinateEventHandler handler, CoordinateEventArgs args)
		{
			if (PathPointFollowerMode == true) 
			{
				JumpTo (args.Coordinate);
			}
		}
	}
}

