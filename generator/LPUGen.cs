// GtkSharp.Generation.LPUGen.cs - unsugned long/pointer generatable.
//
// Author: Mike Kestner <mkestner@novell.com>
//
// Copyright (c) 2004 Novell, Inc.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of version 2 of the GNU General Public
// License as published by the Free Software Foundation.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// General Public License for more details.
//
// You should have received a copy of the GNU General Public
// License along with this program; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.


namespace GtkSharp.Generation {

	using System;
	using System.IO;

	public class LPUGen : SimpleGen, IAccessor {
		
		public LPUGen (string ctype) : base (ctype, "ulong", "0") {}

		public override string MarshalType {
			get {
#if WIN64LONGS
				return "uint";
#else
				return "UIntPtr";
#endif
			}
		}

		public override string CallByName (string var_name)
		{
#if WIN64LONGS
			return "(uint) " + var_name;
#else
			return "new UIntPtr (" + var_name + ")";
#endif
		}
		
		public override string FromNative(string var)
		{
#if WIN64LONGS
			return var;
#else
			return "(ulong) " + var;
#endif
		}

		public void WriteAccessors (StreamWriter sw, string indent, string var)
		{
			sw.WriteLine (indent + "get {");
			sw.WriteLine (indent + "\treturn " + FromNative (var) + ";");
			sw.WriteLine (indent + "}");
			sw.WriteLine (indent + "set {");
			sw.WriteLine (indent + "\t" + var + " = " + CallByName ("value") + ";");
			sw.WriteLine (indent + "}");
		}
	}
}

