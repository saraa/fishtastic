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
			GenerationInfo gen_info = new GenerationInfo (NSElem);
			Generate (gen_info);
		}

		public void Generate (GenerationInfo gen_info)
		{
			StreamWriter sw = gen_info.Writer = gen_info.OpenStream (Name);

			sw.WriteLine ("namespace " + NS + " {");
			sw.WriteLine ();
			sw.WriteLine ("\tusing System;");
			sw.WriteLine ("\tusing System.Collections;");
			sw.WriteLine ("\tusing System.Runtime.InteropServices;");
			sw.WriteLine ();

			sw.WriteLine ("#region Autogenerated code");
			sw.Write ("\tpublic class {0} : GLib.Opaque", Name);
			sw.WriteLine (" {");
			sw.WriteLine ();

			GenMethods (gen_info, null, null);
			GenCtors (gen_info);
			sw.WriteLine ("#endregion");
			
			AppendCustom(sw, gen_info.CustomDir);

			sw.WriteLine ("\t}");
			sw.WriteLine ("}");

			sw.Close ();
			gen_info.Writer = null;
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

		protected override void GenCtors (GenerationInfo gen_info)
		{
			gen_info.Writer.WriteLine("\t\tpublic " + Name + "(IntPtr raw) : base(raw) {}");
			gen_info.Writer.WriteLine();

			base.GenCtors (gen_info);
		}

	}
}

