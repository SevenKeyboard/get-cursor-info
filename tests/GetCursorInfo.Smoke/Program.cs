using System;
using SevenKeyboard;

internal static class Program
{
    private static unsafe int Main()
    {
        GetCursorInfo.NativeCursorInfo native = default;
        byte* start = (byte*)&native;
        int expectedSize = IntPtr.Size == 8 ? 24 : 20;
        if (sizeof(GetCursorInfo.NativeCursorInfo) != expectedSize
            || (byte*)&native.CbSize - start != 0
            || (byte*)&native.Flags - start != 4
            || (byte*)&native.HCursor - start != 8
            || (byte*)&native.X - start != 8 + IntPtr.Size
            || (byte*)&native.Y - start != 12 + IntPtr.Size)
        {
            Console.Error.WriteLine("CURSORINFO layout mismatch.");
            return 1;
        }

        Console.WriteLine($"CURSORINFO layout OK: {IntPtr.Size * 8}-bit, {expectedSize} bytes.");
        if (!GetCursorInfo.TryGet(out CursorInfo info))
        {
            Console.Error.WriteLine("GetCursorInfo failed.");
            return 1;
        }

        Console.WriteLine($"Flags={info.Flags}, HCursor=0x{info.HCursor:X}, X={info.X}, Y={info.Y}");
        return 0;
    }
}
