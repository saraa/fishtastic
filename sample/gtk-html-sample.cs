// mcs -pkg:gtkhtml-sharp -pkg:gtk-sharp gtk-html-sample.cs
using Gtk;
using System;
using System.IO;

class HTMLSample {
	static int Main (string [] args)
	{
		HTML html;
		Window win;
		Application.Init ();
		html = new HTML ();
		win = new Window ("Test");
		win.Add (html);
		HTMLStream s = html.Begin ("text/html");

		if (args.Length > 0){
			StreamReader r = new StreamReader (File.OpenRead (args [0]));
			s.Write (r.ReadToEnd ());
		} else {
			s.Write ("<html><body>");
			s.Write ("Hello world!");
		}
		
		html.End (s, HTMLStreamStatus.Ok);
		win.ShowAll ();
		Application.Run ();
		return 0;
	}
}

