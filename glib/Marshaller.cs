// GLibSharp.Marshaller.cs : Marshalling utils 
//
// Author: Rachel Hestilow <rachel@nullenvoid.com>
//         Mike Kestner  <mkestner@ximian.com>
//
// Copyright (c) 2002, 2003 Rachel Hestilow
// Copyright (c) 2004 Novell, Inc.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of version 2 of the Lesser GNU General 
// Public License as published by the Free Software Foundation.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this program; if not, write to the
// Free Software Foundation, Inc., 59 Temple Place - Suite 330,
// Boston, MA 02111-1307, USA.


namespace GLib {
	using System;
	using System.Runtime.InteropServices;
	
	public class Marshaller {

		private Marshaller () {}
		
		[DllImport("libglib-2.0-0.dll")]
		static extern void g_free (IntPtr mem);

		public static string PtrToStringGFree (IntPtr ptr) {
			string ret = Marshal.PtrToStringAnsi (ptr);
			g_free (ptr);
			return ret;
		}

		[DllImport("libglib-2.0-0.dll")]
		static extern void g_strfreev (IntPtr mem);

		public static string[] PtrToStringGFree (IntPtr[] ptrs) {
			// The last pointer is a null terminator.
			string[] ret = new string[ptrs.Length - 1];
			for (int i = 0; i < ret.Length; i++) {
				ret[i] = Marshal.PtrToStringAnsi (ptrs[i]);
				g_free (ptrs[i]);
			}
			return ret;
		}

		[DllImport("libglib-2.0-0.dll")]
		static extern IntPtr g_strdup (string str);

		public static IntPtr StringToPtrGStrdup (string str) {
			return g_strdup (str);
		}

		// Argv marshalling -- unpleasantly complex, but
		// don't know of a better way to do it.
		//
		// Currently, the 64-bit cleanliness is
		// hypothetical. It's also ugly, but I don't know of a
		// construct to handle both 32 and 64 bitness
		// transparently, since we need to alloc buffers of
		// [native pointer size] * [count] bytes.

		[DllImport("libglib-2.0-0.dll")]
		static extern IntPtr g_malloc(ulong size);

		static bool check_sixtyfour () {
			int szint = Marshal.SizeOf (typeof (int));
			int szlong = Marshal.SizeOf (typeof (long));
			int szptr = IntPtr.Size;

			if (szptr == szint)
				return false;
			if (szptr == szlong)
				return true;

			throw new Exception ("Pointers are neither int- nor long-sized???");
		}

		static IntPtr make_buf_32 (string[] args) 
		{
			int[] ptrs = new int[args.Length];

			for (int i = 0; i < args.Length; i++)
				ptrs[i] = (int) Marshal.StringToHGlobalAuto (args[i]);

			IntPtr buf = g_malloc ((ulong) Marshal.SizeOf(typeof(int)) * 
					       (ulong) args.Length);
			Marshal.Copy (ptrs, 0, buf, ptrs.Length);
			return buf;
		}

		static IntPtr make_buf_64 (string[] args) 
		{
			long[] ptrs = new long[args.Length];

			for (int i = 0; i < args.Length; i++)
				ptrs[i] = (long) Marshal.StringToHGlobalAuto (args[i]);
				
			IntPtr buf = g_malloc ((ulong) Marshal.SizeOf(typeof(long)) * 
					       (ulong) args.Length);
			Marshal.Copy (ptrs, 0, buf, ptrs.Length);
			return buf;
		}

		public static IntPtr ArgvToArrayPtr (string[] args)
		{
			if (args.Length == 0)
				return IntPtr.Zero;

			if (check_sixtyfour ())
				return make_buf_64 (args);

			return make_buf_32 (args);
		}

		// should we be freeing these pointers? they're marshalled
		// from our own strings, so I think not ...

		static string[] unmarshal_32 (IntPtr buf, int argc)
		{
			int[] ptrs = new int[argc];
			string[] args = new string[argc];

			Marshal.Copy (buf, ptrs, 0, argc);

			for (int i = 0; i < ptrs.Length; i++)
				args[i] = Marshal.PtrToStringAuto ((IntPtr) ptrs[i]);
			
			return args;
		}

		static string[] unmarshal_64 (IntPtr buf, int argc)
		{
			long[] ptrs = new long[argc];
			string[] args = new string[argc];

			Marshal.Copy (buf, ptrs, 0, argc);

			for (int i = 0; i < ptrs.Length; i++)
				args[i] = Marshal.PtrToStringAuto ((IntPtr) ptrs[i]);
			
			return args;
		}		

		public static string[] ArrayPtrToArgv (IntPtr array, int argc)
		{
			if (argc == 0)
				return new string[0];

			if (check_sixtyfour ())
				return unmarshal_64 (array, argc);

			return unmarshal_32 (array, argc);
		}

		static DateTime local_epoch = new DateTime (1970, 1, 1, 0, 0, 0);
		static int utc_offset = (int) (DateTime.Now.Subtract (DateTime.UtcNow).TotalSeconds);

		public static IntPtr DateTimeTotime_t (DateTime time)
		{
			return new IntPtr (((int)time.Subtract (local_epoch).TotalSeconds));
		}

		public static DateTime time_tToDateTime (IntPtr time_t)
		{
			return local_epoch.AddSeconds ((int)time_t + utc_offset);
		}

		[DllImport("glibsharpglue-2.0")]
		static extern IntPtr gtksharp_unichar_to_utf8_string (uint c);

		public static char GUnicharToChar (uint ucs4_char)
		{ 
			IntPtr raw_ret = gtksharp_unichar_to_utf8_string (ucs4_char);
			string ret = GLib.Marshaller.PtrToStringGFree(raw_ret);
			if (ret.Length > 1)
				throw new ArgumentOutOfRangeException ("ucs4char is not representable by a char.");

			return ret [0];
		}

		[DllImport("glibsharpglue-2.0")]
		static extern uint glibsharp_utf16_to_gunichar (ushort c);

		public static uint CharToGUnichar (char c)
		{
			return glibsharp_utf16_to_gunichar ((ushort) c);
		}

	}
}

