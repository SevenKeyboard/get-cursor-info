#Requires AutoHotkey v1.1.0+
;==============================================================
; getCursorInfo — Gets extended cursor information (flags, handle, screen position) via GetCursorInfo
;
; GitHub: https://github.com/SevenKeyboard/get-cursor-info
; Author: SevenKeyboard Ltd. (2026)
; License: The Unlicense
;
; Documentation / References:
;   GetCursorInfo has different Results
;     https://www.autohotkey.com/boards/viewtopic.php?t=31095
;   The way to detect the current mouse cursor type from bash or python
;     https://stackoverflow.com/questions/45097307/the-way-to-detect-the-current-mouse-cursor-type-from-bash-or-python
;   Re: Specific detection of non-native "A_Cursor"
;     https://www.autohotkey.com/boards/viewtopic.php?t=106593#p473753
;==============================================================
class VersionManager_getCursorInfo
{
    static _ := VersionManager_getCursorInfo._init()
    _init()    {
        global
        GETCURSORINFO_VERSION := "1.0.0"
    }
}
getCursorInfo()    {
    info:={cbSize:"", flags:"", hCursor:"", ptScreenPos:{x:"", y:""}}
    ,varSetCapacity(CURSORINFO, cbSize:=4+4+A_PtrSize+8)
    ,numput(cbSize, CURSORINFO, 0, "UInt")
    if (dllCall("User32.dll\GetCursorInfo", "Ptr",&CURSORINFO))    {
        info.cbSize:=numGet(CURSORINFO, 0, "UInt") ;  DWORD
        ,info.flags:=numGet(CURSORINFO, 4, "UInt") ;  DWORD
        ,info.hCursor:=numGet(CURSORINFO, 8, "Ptr") ;  HCURSOR
        ,info.ptScreenPos.x:=numGet(CURSORINFO, 8+A_PtrSize, "Int") ;  POINT  LONG
        ,info.ptScreenPos.y:=numGet(CURSORINFO, 8+A_PtrSize+4, "Int") ;  POINT  LONG
    }
    return info
}