// GtkSharp.Generation.ObjectGen.cs - The Object Generatable.
//
// Author: Mike Kestner <mkestner@speakeasy.net>
//
// (c) 2001-2002 Mike Kestner

namespace GtkSharp.Generation {

	using System;
	using System.Collections;
	using System.IO;
	using System.Xml;

	public class OpaqueGen : ClassBase, IGeneratable  {

		public OpaqueGen (XmlElement ns, XmlElement elem) : base (ns, elem) 
		{
		}
	
		public override String FromNative(String var)
		{
			return "(" + QualifiedName + ") GLib.Opaque.GetOpaque(" + var + ")";
		}

		public override String FromNativeReturn(String var)
		{
			return FromNative (var);
		}
		
		public void Generate ()
		{
			StreamWriter sw = CreateWriter ();

			sw.WriteLine ("\tusing System;");
			sw.WriteLine ("\tusing System.Collections;");
			sw.WriteLine ("\tusing System.Runtime.InteropServices;");
			sw.WriteLine ();

			sw.WriteLine("\t\t/// <summary> " + Name + " Opaque Struct</summary>");
			sw.WriteLine("\t\t/// <remarks>");
			sw.WriteLine("\t\t/// </remarks>");
			sw.Write ("\tpublic class {0} : GLib.Opaque", Name);
			sw.WriteLine (" {");
			sw.WriteLine ();

			GenMethods (sw, null, null, true);
			GenCtors (sw);
			
			AppendCustom(sw);

			sw.WriteLine ("\t}");

			CloseWriter (sw);
			Statistics.OpaqueCount++;
		}

		private bool Validate ()
		{
			if (methods != null)
				foreach (Method method in methods.Values)
					if (!method.Validate()) {
						Console.WriteLine ("in Opaque" + QualifiedName);
						return false;
					}
			return true;
		}

		protected override void GenCtors (StreamWriter sw)
		{
			sw.WriteLine("\t\tpublic " + Name + "(IntPtr raw) : base(raw) {}");
			sw.WriteLine();

			base.GenCtors (sw);
		}

	}
}

