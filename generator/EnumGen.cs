// GtkSharp.Generation.EnumGen.cs - The Enumeration Generatable.
//
// Author: Mike Kestner <mkestner@speakeasy.net>
//
// (c) 2001 Mike Kestner

namespace GtkSharp.Generation {

	using System;
	using System.IO;
	using System.Xml;

	public class EnumGen : GenBase, IGeneratable  {
		
		public EnumGen (XmlElement ns, XmlElement elem) : base (ns, elem) {}
		
		public String MarshalType {
			get
			{
				return "int";
			}
		}
		public String MarshalReturnType {
			get
			{
				return MarshalType;
			}
		}

		public String CallByName (String var_name)
		{
			return "(int) " + var_name;
		}
		
		public String FromNative(String var)
		{
			return "(" + QualifiedName + ")" + var;
		}
		
		public String FromNativeReturn(String var)
		{
			return FromNative (var);
		}

		public virtual String ToNativeReturn(String var)
		{
			return CallByName (var);
		}
		
		public void Generate ()
		{
			if (!DoGenerate)
				return;

			StreamWriter sw = CreateWriter ();

			if (Elem.GetAttribute("type") == "flags") {
				sw.WriteLine ();
				sw.WriteLine ("\t[Flags]");
			}

			// Ok, this is obscene.  We need to go through the enums first
			// to find "large" values.  If we find some, we need to change
			// the base type of the enum.

			string enum_type = null;

			foreach (XmlNode node in Elem.ChildNodes) {
				if (!(node is XmlElement) || node.Name != "member") {
					continue;
				}

				XmlElement member = (XmlElement) node;

				if (member.HasAttribute("value")) {
					string value = member.GetAttribute("value");
					if (value.EndsWith("U")) {
						enum_type = "uint";
						member.SetAttribute("value", value.TrimEnd('U'));
					}
				}
			}

			sw.WriteLine ("#region Autogenerated code");
					
			if (enum_type != null)
				sw.WriteLine ("\tpublic enum " + Name + " : " + enum_type + " {");
			else
				sw.WriteLine ("\tpublic enum " + Name + " {");

			sw.WriteLine ();
				
			foreach (XmlNode node in Elem.ChildNodes) {
				if (!(node is XmlElement) || node.Name != "member") {
					continue;
				}
				
				XmlElement member = (XmlElement) node;

				sw.Write ("\t\t" + member.GetAttribute("name"));
				if (member.HasAttribute("value")) {
					sw.WriteLine (" = " + member.GetAttribute("value") + ",");
				} else {
					sw.WriteLine (",");
				}
			}

			sw.WriteLine ("\t}");
			sw.WriteLine ("#endregion");
			CloseWriter (sw);
			Statistics.EnumCount++;
		}
		
	}
}

