// Object.cs - GtkObject class wrapper implementation
//
// Author: Mike Kestner <mkestner@speakeasy.net>
//
// (c) 2001 Mike Kestner

namespace GTK {

	using System;
	using System.Drawing;
	using System.Runtime.InteropServices;

	public abstract class Object  {

		protected IntPtr obj;

		protected delegate void SimpleCallback (IntPtr obj);

		[DllImport("gtk-1.3")]
		static extern void gtk_signal_connect_full (
					IntPtr obj, string evname,
					SimpleCallback cb, IntPtr unsupported, 
					IntPtr data, IntPtr destroycb, 
					int objsig, int after );


		protected void ConnectSignal (string name, SimpleCallback cb)
		{
			gtk_signal_connect_full (obj, name, cb,
					new IntPtr (0), new IntPtr (0), 
					new IntPtr (0), 0, 0);
		}


	}
}
