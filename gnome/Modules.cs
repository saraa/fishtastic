namespace Gnome
{
	using System;
	using System.Runtime.InteropServices;

	class Modules
	{
		[DllImport("libgnome-2.so.0")]
		static extern System.IntPtr libgnome_module_info_get ();
		[DllImport("libgnomeui-2.so.0")]
		static extern System.IntPtr libgnomeui_module_info_get ();

		public static ModuleInfo LibGnome {
			get { return new ModuleInfo (libgnome_module_info_get ()); }
		}

		public static ModuleInfo UI {
			get { return new ModuleInfo (libgnomeui_module_info_get ()); }
		}
	}
}

