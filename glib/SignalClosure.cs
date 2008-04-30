// SignalClosure.cs - signal marshaling class
//
// Authors: Mike Kestner <mkestner@novell.com>
//
// Copyright (c) 2008 Novell, Inc.
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
	using System.Collections;
	using System.Runtime.InteropServices;

	internal  class ClosureInvokedArgs : EventArgs {

		EventArgs args;
		GLib.Object obj;
		object result;

		public ClosureInvokedArgs (GLib.Object obj, EventArgs args)
		{
			this.obj = obj;
			this.args = args;
		}

		public EventArgs Args {
			get {
				return args;
			}
		}

		public GLib.Object Target {
			get {
				return obj;
			}
		}
	}

	internal delegate void ClosureInvokedHandler (object o, ClosureInvokedArgs args);

	internal class SignalClosure : IDisposable {

		IntPtr handle;
		IntPtr raw_closure;
		string name;
		uint id = UInt32.MaxValue;
		System.Type args_type;

		static Hashtable closures = new Hashtable ();

		public SignalClosure (IntPtr obj, string signal_name, System.Type args_type)
		{
			raw_closure = glibsharp_closure_new (Marshaler, Notify, IntPtr.Zero);
			closures [raw_closure] = this;
			handle = obj;
			name = signal_name;
			this.args_type = args_type;
		}

		public event EventHandler Disposed;
		public event ClosureInvokedHandler Invoked;

		public void Connect (bool is_after)
		{
			IntPtr native_name = GLib.Marshaller.StringToPtrGStrdup (name);
			id = g_signal_connect_closure (handle, native_name, raw_closure, is_after);
			GLib.Marshaller.Free (native_name);
		}

		public void Disconnect ()
		{
			if (id != UInt32.MaxValue && g_signal_handler_is_connected (handle, id))
				g_signal_handler_disconnect (handle, id);
		}

		public void Dispose ()
		{
			Disconnect ();
			closures.Remove (raw_closure);
			if (Disposed != null)
				Disposed (this, EventArgs.Empty);
			GC.SuppressFinalize (this);
		}

		public void Invoke (ClosureInvokedArgs args)
		{
			if (Invoked == null)
				return;
			Invoked (this, args);
		}

		static ClosureMarshal marshaler;
		static ClosureMarshal Marshaler {
			get {
				if (marshaler == null)
					marshaler = new ClosureMarshal (MarshalCallback);
				return marshaler;
			}
		}

		[CDeclCallback]
		delegate void ClosureMarshal (IntPtr closure, IntPtr return_val, uint n_param_vals, IntPtr param_values, IntPtr invocation_hint, IntPtr marshal_data);

		static void MarshalCallback (IntPtr raw_closure, IntPtr return_val, uint n_param_vals, IntPtr param_values, IntPtr invocation_hint, IntPtr marshal_data)
		{
			string message = String.Empty;

			try {
				SignalClosure closure = closures [raw_closure] as SignalClosure;
				message = "Marshaling " + closure.name + " signal";
				Value objval = (Value) Marshal.PtrToStructure (param_values, typeof (Value));
				GLib.Object __obj = objval.Val as GLib.Object;
				if (__obj == null)
					return;

				if (closure.args_type == typeof (EventArgs)) {
					closure.Invoke (new ClosureInvokedArgs (__obj, EventArgs.Empty));
					return;
				}

				SignalArgs args = Activator.CreateInstance (closure.args_type, new object [0]) as SignalArgs;
				args.Args = new object [n_param_vals - 1];
				for (int i = 1; i < n_param_vals; i++) {
					IntPtr ptr = new IntPtr ((long)param_values + i * Marshal.SizeOf (typeof (Value)));
					Value val = (Value) Marshal.PtrToStructure (ptr, typeof (Value));
					args.Args [i - 1] = val.Val;
				}
				ClosureInvokedArgs ci_args = new ClosureInvokedArgs (__obj, args);
				closure.Invoke (ci_args);
				if (return_val == IntPtr.Zero || args.RetVal == null)
					return;

				Value ret = (Value) Marshal.PtrToStructure (return_val, typeof (Value));
				ret.Val = args.RetVal;
				Marshal.StructureToPtr (ret, return_val, false);
			} catch (Exception e) {
				Console.WriteLine (message);
				ExceptionManager.RaiseUnhandledException (e, false);
			}
		}

		[CDeclCallback]
		delegate void ClosureNotify (IntPtr data, IntPtr closure);

		static void NotifyCallback (IntPtr data, IntPtr raw_closure)
		{
			SignalClosure closure = closures [raw_closure] as SignalClosure;
			if (closure != null)
				closure.Dispose ();
		}

		static ClosureNotify notify_handler;
		static ClosureNotify Notify {
			get {
				if (notify_handler == null)
					notify_handler = new ClosureNotify (NotifyCallback);
				return notify_handler;
			}
		}

		[DllImport("glibsharpglue-2")]
		static extern IntPtr glibsharp_closure_new (ClosureMarshal marshaler, ClosureNotify notify, IntPtr gch);

		[DllImport("libgobject-2.0-0.dll")]
		static extern uint g_signal_connect_closure (IntPtr obj, IntPtr name, IntPtr closure, bool is_after);

		[DllImport("libgobject-2.0-0.dll")]
		static extern void g_signal_handler_disconnect (IntPtr instance, uint handler);

		[DllImport("libgobject-2.0-0.dll")]
		static extern bool g_signal_handler_is_connected (IntPtr instance, uint handler);
	}
}

