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
    // On macOS the GDK keymap reports the Carbon virtual keycodes (the kVK_*
    // constants from HIToolbox Events.h), a fixed layout-independent numbering
    // that shares nothing with X11's — so the display analyzer needs this table
    // where Linux uses X11ToKeyScanCodeConverter.
    internal class MacosToKeyScanCodeConverter
    {
        static MacosToKeyScanCodeConverter()
        {
            Instance = new MacosToKeyScanCodeConverter();
        }

        public static MacosToKeyScanCodeConverter Instance { get; private set; }

        public KeyScanCode? GetScanCode(int fromValue)
        {
            KeyScanCode result;
            return ToScanCode.TryGetValue(fromValue, out result) ? (KeyScanCode?)result : null;
        }

        private readonly Dictionary<int, KeyScanCode> ToScanCode = new Dictionary<int, KeyScanCode> {
            { 0x00, KeyScanCode.A },
            { 0x01, KeyScanCode.S },
            { 0x02, KeyScanCode.D },
            { 0x03, KeyScanCode.F },
            { 0x04, KeyScanCode.H },
            { 0x05, KeyScanCode.G },
            { 0x06, KeyScanCode.Z },
            { 0x07, KeyScanCode.X },
            { 0x08, KeyScanCode.C },
            { 0x09, KeyScanCode.V },
            { 0x0b, KeyScanCode.B },
            { 0x0c, KeyScanCode.Q },
            { 0x0d, KeyScanCode.W },
            { 0x0e, KeyScanCode.E },
            { 0x0f, KeyScanCode.R },
            { 0x10, KeyScanCode.Y },
            { 0x11, KeyScanCode.T },
            { 0x12, KeyScanCode.Number1 },
            { 0x13, KeyScanCode.Number2 },
            { 0x14, KeyScanCode.Number3 },
            { 0x15, KeyScanCode.Number4 },
            { 0x16, KeyScanCode.Number6 },
            { 0x17, KeyScanCode.Number5 },
            { 0x18, KeyScanCode.OemPlus },
            { 0x19, KeyScanCode.Number9 },
            { 0x1a, KeyScanCode.Number7 },
            { 0x1b, KeyScanCode.OemMinus },
            { 0x1c, KeyScanCode.Number8 },
            { 0x1d, KeyScanCode.Number0 },
            { 0x1e, KeyScanCode.OemCloseBrackets },
            { 0x1f, KeyScanCode.O },
            { 0x20, KeyScanCode.U },
            { 0x21, KeyScanCode.OemOpenBrackets },
            { 0x22, KeyScanCode.I },
            { 0x23, KeyScanCode.P },
            { 0x24, KeyScanCode.Enter },
            { 0x25, KeyScanCode.L },
            { 0x26, KeyScanCode.J },
            { 0x27, KeyScanCode.OemQuotes },
            { 0x28, KeyScanCode.K },
            { 0x29, KeyScanCode.OemSemicolon },
            { 0x2a, KeyScanCode.OemPipe },
            { 0x2b, KeyScanCode.OemComma },
            { 0x2c, KeyScanCode.OemQuestion },
            { 0x2d, KeyScanCode.N },
            { 0x2e, KeyScanCode.M },
            { 0x2f, KeyScanCode.OemPeriod },
            { 0x30, KeyScanCode.Tab },
            { 0x31, KeyScanCode.Space },
            { 0x32, KeyScanCode.Tilde },
            { 0x33, KeyScanCode.BackSpace },
            { 0x35, KeyScanCode.Escape },
            { 0x36, KeyScanCode.WinR },
            { 0x37, KeyScanCode.WinL },
            { 0x38, KeyScanCode.ShiftL },
            { 0x39, KeyScanCode.CapsLock },
            { 0x3a, KeyScanCode.AltL },
            { 0x3b, KeyScanCode.CtrlL },
            { 0x3c, KeyScanCode.ShiftR },
            { 0x3d, KeyScanCode.AltR },
            { 0x3e, KeyScanCode.CtrlR },
            { 0x41, KeyScanCode.KeypadComma },
            { 0x43, KeyScanCode.KeypadMultiply },
            { 0x45, KeyScanCode.KeypadPlus },
            { 0x47, KeyScanCode.NumLock },
            { 0x4b, KeyScanCode.KeypadDivide },
            { 0x4c, KeyScanCode.KeypadEnter },
            { 0x4e, KeyScanCode.KeypadMinus },
            { 0x52, KeyScanCode.Keypad0 },
            { 0x53, KeyScanCode.Keypad1 },
            { 0x54, KeyScanCode.Keypad2 },
            { 0x55, KeyScanCode.Keypad3 },
            { 0x56, KeyScanCode.Keypad4 },
            { 0x57, KeyScanCode.Keypad5 },
            { 0x58, KeyScanCode.Keypad6 },
            { 0x59, KeyScanCode.Keypad7 },
            { 0x5b, KeyScanCode.Keypad8 },
            { 0x5c, KeyScanCode.Keypad9 },
            { 0x60, KeyScanCode.F5 },
            { 0x61, KeyScanCode.F6 },
            { 0x62, KeyScanCode.F7 },
            { 0x63, KeyScanCode.F3 },
            { 0x64, KeyScanCode.F8 },
            { 0x65, KeyScanCode.F9 },
            { 0x67, KeyScanCode.F11 },
            // F13-F15 sit where a PC keyboard has PrtSc/ScrollLock/Pause.
            { 0x69, KeyScanCode.PrtSc },
            { 0x6b, KeyScanCode.ScrollLock },
            { 0x6d, KeyScanCode.F10 },
            { 0x6f, KeyScanCode.F12 },
            { 0x71, KeyScanCode.Pause },
            // kVK_Help: the Insert position on Apple's full-size keyboards.
            { 0x72, KeyScanCode.Insert },
            { 0x73, KeyScanCode.Home },
            { 0x74, KeyScanCode.PageUp },
            { 0x75, KeyScanCode.Delete },
            { 0x76, KeyScanCode.F4 },
            { 0x77, KeyScanCode.End },
            { 0x78, KeyScanCode.F2 },
            { 0x79, KeyScanCode.PageDown },
            { 0x7a, KeyScanCode.F1 },
            { 0x7b, KeyScanCode.Left },
            { 0x7c, KeyScanCode.Right },
            { 0x7d, KeyScanCode.Down },
            { 0x7e, KeyScanCode.Up },
        };
    }
}
