using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

namespace Unknown6656.Runtime;


public static unsafe class NativeInterop
{
    public const string GDI32 = "gdi32.dll";
    public const string USER32 = "user32.dll";
    public const string KERNEL32 = "kernel32.dll";
    public const string NTDLL = "ntdll.dll";
    public const string LIBC = "libc.so";


    [DllImport(USER32)]
    [SupportedOSPlatform(OS.WIN)]
    public extern static int GetDC(void* hwnd);

    [DllImport(USER32)]
    [SupportedOSPlatform(OS.WIN)]
    public static extern bool GetClientRect(void* hWnd, out RECT lpRect);

    [DllImport(USER32)]
    [SupportedOSPlatform(OS.WIN)]
    public static extern bool GetWindowRect(void* hWnd, out RECT lpRect);


    [DllImport(GDI32, CharSet = CharSet.Auto, EntryPoint = "GetCurrentObject", ExactSpelling = true, SetLastError = true)]
    [SupportedOSPlatform(OS.WIN)]
    public static extern nint IntGetCurrentObject(HandleRef hDC, nint uObjectType);


    [DllImport(KERNEL32, CharSet = CharSet.Unicode)]
    [SupportedOSPlatform(OS.WIN)]
    public static extern int GetShortPathName([MarshalAs(UnmanagedType.LPWStr)] string? path, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder @short, int length);

    [DllImport(KERNEL32, CharSet = CharSet.Unicode)]
    [SupportedOSPlatform(OS.WIN)]
    public static extern bool CreateHardLink([MarshalAs(UnmanagedType.LPWStr)] string lpFileName, [MarshalAs(UnmanagedType.LPWStr)] string lpExistingFileName, void* reserved);

    [DllImport(KERNEL32, SetLastError = true)]
    [SupportedOSPlatform(OS.WIN)]
    public static extern void* GetStdHandle(int nStdHandle);

    [DllImport(KERNEL32)]
    [SupportedOSPlatform(OS.WIN)]
    public extern static void* GetConsoleWindow();

    [DllImport(KERNEL32)]
    [SupportedOSPlatform(OS.WIN)]
    public static extern bool SetConsoleMode(void* hWnd, ConsoleMode dwMode);

    [DllImport(KERNEL32)]
    [SupportedOSPlatform(OS.WIN)]
    public static extern bool GetConsoleMode(void* hWnd, ConsoleMode* lpMode);

    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport(KERNEL32, SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern bool SetCurrentConsoleFontEx(void* hWnd, bool MaximumWindow, ref ConsoleFontInfo ConsoleCurrentFontEx);

    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport(KERNEL32, SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern bool GetCurrentConsoleFontEx(void* hWnd, bool MaximumWindow, ref ConsoleFontInfo ConsoleCurrentFontEx);

    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport(KERNEL32, SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern bool ReadConsoleInput(nint hConsoleInput, [Out] INPUT_RECORD[] lpBuffer, int nLength, out int lpNumberOfEventsRead);

    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport(KERNEL32, SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern bool PeekConsoleInput(nint hConsoleInput, [Out] INPUT_RECORD[] lpBuffer, int nLength, out int lpNumberOfEventsRead);

    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport(KERNEL32, SetLastError = true)]
    public static extern bool GetNumberOfConsoleInputEvents(nint hConsoleInput, out int lpcNumberOfEvents);

    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport(KERNEL32, SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern bool WriteConsoleInput(nint hConsoleInput, INPUT_RECORD[] lpBuffer, int nLength, out int lpNumberOfEventsWritten);

    [DllImport(KERNEL32)]
    [SupportedOSPlatform(OS.WIN)]
    public static extern void* VirtualAlloc(void* addr, int size, int type, int protect);

    [DllImport(KERNEL32)]
    [SupportedOSPlatform(OS.WIN)]
    public static extern bool VirtualProtect(void* addr, int size, int new_protect, int* old_protect);

    [DllImport(KERNEL32)]
    [SupportedOSPlatform(OS.WIN)]
    public static extern bool VirtualFree(void* addr, int size, int type);


    [DllImport(NTDLL)]
    [SupportedOSPlatform(OS.WIN)]
    public static extern int RtlAdjustPrivilege(int Privilege, bool bEnablePrivilege, bool IsThreadPrivilege, out bool _);

    [DllImport(NTDLL)]
    [SupportedOSPlatform(OS.WIN)]
    public static extern int NtRaiseHardError(uint ErrorStatus, uint NumberOfParameters, uint UnicodeStringParameterMask, void* Parameters, uint ValidResponseOption, out uint _);


    [DllImport(LIBC)]
    [SupportedOSPlatform(OS.LIN)]
    [SupportedOSPlatform(OS.MAC)]
    public static extern void mprotect(void* buffer, int size, int mode);

    [DllImport(LIBC)]
    [SupportedOSPlatform(OS.LIN)]
    [SupportedOSPlatform(OS.MAC)]
    public static extern void posix_memalign(void** buffer, int alignment, int size);

    [DllImport(LIBC)]
    [SupportedOSPlatform(OS.LIN)]
    [SupportedOSPlatform(OS.MAC)]
    public static extern void free(void* buffer);
}

[Flags]
[SupportedOSPlatform(OS.WIN)]
public enum ConsoleMode
    : uint
{
    ENABLE_PROCESSED_INPUT = 0x0001,
    ENABLE_LINE_INPUT = 0x0002,
    ENABLE_ECHO_INPUT = 0x0004,
    ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004,
    ENABLE_WINDOW_INPUT = 0x0008,
    ENABLE_MOUSE_INPUT = 0x0010,
    ENABLE_INSERT_MODE = 0x0020,
    ENABLE_QUICK_EDIT_MODE = 0x0040,
    ENABLE_EXTENDED_FLAGS = 0x0080,
    ENABLE_VIRTUAL_TERMINAL_INPUT = 0x0200,
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public struct ConsoleFontInfo
{
    public const int FIXED_WIDTH_TRUETYPE = 0x0036;

    public int cbSize;
    public int FontIndex;
    public short FontWidth;
    public (short W, short H) FontSize;
    public int FontFamily;
    public int FontWeight;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    //[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.wc, SizeConst = 32)]
    public string FontName;
}

[StructLayout(LayoutKind.Explicit)]
public record struct INPUT_RECORD
{
    [FieldOffset(0)]
    public EventType EventType;
    [FieldOffset(4)]
    public KeyEvent KeyEvent;
    [FieldOffset(4)]
    public MouseEvent MouseEvent;
    [FieldOffset(4)]
    public short WindowBufferSizeEventX;
    [FieldOffset(6)]
    public short WindowBufferSizeEventY;
    // [FieldOffset(4)]
    // public MENU_EVENT_RECORD MenuEvent;
    [FieldOffset(4)]
    public int FocusEvent;
}

public record struct MouseEvent
{
    public short wMousePositionX;
    public short wMousePositionY;
    public MouseButtons dwButtonState;
    public ModifierKeysState dwControlKeyState;
    public MouseActions dwEventFlags;
}

[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
public record struct KeyEvent
{
    [FieldOffset(0)]
    public bool bKeyDown;
    [FieldOffset(4)]
    public ushort wRepeatCount;
    [FieldOffset(6)]
    public ushort wVirtualKeyCode;
    [FieldOffset(8)]
    public ushort wVirtualScanCode;
    [FieldOffset(10)]
    public char UnicodeChar;
    [FieldOffset(10)]
    public byte AsciiChar;
    [FieldOffset(12)]
    public ModifierKeysState dwControlKeyState;
}

public enum EventType
    : ushort
{
    KeyEvent = 1,
    MouseEvent = 2,
    BufferSizeEvent = 4,
    MenuEvent = 8,
    FocusEvent = 16,
}

[Flags]
public enum MouseButtons
    : uint
{
    LeftMost = 0x0001,
    RightMost = 0x0002,
    Button2 = 0x0004,
    Button3 = 0x0008,
    Button4 = 0x0010,
}

[Flags]
public enum MouseActions
    : uint
{
    Movement = 0x0001,
    DoubleClick = 0x0002,
    Wheel = 0x0004,
    HorizontalWheel = 0x0008,
}

[Flags]
public enum ModifierKeysState
    : uint
{
    RightAlt = 0x0001,
    LeftAlt = 0x0002,
    RightCrtl = 0x0004,
    LeftCrtl = 0x0008,
    Shift = 0x0010,
    NumLock = 0x0020,
    ScrollLock = 0x0040,
    CapsLock = 0x0080,
    Enhanced = 0x0100,
}
