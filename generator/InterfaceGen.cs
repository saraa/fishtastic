// GtkSharp.Generation.InterfaceGen.cs - The Interface Generatable.
//
// Author: Mike Kestner <mkestner@speakeasy.net>
//
// (c) 2001-2002 Mike Kestner

namespace GtkSharp.Generation {

	using System;
	using System.IO;
	using System.Xml;

	public class InterfaceGen : ClassBase, IGeneratable  {

		public InterfaceGen (XmlElement ns, XmlElement elem) : base (ns, elem) {}

		public void Generate ()
		{
			StreamWriter sw = CreateWriter ();

			sw.WriteLine ("\tusing System;");
			sw.WriteLine ();

			sw.WriteLine("\t\t/// <summary> " + Name + " Interface</summary>");
			sw.WriteLine("\t\t/// <remarks>");
			sw.WriteLine("\t\t/// </remarks>");

			sw.WriteLine ("\tpublic interface " + Name + " : GLib.IWrapper {");
			sw.WriteLine ();
			
			foreach (Signal sig in sigs.Values) {
				if (sig.Validate ())
					sig.GenerateDecl (sw);
			}

			foreach (Method method in methods.Values) {
				if (IgnoreMethod (method))
					continue;

				if (method.Validate ())
					method.GenerateDecl (sw);
			}

			sw.WriteLine ("\t}");
			CloseWriter (sw);
			Statistics.IFaceCount++;
		}
	}
}

