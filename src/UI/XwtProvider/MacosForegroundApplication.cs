//
// Copyright (c) 2010-2026 Antmicro
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
//
using System;
using System.Runtime.InteropServices;

using Antmicro.Renode.Logging;

namespace Antmicro.Renode.UI
{
    public static class MacosForegroundApplication
    {
        // An unbundled executable on macOS is not an application to LaunchServices:
        // its windows draw, but the process cannot be activated, so no window of
        // it can ever take keyboard focus — not by click, not by Dock, not by
        // System Events. GTK's quartz backend tries to fix this by calling the
        // legacy TransformProcessType at display-open and ignores its result,
        // which leaves the process half-registered (an ASN with no check-in).
        // Setting the activation policy from inside the process completes the
        // registration; it must run after the toolkit created NSApp, on the
        // main thread — Initialize() satisfies both.
        public static void Ensure()
        {
            if(!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return;
            }
            try
            {
                // sharedApplication returns the NSApp the toolkit created; it never
                // returns nil (it would create one, which is why this must only run
                // after Application.Initialize).
                var nsApplication = objc_msgSend(objc_getClass("NSApplication"), sel_registerName("sharedApplication"));
                // setActivationPolicy: returns NO when the transformation is deferred,
                // which is the normal case before the application finishes launching;
                // the policy still applies, so the result is not a failure signal.
                objc_msgSend_bool_ret(nsApplication, sel_registerName("setActivationPolicy:"), (IntPtr)NSApplicationActivationPolicyRegular);
                objc_msgSend_bool_arg(nsApplication, sel_registerName("activateIgnoringOtherApps:"), true);
            }
            catch(Exception e)
            {
                Logger.Log(LogLevel.Warning, "Could not become a foreground application: {0}; windows may not take focus", e.Message);
            }
        }

        [DllImport(LibObjc, EntryPoint = "objc_getClass")]
        private static extern IntPtr objc_getClass(string name);

        [DllImport(LibObjc, EntryPoint = "sel_registerName")]
        private static extern IntPtr sel_registerName(string name);

        [DllImport(LibObjc, EntryPoint = "objc_msgSend")]
        private static extern IntPtr objc_msgSend(IntPtr receiver, IntPtr selector);

        [DllImport(LibObjc, EntryPoint = "objc_msgSend")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool objc_msgSend_bool_ret(IntPtr receiver, IntPtr selector, IntPtr argument);

        [DllImport(LibObjc, EntryPoint = "objc_msgSend")]
        private static extern void objc_msgSend_bool_arg(IntPtr receiver, IntPtr selector, [MarshalAs(UnmanagedType.I1)] bool argument);

        private const int NSApplicationActivationPolicyRegular = 0;
        private const string LibObjc = "/usr/lib/libobjc.A.dylib";
    }
}
