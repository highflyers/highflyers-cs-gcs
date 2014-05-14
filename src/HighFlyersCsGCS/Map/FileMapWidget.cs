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

		public FileMapWidget () : base ()
		{
			AddWaypoint (new Coordinate (1, 2));
			AddWaypoint (new Coordinate (2, 2));
			AddWaypoint (new Coordinate (3, 3));

			SetSizeRequest(100, 100);

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
			SetPosition (end_mouse_x - mouse_x, end_mouse_y - mouse_y);
			QueueDraw ();

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
			return true;
		}

		#region Drawing

		protected override bool OnDrawn (Cairo.Context cr)
		{
			//cr.Antialias = Antialias.Default;
			cr.Scale( 5, 5);
			Gdk.CairoHelper.SetSourcePixbuf (cr, mapImage.Pixbuf, drag_position_x/5, drag_position_y/5);			//divided by scale
			cr.Paint();
			cr.Scale( 1, 1);
			DrawWaypoints (cr);
			return true;  
		} 

		private void DrawWaypoints(Cairo.Context cr)
		{
			Surface waypoints;
			using (var target = cr.GetTarget ()) 
			{
				waypoints   = target.CreateSimilar (Content.ColorAlpha, AllocatedWidth, AllocatedHeight);
			}

			using (Context cr_overlay = new Context (waypoints)) 
			{
				int i = 0;
				cr.SetSourceSurface (waypoints, (int)drag_position_x/5, (int)drag_position_y/5);


				cr_overlay.SelectFontFace ("Courier", FontSlant.Normal, FontWeight.Bold);		//TODO antialiasing for text is not working
				cr_overlay.SetFontSize (11);
				foreach(Coordinate c in GetWaypointList())
				{
					PointD cords = CoordinateToPixel (c);
					cr_overlay.SetSourceRGB (0.5, 0.5, 0.5);
					cr_overlay.Arc (cords.X,cords.Y, 6, 0, 2 * Math.PI);
					cr_overlay.Fill ();
					cords.X -= 3;				//to draw in center of circle
					cords.Y += 3;
					cr_overlay.MoveTo (cords);
					cr_overlay.SetSourceRGB (1.0, 1.0, 1.0);
					cr_overlay.TextPath (Convert.ToString (i));
					cr_overlay.Fill ();
					++i;
				}
				if (GetWaypointList ().Count > 1) 
				{
					for(int j = 0; j < GetWaypointList ().Count-1; ++j)
					{
						cr_overlay.SetSourceRGB (0.0, 1.0, 0.0);				//TODO make it draw  
						Coordinate way1 = GetWaypoint (j);
						Coordinate way2 = GetWaypoint (j+1);
						cr_overlay.MoveTo(CoordinateToPixel(way1));
						cr_overlay.LineTo(CoordinateToPixel(way2));
						cr_overlay.Fill ();
					}


				}
			}


			cr.Paint ();
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

		public override void JumpTo(Coordinate coordinate)
		{

		}

		public override Coordinate GetCurrentMapLocation ()
		{
			return new Coordinate (0, 0);
		}

		public override bool PathPointFollowerMode{ get; set; }
	}
}

