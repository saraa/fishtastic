// GtkSharp.Generation.Parameters.cs - The Parameters Generation Class.
//
// Author: Mike Kestner <mkestner@speakeasy.net>
//
// (c) 2001-2002 Mike Kestner

namespace GtkSharp.Generation {

	using System;
	using System.Collections;
	using System.IO;
	using System.Xml;

	public class Parameter {

		private XmlElement elem;

		public Parameter (XmlElement e)
		{
			elem = e;
		}

		public string CType {
			get {
				string type = elem.GetAttribute("type");
				if (type == "void*")
					type = "gpointer";
				return type;
			}
		}

		public string CSType {
			get {
				string cstype = SymbolTable.Table.GetCSType( elem.GetAttribute("type"));
				if (cstype == "void")
					cstype = "System.IntPtr";
				if (IsArray) {
					if (IsParams)
						cstype = "params " + cstype;
					cstype += "[]";
					cstype = cstype.Replace ("ref ", "");
				}
				return cstype;
			}
		}

		public IGeneratable Generatable {
			get {
				return SymbolTable.Table[CType];
			}
		}

		public bool IsArray {
			get {
				return elem.HasAttribute("array");
			}
		}

		public bool IsEllipsis {
			get {
				return elem.HasAttribute("ellipsis");
			}
		}

		public bool IsCount {
			get {
				
				if (Name.StartsWith("n_"))
					switch (CSType) {
					case "int":
					case "uint":
					case "long":
					case "ulong":
					case "short":
					case "ushort": 
						return true;
					default:
						return false;
					}
				else
					return false;
			}
		}

		public bool IsLength {
			get {
				
				if (Name.EndsWith("len") || Name.EndsWith("length"))
					switch (CSType) {
					case "int":
					case "uint":
					case "long":
					case "ulong":
					case "short":
					case "ushort": 
						return true;
					default:
						return false;
					}
				else
					return false;
			}
		}

		public bool IsParams {
			get {
				return elem.HasAttribute("params");
			}
		}

		public bool IsString {
			get {
				return (CSType == "string");
			}
		}

		public bool IsUserData {
			get {
				return CSType == "IntPtr" && (Name.EndsWith ("data") || Name.EndsWith ("data_or_owner"));
			}
		}

		public string MarshalType {
			get {
				string type = SymbolTable.Table.GetMarshalType( elem.GetAttribute("type"));
				if (type == "void")
					type = "System.IntPtr";
				if (IsArray) {
					type += "[]";
					type = type.Replace ("ref ", "");
				}
				return type;
			}
		}

		public string Name {
			get {
				return SymbolTable.Table.MangleName (elem.GetAttribute("name"));
			}
		}

		public bool NullOk {
			get {
				return elem.HasAttribute ("null_ok");
			}
		}

		public string PropertyName {
			get {
				return elem.GetAttribute("property_name");
			}
		}

		public string PassAs {
			get {
				if (elem.HasAttribute ("pass_as"))
					return elem.GetAttribute ("pass_as");

				if (IsArray)
					return "";

				if (Generatable is SimpleGen && !(Generatable is ConstStringGen) && CType.EndsWith ("*") && !CSType.EndsWith ("IntPtr"))
					return "out";

				if (Generatable is EnumGen && CType.EndsWith ("*"))
					return "out";

				return "";
			}
		}

		public string CallByName (string call_parm_name)
		{
			string call_parm;
			if (Generatable is CallbackGen)
				call_parm = SymbolTable.Table.CallByName (CType, call_parm_name + "_wrapper");
			else
				call_parm = SymbolTable.Table.CallByName(CType, call_parm_name);
			
			if (NullOk && !CSType.EndsWith ("IntPtr") && !(Generatable is StructBase))
				call_parm = String.Format ("({0} != null) ? {1} : {2}", call_parm_name, call_parm, Generatable is CallbackGen ? "null" : "IntPtr.Zero");

			if (IsArray)
				call_parm = call_parm.Replace ("ref ", "");

			return call_parm;

		}

		public string StudlyName {
			get {
				string name = elem.GetAttribute("name");
				string[] segs = name.Split('_');
				string studly = "";
				foreach (string s in segs) {
					if (s.Trim () == "")
						continue;
					studly += (s.Substring(0,1).ToUpper() + s.Substring(1));
				}
				return studly;
				
			}
		}
	}

	public class Parameters  {
		
		private ArrayList param_list;
		private XmlElement elem;
		private string impl_ns;
		private bool hide_data;
		private bool is_static;

		public Parameters (XmlElement elem, string impl_ns) {
			
			this.elem = elem;
			this.impl_ns = impl_ns;
			param_list = new ArrayList ();
			foreach (XmlNode node in elem.ChildNodes) {
				XmlElement parm = node as XmlElement;
				if (parm != null && parm.Name == "parameter")
					param_list.Add (new Parameter (parm));
			}
		}

		public int Count {
			get {
				return param_list.Count;
			}
		}

		public Parameter this [int idx] {
			get {
				return param_list [idx] as Parameter;
			}
		}

		public bool HideData {
			get { return hide_data; }
			set { hide_data = value; }
		}

		public bool Static {
			get { return is_static; }
			set { is_static = value; }
		}

		public bool Validate ()
		{
			foreach (Parameter p in param_list) {
				
				if (p.IsEllipsis) {
					Console.Write("Ellipsis parameter ");
					return false;
				}

				if ((p.CSType == "") || (p.Name == "") || 
				    (p.MarshalType == "") || (SymbolTable.Table.CallByName(p.CType, p.Name) == "")) {
					Console.Write("Name: " + p.Name + " Type: " + p.CType + " ");
					return false;
				}
			}
			
			return true;
		}

		public bool IsAccessor {
			get {
				return Count == 1 && this [0].PassAs == "out";
			}
		}

		public string AccessorReturnType {
			get {
				if (Count > 0)
					return this [0].CSType;
				else
					return null;
			}
		}

		public string AccessorName {
			get {
				if (Count > 0)
					return this [0].Name;
				else
					return null;
			}
		}

	}
}

