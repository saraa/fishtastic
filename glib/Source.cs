// GLib.Source.cs - Source class implementation
//
// Author: Duncan Mak  <duncan@ximian.com>
//
// (c) 2002 Mike Kestner

namespace GLib {

	using System;
	using System.Runtime.InteropServices;

        public class Source {
		
		[DllImport("glib-2.0")]
		static extern bool g_source_remove (uint tag);

		public static bool Remove (uint tag)
		{
			return g_source_remove (tag);
		}
	}
}
