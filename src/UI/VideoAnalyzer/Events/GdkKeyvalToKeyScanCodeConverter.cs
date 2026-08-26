//
// Copyright (c) 2010-2026 Antmicro
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
//
using System.Collections.Generic;

using Antmicro.Renode.Peripherals.Input;

namespace Antmicro.Renode.Extensions.Analyzers.Video.Events
{
    // Fallback for keyvals the GDK keymap cannot resolve to a hardware keycode.
    // The quartz keymap only covers keys that produce characters: arrows, function
    // and navigation keys exist in its event translation but not in the reverse
    // keyval-to-keycode direction, so they never reach the keycode converters.
    // These are the layout-independent GDK special keyvals (the 0xFFxx block),
    // identical on every backend, so one table serves all platforms.
    internal class GdkKeyvalToKeyScanCodeConverter
    {
        static GdkKeyvalToKeyScanCodeConverter()
        {
            Instance = new GdkKeyvalToKeyScanCodeConverter();
        }

        public static GdkKeyvalToKeyScanCodeConverter Instance { get; private set; }

        public KeyScanCode? GetScanCode(uint fromValue)
        {
            KeyScanCode result;
            return ToScanCode.TryGetValue(fromValue, out result) ? (KeyScanCode?)result : null;
        }

        private readonly Dictionary<uint, KeyScanCode> ToScanCode = new Dictionary<uint, KeyScanCode> {
            { 0xff08, KeyScanCode.BackSpace },
            { 0xff09, KeyScanCode.Tab },
            { 0xff0d, KeyScanCode.Enter },
            { 0xff13, KeyScanCode.Pause },
            { 0xff14, KeyScanCode.ScrollLock },
            { 0xff1b, KeyScanCode.Escape },
            { 0xff50, KeyScanCode.Home },
            { 0xff51, KeyScanCode.Left },
            { 0xff52, KeyScanCode.Up },
            { 0xff53, KeyScanCode.Right },
            { 0xff54, KeyScanCode.Down },
            { 0xff55, KeyScanCode.PageUp },
            { 0xff56, KeyScanCode.PageDown },
            { 0xff57, KeyScanCode.End },
            { 0xff61, KeyScanCode.PrtSc },
            { 0xff63, KeyScanCode.Insert },
            { 0xff67, KeyScanCode.WinMenu },
            { 0xff7f, KeyScanCode.NumLock },
            { 0xff8d, KeyScanCode.KeypadEnter },
            { 0xffaa, KeyScanCode.KeypadMultiply },
            { 0xffab, KeyScanCode.KeypadPlus },
            { 0xffad, KeyScanCode.KeypadMinus },
            { 0xffae, KeyScanCode.KeypadComma },
            { 0xffaf, KeyScanCode.KeypadDivide },
            { 0xffb0, KeyScanCode.Keypad0 },
            { 0xffb1, KeyScanCode.Keypad1 },
            { 0xffb2, KeyScanCode.Keypad2 },
            { 0xffb3, KeyScanCode.Keypad3 },
            { 0xffb4, KeyScanCode.Keypad4 },
            { 0xffb5, KeyScanCode.Keypad5 },
            { 0xffb6, KeyScanCode.Keypad6 },
            { 0xffb7, KeyScanCode.Keypad7 },
            { 0xffb8, KeyScanCode.Keypad8 },
            { 0xffb9, KeyScanCode.Keypad9 },
            { 0xffbe, KeyScanCode.F1 },
            { 0xffbf, KeyScanCode.F2 },
            { 0xffc0, KeyScanCode.F3 },
            { 0xffc1, KeyScanCode.F4 },
            { 0xffc2, KeyScanCode.F5 },
            { 0xffc3, KeyScanCode.F6 },
            { 0xffc4, KeyScanCode.F7 },
            { 0xffc5, KeyScanCode.F8 },
            { 0xffc6, KeyScanCode.F9 },
            { 0xffc7, KeyScanCode.F10 },
            { 0xffc8, KeyScanCode.F11 },
            { 0xffc9, KeyScanCode.F12 },
            { 0xffe1, KeyScanCode.ShiftL },
            { 0xffe2, KeyScanCode.ShiftR },
            { 0xffe3, KeyScanCode.CtrlL },
            { 0xffe4, KeyScanCode.CtrlR },
            { 0xffe5, KeyScanCode.CapsLock },
            { 0xffe9, KeyScanCode.AltL },
            { 0xffea, KeyScanCode.AltR },
            { 0xffeb, KeyScanCode.WinL },
            { 0xffec, KeyScanCode.WinR },
            { 0xffff, KeyScanCode.Delete },
        };
    }
}
