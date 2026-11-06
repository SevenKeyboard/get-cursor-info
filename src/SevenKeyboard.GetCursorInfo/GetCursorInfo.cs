//==============================================================
// getCursorInfo — Gets extended cursor information (flags, handle, screen position) via GetCursorInfo
//
// GitHub: https://github.com/SevenKeyboard/get-cursor-info
// Author: SevenKeyboard Ltd. (2026)
// License: The Unlicense
//
// Documentation / References:
//   GetCursorInfo has different Results
//     https://www.autohotkey.com/boards/viewtopic.php?t=31095
//   The way to detect the current mouse cursor type from bash or python
//     https://stackoverflow.com/questions/45097307/the-way-to-detect-the-current-mouse-cursor-type-from-bash-or-python
//   Re: Specific detection of non-native "A_Cursor"
//     https://www.autohotkey.com/boards/viewtopic.php?t=106593#p473753
//   GetCursorInfo / CURSORINFO
//     https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getcursorinfo
//     https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-cursorinfo
//==============================================================

using System;
using System.Runtime.InteropServices;

namespace SevenKeyboard
{
    [Flags]
    public enum CursorFlags : uint
    {
        Showing = 1,
        Suppressed = 2
    }

    public readonly struct CursorInfo
    {
        public CursorFlags Flags { get; }

        // Borrowed handle: never pass it to DestroyIcon or DestroyCursor.
        public nint HCursor { get; }
        public int X { get; }
        public int Y { get; }

        internal CursorInfo(CursorFlags flags, nint hCursor, int x, int y)
        {
            Flags = flags;
            HCursor = hCursor;
            X = x;
            Y = y;
        }

    }

    public static partial class GetCursorInfo
    {
        // On failure, info is default; zero flags alone mean hidden, not failure.
        public static unsafe bool TryGet(out CursorInfo info)
        {
            NativeCursorInfo native = new() { CbSize = (uint)sizeof(NativeCursorInfo) };
            info = default;
            if (GetCursorInfoNative(ref native) == 0)
                return false;

            info = new CursorInfo((CursorFlags)native.Flags, native.HCursor, native.X, native.Y);
            return true;
        }

        // DWORD, DWORD, HCURSOR, POINT (two signed 32-bit LONGs).
        // Default native alignment gives 20 bytes on x86 and 24 on x64.
        [StructLayout(LayoutKind.Sequential)]
        internal struct NativeCursorInfo
        {
            internal uint CbSize;
            internal uint Flags;
            internal nint HCursor;
            internal int X;
            internal int Y;
        }

        [LibraryImport("user32.dll", EntryPoint = "GetCursorInfo")]
        private static partial int GetCursorInfoNative(ref NativeCursorInfo info);
    }
}
