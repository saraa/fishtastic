// GtkSharp.Generation.StringGen.cs - The String type Generatable.
//
// Author: Rachel Hestilow <rachel@nullenvoid.com>
//
// (c) 2003 Rachel Hestilow

namespace GtkSharp.Generation {

	using System;

	public class StringGen : ConstStringGen {

		public StringGen (string ctype) : base (ctype)
		{
		}
	
		public override String FromNativeReturn(String var)
		{
			return "GLibSharp.Marshaller.PtrToStringGFree(" + var + ")";
		}

		public override String ToNativeReturn(String var)
		{
			return "GLibSharp.Marshaller.StringToPtrGStrdup(" + var + ")";
		}
	}
}
