//
// Copyright (c) 2010-2021 Antmicro
// Copyright (c) 2011-2015 Realtime Embedded
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
//

#pragma warning disable IDE0005
using System;
using System.Runtime.InteropServices;
#pragma warning restore IDE0005

using Antmicro.Renode.Extensions.Analyzers.Video.Handlers;
using Antmicro.Renode.Logging;
using Antmicro.Renode.Peripherals.Input;

using Xwt;

namespace Antmicro.Renode.Extensions.Analyzers.Video.Events
{
    internal class XWTEventSource : IEventSource
    {
        public XWTEventSource(Widget source)
        {
            this.source = source;
        }

        public void DetachHandler()
        {
            source.MouseMoved -= HandleMouseMoved;
            source.ButtonPressed -= HandleButtonPressed;
            source.ButtonReleased -= HandleButtonReleased;
            source.KeyPressed -= HandleKeyPressed;
            source.KeyReleased -= HandleKeyReleased;

            handler = null;
        }

        public void AttachHandler(IOHandler h)
        {
            handler = h;

            source.MouseMoved += HandleMouseMoved;
            source.ButtonPressed += HandleButtonPressed;
            source.ButtonReleased += HandleButtonReleased;
            source.KeyPressed += HandleKeyPressed;
            source.KeyReleased += HandleKeyReleased;
        }

        public int X { get { return lastX ?? 0; } }

        public int Y { get { return lastY ?? 0; } }

        private void HandleKeyReleased(object sender, KeyEventArgs e)
        {
#if !GUI_DISABLED
            e.Handled = true;
#if PLATFORM_WINDOWS
            var keyboardLayout = GetKeyboardLayout(0);
            var vks = VkKeyScanEx((char)e.Key, keyboardLayout);
            var vsc = MapVirtualKeyEx((uint)vks & 0xff, MAPVK_VK_TO_VSC, keyboardLayout);
            var key = WPFToKeyScanCodeConverter.Instance.GetScanCode((int)vsc, e.Key);
#else
            var key = GetScanCodeForKeyval((uint)e.Key);
#endif // !PLATFORM_WINDOWS
            if(key != null)
            {
                handler.KeyReleased(key.Value);
                return;
            }
#endif // !GUI_DISABLED
            Logger.LogAs(this, LogLevel.Warning, "Unhandled keycode: {0}", e.Key);
        }

        private void HandleKeyPressed(object sender, KeyEventArgs e)
        {
#if !GUI_DISABLED
            e.Handled = true;
#if PLATFORM_WINDOWS
            var keyboardLayout = GetKeyboardLayout(0);
            var vks = VkKeyScanEx((char)e.Key, keyboardLayout);
            var vsc = MapVirtualKeyEx((uint)vks & 0xff, MAPVK_VK_TO_VSC, keyboardLayout);
            var key = WPFToKeyScanCodeConverter.Instance.GetScanCode((int)vsc, e.Key);
#else
            var key = GetScanCodeForKeyval((uint)e.Key);
#endif // !PLATFORM_WINDOWS
            if(key != null)
            {
                handler.KeyPressed(key.Value);
                return;
            }
#endif // !GUI_DISABLED
            Logger.LogAs(this, LogLevel.Warning, "Unhandled keycode: {0}", e.Key);
        }

#if !GUI_DISABLED && !PLATFORM_WINDOWS
        // The GDK keymap reports the platform's hardware keycodes: X11 keycodes on
        // Linux, Carbon virtual keycodes (kVK_*) on macOS — each needs its own table.
        // Keyvals the keymap cannot resolve to a keycode (on quartz that is every
        // non-character key: arrows, navigation, function keys) fall back to the
        // layout-independent GDK keyval table.
        private static KeyScanCode? GetScanCodeForKeyval(uint keyval)
        {
            var entries = Gdk.Keymap.Default.GetEntriesForKeyval(keyval);
            if(entries.Length > 0)
            {
                var keycode = (int)entries[0].Keycode;
                var scanCode = RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
                    ? MacosToKeyScanCodeConverter.Instance.GetScanCode(keycode)
                    : X11ToKeyScanCodeConverter.Instance.GetScanCode(keycode);
                if(scanCode != null)
                {
                    return scanCode;
                }
            }
            return GdkKeyvalToKeyScanCodeConverter.Instance.GetScanCode(keyval);
        }
#endif

        private void HandleButtonReleased(object sender, ButtonEventArgs e)
        {
            if(!e.Handled)
            {
                handler.ButtonReleased(e.Button);
                e.Handled = true;
            }
        }

        private void HandleButtonPressed(object sender, ButtonEventArgs e)
        {
            if(!e.Handled)
            {
                handler.ButtonPressed(e.Button);
                e.Handled = true;
            }
        }

        private void HandleMouseMoved(object sender, MouseMovedEventArgs e)
        {
            if(lastX == null || lastY == null)
            {
                lastX = (int)e.X;
                lastY = (int)e.Y;
                return;
            }

            handler.MouseMoved((int)e.X, (int)e.Y, lastX.Value - (int)e.X, lastY.Value - (int)e.Y);
            lastX = (int)e.X;
            lastY = (int)e.Y;
        }

        private IOHandler handler;

        private int? lastX;
        private int? lastY;

        private readonly Widget source;

#if PLATFORM_WINDOWS
        [DllImport("user32.dll")]
        private static extern uint MapVirtualKeyEx(uint uCode, uint uMapType, IntPtr dwhkl);
        [DllImport("user32.dll")]
        private static extern uint MapVirtualKeyW(uint uCode, uint uMapType);
        [DllImport("user32.dll")]
        private static extern IntPtr GetKeyboardLayout(uint idThread);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short VkKeyScan(char ch);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short VkKeyScanEx(char ch, IntPtr dwhkl);

        private const uint MAPVK_VK_TO_VSC = 0x00;
        private const uint MAPVK_VSC_TO_VK = 0x01;
        private const uint MAPVK_VK_TO_CHAR = 0x02;
        private const uint MAPVK_VSC_TO_VK_EX = 0x03;
        private const uint MAPVK_VK_TO_VSC_EX = 0x04;
#endif
    }
}