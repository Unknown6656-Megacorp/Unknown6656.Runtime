using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;
using System;

using Unknown6656.Generics;
using Unknown6656.Common;

namespace Unknown6656.Runtime.Console;

using Console = System.Console;


public enum TextIntensityMode
    : byte
{
    Regular = 0,
    Bold = 1,
    Dim = 2,
}

public enum TextBlinkMode
    : byte
{
    NotBlinking = 0,
    Slow = 1,
    Rapid = 2,
}

public enum LineRenderingMode
{
    Regular,
    DoubleWidth,
    DoubleHeight,
    DoubleHeight_Top,
    DoubleHeight_Bottom,
}

public enum ConsoleCursorShape
{
    Default = 0,
    BlinkingBlock = 1,
    SolidBlock = 2,
    BlinkingUnderline = 3,
    SolidUnderline = 4,
    BlinkingBar = 5,
    SolidBar = 6,
}

public static unsafe partial class ConsoleExtensions
{
    public static bool ThrowOnInvalidConsoleMode { get; set; } = false;

    [SupportedOSPlatform(OS.WIN)]
    public static void* STDINHandle => OS.IsWindows ? NativeInterop.GetStdHandle(-10)
                                                    : throw new InvalidOperationException("This operation is not supported on non-Windows operating systems.");

    [SupportedOSPlatform(OS.WIN)]
    public static void* STDOUTHandle => OS.IsWindows ? NativeInterop.GetStdHandle(-11)
                                                     : throw new InvalidOperationException("This operation is not supported on non-Windows operating systems.");

    [SupportedOSPlatform(OS.WIN)]
    public static void* STDERRHandle => OS.IsWindows ? NativeInterop.GetStdHandle(-12)
                                                     : throw new InvalidOperationException("This operation is not supported on non-Windows operating systems.");

    [SupportedOSPlatform(OS.WIN)]
    public static ConsoleMode STDINConsoleMode
    {
        set
        {
            if (!OS.IsWindows)
                throw new InvalidOperationException("Writing the STDIN console mode is not supported on non-Windows operating systems.");
            else if (!NativeInterop.SetConsoleMode(STDINHandle, value))
                if (ThrowOnInvalidConsoleMode)
                    throw NETRuntimeInterop.GetLastWin32Error();
        }
        get
        {
            if (!OS.IsWindows)
                throw new InvalidOperationException("Reading the STDIN console mode is not supported on non-Windows operating systems.");

            ConsoleMode mode = default;

            if (NativeInterop.GetConsoleMode(STDINHandle, &mode))
                return mode;
            else if (ThrowOnInvalidConsoleMode)
                throw NETRuntimeInterop.GetLastWin32Error();
            else
                return default;
        }
    }

    [SupportedOSPlatform(OS.WIN)]
    public static ConsoleMode STDOUTConsoleMode
    {
        set
        {
            if (!OS.IsWindows)
                throw new InvalidOperationException("Writing the STDOUT console mode is not supported on non-Windows operating systems.");
            else if (!NativeInterop.SetConsoleMode(STDOUTHandle, value))
                if (ThrowOnInvalidConsoleMode)
                    throw NETRuntimeInterop.GetLastWin32Error();
        }
        get
        {
            if (!OS.IsWindows)
                throw new InvalidOperationException("Reading the STDOUT console mode is not supported on non-Windows operating systems.");

            ConsoleMode mode = default;

            if (NativeInterop.GetConsoleMode(STDOUTHandle, &mode))
                return mode;
            else if (ThrowOnInvalidConsoleMode)
                throw NETRuntimeInterop.GetLastWin32Error();
            else
                return default;
        }
    }

    [SupportedOSPlatform(OS.WIN)]
    public static ConsoleMode STDERRConsoleMode
    {
        set
        {
            if (!OS.IsWindows)
                throw new InvalidOperationException("Writing the STDERR console mode is not supported on non-Windows operating systems.");
            else if (!NativeInterop.SetConsoleMode(STDERRHandle, value))
                if (ThrowOnInvalidConsoleMode)
                    throw NETRuntimeInterop.GetLastWin32Error();
        }
        get
        {
            if (!OS.IsWindows)
                throw new InvalidOperationException("Reading the STDERR console mode is not supported on non-Windows operating systems.");

            ConsoleMode mode = default;

            if (NativeInterop.GetConsoleMode(STDERRHandle, &mode))
                return mode;
            else if (ThrowOnInvalidConsoleMode)
                throw NETRuntimeInterop.GetLastWin32Error();
            else
                return default;
        }
    }

    [SupportedOSPlatform(OS.WIN)]
    public static ConsoleFontInfo FontInfo
    {
        set
        {
            if (!OS.IsWindows)
                throw new InvalidOperationException("Changing the console font is not supported on non-Windows operating systems.");
            else if (!NativeInterop.SetCurrentConsoleFontEx(STDOUTHandle, false, ref value))
                throw NETRuntimeInterop.GetLastWin32Error();
        }
        get
        {
            if (!OS.IsWindows)
                throw new InvalidOperationException("Reading the console font is not supported on non-Windows operating systems.");

            ConsoleFontInfo font = new()
            {
                cbSize = Marshal.SizeOf<ConsoleFontInfo>()
            };

            return NativeInterop.GetCurrentConsoleFontEx(STDOUTHandle, false, ref font) ? font : throw NETRuntimeInterop.GetLastWin32Error();
        }
    }

    [SupportedOSPlatform(OS.WIN)]
    public static Font Font
    {
        set => SetCurrentFont(value);
        get
        {
            ConsoleFontInfo font = FontInfo;

            return new Font(font.FontName, font.FontSize.H, font.FontWeight > 550 ? FontStyle.Bold : FontStyle.Regular);
        }
    }

    public static ConsoleCursorShape CursorShape
    {
        get => throw new NotImplementedException();
        set => Console.Write($"\e[{(int)value} q");
    }

    public static bool CursorVisible
    {
        [SupportedOSPlatform(OS.WIN)]
        get => Console.CursorVisible;
        set
        {
            if (OS.IsWindows)
                Console.CursorVisible = value;
            else
                Console.Write(value ? "\e[?25h" : "\e[?25l");
        }
    }

    public static bool AlternateScreenEnabled
    {
        get => throw new NotImplementedException();
        set => Console.Write(value ? "\e[?1049h" : "\e[?1049l");
    }

    public static bool BracketedPasteModeEnabled
    {
        get => throw new NotImplementedException();
        set => Console.Write(value ? "\e[?2004h" : "\e[?2004l");
    }

    public static bool MouseEnabled
    {
        get => throw new NotImplementedException();
#warning TODO : verify if this is correct
        set => Console.Write(value ? "\e[?1000h" : "\e[?1000l");
    }

    public static bool FocusReportingEnabled
    {
        get => throw new NotImplementedException();
        set => Console.Write(value ? "\e[?1004h" : "\e[?1004l");
    }

    //    public static bool MouseHighlightingEnabled
    //    {
    //        get => throw new NotImplementedException();
    //        set => Console.Write(value ? "\e[?1006h" : "\e[?1006l");
    //    }

    public static TextIntensityMode TextIntensity
    {
        get => throw new NotImplementedException();
        set => Console.Write(value switch {
            TextIntensityMode.Bold => "\e[1m",
            TextIntensityMode.Dim => "\e[2m",
            _ => "\e[22m"
        });
    }

    public static TextBlinkMode TextBlink
    {
        get => throw new NotImplementedException();
        set => Console.Write(value switch
        {
            TextBlinkMode.Slow => "\e[5m",
            TextBlinkMode.Rapid => "\e[6m",
            _ => "\e[25m"
        });
    }

    public static bool InvertedColors
    {
        get => throw new NotImplementedException();
        set => Console.Write(value ? "\e[7m" : "\e[27m");
    }

    public static bool UnderlinedText
    {
        get => throw new NotImplementedException();
        set => Console.Write(value ? "\e[4m" : "\e[24m");
    }

    public static bool StrikethroughText
    {
        get => throw new NotImplementedException();
        set => Console.Write(value ? "\e[9m" : "\e[29m");
    }

    public static bool OverlinedText
    {
        get => throw new NotImplementedException();
        set => Console.Write(value ? "\e[53m" : "\e[55m");
    }

    public static bool FrameText
    {
        get => throw new NotImplementedException();
        set => Console.Write(value ? "\e[51m" : "\e[54m");
    }

    public static bool HiddenText
    {
        get => throw new NotImplementedException();
        set => Console.Write(value ? "\e[8m" : "\e[28m");
    }

    // TODO : ^[[5n     ???
    // TODO : ^[[6n     cursor position report
    // TODO : -> ^[[c
    //        <- ^[[?61;6;7;21;22;23;24;28;32;42c



    public static bool SupportsVT100EscapeSequences => !OS.IsWindows || Environment.OSVersion.Version is { Major: >= 10, Build: >= 16257 };
#pragma warning disable CA1416 // Validate platform compatibility
    public static bool AreSTDInVT100EscapeSequencesEnabled => !OS.IsWindows || STDINConsoleMode.HasFlag(ConsoleMode.ENABLE_VIRTUAL_TERMINAL_PROCESSING);

    public static bool AreSTDOutVT100EscapeSequencesEnabled => !OS.IsWindows || STDOUTConsoleMode.HasFlag(ConsoleMode.ENABLE_VIRTUAL_TERMINAL_PROCESSING);

    public static bool AreSTDErrVT100EscapeSequencesEnabled => !OS.IsWindows || STDERRConsoleMode.HasFlag(ConsoleMode.ENABLE_VIRTUAL_TERMINAL_PROCESSING);
#pragma warning restore CA1416


    static ConsoleExtensions()
    {
        if (OS.IsWindows)
        {
            //LINQ.TryDo(() => STDINConsoleMode |= ConsoleMode.ENABLE_VIRTUAL_TERMINAL_INPUT);
            LINQ.TryDo(() => STDINConsoleMode |= ConsoleMode.ENABLE_VIRTUAL_TERMINAL_PROCESSING);
            LINQ.TryDo(() => STDOUTConsoleMode |= ConsoleMode.ENABLE_VIRTUAL_TERMINAL_PROCESSING);
            LINQ.TryDo(() => STDERRConsoleMode |= ConsoleMode.ENABLE_VIRTUAL_TERMINAL_PROCESSING);
        }
    }

    public static void ClearAndResetAll() => Console.Write("\e[3J\ec");

    public static void ResetAllAttributes() => Console.Write("\e[0m");

    public static void ResetForegroundColor() => Console.Write("\e[39m");

    public static void ResetBackgroundColor() => Console.Write("\e[49m");

    public static void ScrollUp(int lines)
    {
        if (lines < 0)
            ScrollDown(-lines);
        else if (lines > 0)
            Console.Write($"\e[{lines}S");
    }

    public static void ScrollDown(int lines)
    {
        if (lines < 0)
            ScrollUp(-lines);
        else if (lines > 0)
            Console.Write($"\e[{lines}T");
    }

    public static void Write(object? value, int left, int top) => Write(value, (left, top));

    public static void Write(object? value, (int left, int top) starting_pos)
    {
        Console.SetCursorPosition(starting_pos.left, starting_pos.top);
        Console.Write(value);
    }

    public static void ChangeLineRendering(int line, LineRenderingMode mode)
    {
        if (mode is LineRenderingMode.DoubleHeight)
        {
            ChangeLineRendering(line, LineRenderingMode.DoubleHeight_Top);
            ChangeLineRendering(line + 1, LineRenderingMode.DoubleHeight_Bottom);
        }
        else
        {
            Console.CursorTop = line;
            Console.Write($"\e#{mode switch
            {
                LineRenderingMode.DoubleWidth => 6,
                LineRenderingMode.DoubleHeight_Top => 3,
                LineRenderingMode.DoubleHeight_Bottom => 4,
                _ => 5
            }}");
        }
    }

    public static void WriteDoubleWidthLine(object? value) => WriteDoubleWidthLine(value, (Console.CursorLeft, Console.CursorTop));

    public static void WriteDoubleWidthLine(object? value, int left, int top) => WriteDoubleWidthLine(value, (left, top));

    public static void WriteDoubleWidthLine(object? value, (int left, int top)? starting_pos)
    {
        if (starting_pos is (int x, int y))
            Console.SetCursorPosition(x, y);

        Console.WriteLine($"\e#5{value}");
    }

    public static void WriteDoubleSizeLine(object? value) => WriteDoubleSizeLine(value, (Console.CursorLeft, Console.CursorTop));

    public static void WriteDoubleSizeLine(object? value, int left, int top) => WriteDoubleSizeLine(value, (left, top));

    public static void WriteDoubleSizeLine(object? value, (int left, int top)? starting_pos)
    {
        int x = starting_pos?.left ?? Console.CursorLeft;
        int y = starting_pos?.top ?? Console.CursorTop;
        string text = value?.ToString() ?? "";

        Console.SetCursorPosition(x, y);
        Console.Write($"\e#3{text}");
        Console.SetCursorPosition(x, y + 1);
        Console.WriteLine($"\e#4{text}");
    }

    public static void FullClear()
    {
        Console.Clear();
        Console.Write("\e[3J");
    }


#warning TODO : ignore escape sequences for length calculation in all the following functions

    public static (int max_line_length, int line_count) WriteBlock(string value, int left, int top) =>
        WriteBlock(value, (left, top));

    public static (int max_line_length, int line_count) WriteBlock(string value, (int left, int top) starting_pos) =>
        WriteBlock(value.SplitIntoLines(), starting_pos);

    public static (int max_line_length, int line_count) WriteBlock(IEnumerable<string> lines, int left, int top) => WriteBlock(lines, (left, top));

    public static (int max_line_length, int line_count) WriteBlock(IEnumerable<string> lines, (int left, int top) starting_pos) =>
        WriteBlock(lines, starting_pos, (0x0fffffff, 0x0fffffff), true);

    public static (int max_line_length, int line_count) WriteBlock(string value, int left, int top, int max_width, int max_height, bool wrap_overflow = true) =>
        WriteBlock(value, (left, top), (max_width, max_height), wrap_overflow);

    public static (int max_line_length, int line_count) WriteBlock(string value, (int left, int top) starting_pos, (int width, int height) max_size, bool wrap_overflow = true) =>
        WriteBlock(value.SplitIntoLines(), starting_pos, max_size, wrap_overflow);

    public static (int max_line_length, int line_count) WriteBlock(IEnumerable<string> lines, int left, int top, int max_width, int max_height, bool wrap_overflow = true) =>
        WriteBlock(lines, (left, top), (max_width, max_height), wrap_overflow);

    public static (int max_line_length, int line_count) WriteBlock(IEnumerable<string> lines, (int left, int top) starting_pos, (int width, int height) max_size, bool wrap_overflow = true)
    {
        List<string> cropped_lines = [];

        foreach (string line in lines)
        {
            string[] sub_lines = line.PartitionByArraySize(max_size.width).ToArray(c => new string(c));

            if (!wrap_overflow && sub_lines.Length > 0)
                cropped_lines.Add(sub_lines[0]);
            else
                cropped_lines.AddRange(sub_lines);
        }

        int line_no = 0;

        while (cropped_lines.Count > max_size.height)
            cropped_lines.RemoveAt(cropped_lines.Count - 1);

        foreach (string line in cropped_lines.Take(max_size.height))
        {
            Console.SetCursorPosition(starting_pos.left, starting_pos.top + line_no);
            Console.Write(line);

            ++line_no;
        }

        return (cropped_lines.Max(line => line.Length), cropped_lines.Count);
    }

    public static void WriteVertical(object? value) => WriteVertical(value, Console.CursorLeft, Console.CursorTop);

    public static void WriteVertical(object? value, int left, int top) => WriteVertical(value, (left, top));

    public static void WriteVertical(object? value, (int left, int top) starting_pos)
    {
        string s = value?.ToString() ?? "";

        for (int i = 0; i < s.Length; i++)
        {
            Console.CursorTop = starting_pos.top + i;
            Console.CursorLeft = starting_pos.left;
            Console.Write(s[i]);
        }
    }

    public static void WriteUnderlined(object? value) => Console.Write($"\e[4m{value}\e[24m");

    public static void WriteInverted(object? value) => Console.Write($"\e[7m{value}\e[27m");



    [SupportedOSPlatform(OS.WIN)]
    public static (ConsoleFontInfo before, ConsoleFontInfo after) SetCurrentFont(Font font)
    {
        ConsoleFontInfo before = FontInfo;
        ConsoleFontInfo set = new()
        {
            cbSize = Marshal.SizeOf<ConsoleFontInfo>(),
            FontIndex = 0,
            FontFamily = ConsoleFontInfo.FIXED_WIDTH_TRUETYPE,
            FontName = font.Name,
            FontWeight = font.Bold ? 700 : 400,
            FontSize = font.Size > 0 ? (default, (short)font.Size) : before.FontSize,
        };

        FontInfo = set;

        return (before, FontInfo);
    }

    // hexdump now moved to Unknown6656.Serialization

    public static ConsoleState SaveConsoleState()
    {
        bool? cursor_visible = null;
        ConsoleMode stdinmode = default;

        if (OS.IsWindows)
        {
#pragma warning disable CA1416 // Validate platform compatibility
            cursor_visible = Console.CursorVisible;
            stdinmode = STDINConsoleMode;
#pragma warning restore CA1416
        }

        return new()
        {
            Background = Console.BackgroundColor,
            Foreground = Console.ForegroundColor,
            InputEncoding = Console.InputEncoding,
            OutputEncoding = Console.OutputEncoding,
            CursorVisible = cursor_visible,
            CursorSize = OS.IsWindows ? Console.CursorSize : 100,
            Mode = stdinmode,
        };
    }

    public static void RestoreConsoleState(ConsoleState? state)
    { 
        if (state is { })
        {
            Console.BackgroundColor = state.Background;
            Console.ForegroundColor = state.Foreground;
            Console.InputEncoding = state.InputEncoding ?? Encoding.Default;
            Console.OutputEncoding = state.OutputEncoding ?? Encoding.Default;

            if (state.CursorVisible is bool vis)
                CursorVisible = vis;

            if (OS.IsWindows)
            {
#pragma warning disable CA1416 // Validate platform compatibility
                STDINConsoleMode = state.Mode;

                if (state.CursorSize is int sz)
                    LINQ.TryDo(() => Console.CursorSize = sz);
#pragma warning restore CA1416
            }
        }
    }

    public static string StripVT100EscapeSequences(this string raw_string) => GenerateVT100Regex().Replace(raw_string, "");

    public static MatchCollection MatchVT100EscapeSequences(this string raw_string) => GenerateVT100Regex().Matches(raw_string);

    public static int CountVT100EscapeSequences(this string raw_string) => GenerateVT100Regex().Count(raw_string);

    public static bool ContainsVT100EscapeSequences(this string raw_string) => GenerateVT100Regex().IsMatch(raw_string);

    public static int LengthWithoutVT100EscapeSequences(this string raw_string) => raw_string.Length - MatchVT100EscapeSequences(raw_string).Sum(m => m.Length);


    [GeneratedRegex(@"(\x1b\[|\x9b)([0-\?]*[\x20-\/]*[@-~]|[^@-_]*[@-_]|[\da-z]{1,2};\d{1,2}H)|\x1b([@-_0-\?\x60-~]|[\x20-\/]|[\x20-\/]{2,}[@-~])", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex GenerateVT100Regex();
}

public sealed class ConsoleState
{
    public ConsoleMode Mode { get; set; }
    public ConsoleColor Background { set; get; }
    public ConsoleColor Foreground { set; get; }
    public Encoding? OutputEncoding { set; get; }
    public Encoding? InputEncoding { set; get; }
    public bool? CursorVisible { set; get; }
    public int? CursorSize { set; get; }
}
