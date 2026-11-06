# get-cursor-info
Gets Windows cursor flags, borrowed handle, and screen position using C# (.NET 10).

```csharp
using SevenKeyboard;
if (GetCursorInfo.TryGet(out CursorInfo info))
    Console.WriteLine($"{info.Flags}: {info.HCursor:X} at ({info.X}, {info.Y})");
```

On failure, TryGet returns false and info is default. Zero flags mean hidden.
HCursor is borrowed; never pass it to DestroyIcon or DestroyCursor.
