// Subclass.cs - Widget subclass Test implementation
//
// Author: Mike Kestner <mkestner@speakeasy.net>
//
// (c) 2001-2002 Mike Kestner

namespace GtkSamples {

	using Gtk;
	using GtkSharp;
	using System;
	using System.Drawing;

	public class ButtonApp  {

		public static int Main (string[] args)
		{
			Application.Init ();
			Window win = new Window ("Button Tester");
			win.DefaultSize = new Size (200, 150);
			win.DeleteEvent += new DeleteEventHandler (Window_Delete);
			Button btn = new MyButton ();
			btn.Label = "I'm a subclassed button";
			btn.Clicked += new EventHandler (btn_click);
			win.Add (btn);
			win.ShowAll ();
			Application.Run ();
			return 0;
		}

		static void btn_click (object obj, EventArgs args)
		{
			Console.WriteLine ("Button Clicked");
		}

		static void Window_Delete (object obj, DeleteEventArgs args)
		{
			Application.Quit ();
			args.RetVal = true;
		}

	}

	public class MyButton : Gtk.Button {

		static GLib.GType gtype = GLib.GType.Invalid;

		public MyButton () : base (GType) {}

		public static new GLib.GType GType {
			get {
				if (gtype == GLib.GType.Invalid)
					gtype = RegisterGType (typeof (MyButton));
				return gtype;
			}
		}
	}
}
