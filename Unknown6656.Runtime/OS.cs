using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System;

namespace Unknown6656.Runtime;


public enum KnownOS
{
    Windows,
    UnknownUnix,
    Linux,
    FreeBSD,
    Docker,
    WSL,
    Android,
    iOS,
    TvOS,
    WatchOS,
    MacOS,
    MacCatalyst,
    Browser,
}

public static class OS
{
    private const string DOCKER_INDICATOR = "/.dockerenv";
    private const string WSL_INDICATOR = "/proc/sys/fs/binfmt_misc/WSLInterop";

    public const string WIN = "windows";
    public const string LNX = "linux";
    public const string IOS = "iOS";
    public const string MAC = "macos";
    public const string MACC = "MacCatalyst";
    public const string ANDR = "android";
    public const string BROW = "browser";
    public const string TVOS = "tvos";
    public const string WAT = "watchos";


    public static KnownOS CurrentOS
    {
        get
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ||
                Environment.OSVersion.Platform is PlatformID.Win32S
                                               or PlatformID.Win32Windows
                                               or PlatformID.Win32NT
                                               or PlatformID.WinCE
                                               or PlatformID.Xbox)
                return KnownOS.Windows;
            else if (IsInsideWSL)
                return KnownOS.WSL;
            else if (IsInsideDocker)
                return KnownOS.Docker;
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD) || OperatingSystem.IsFreeBSD())
                return KnownOS.FreeBSD;
            else if (OperatingSystem.IsMacCatalyst())
                return KnownOS.MacCatalyst;
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || Environment.OSVersion.Platform is PlatformID.MacOSX || OperatingSystem.IsMacOS())
                return KnownOS.MacOS;
            else if (OperatingSystem.IsAndroid())
                return KnownOS.Android;
            else if (OperatingSystem.IsIOS())
                return KnownOS.iOS;
            else if (OperatingSystem.IsTvOS())
                return KnownOS.TvOS;
            else if (OperatingSystem.IsWatchOS())
                return KnownOS.WatchOS;
            else if (OperatingSystem.IsBrowser() || OperatingSystem.IsWasi())
                return KnownOS.Browser;
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return KnownOS.Linux;
            else if (Environment.OSVersion.Platform is PlatformID.Unix)
                return KnownOS.UnknownUnix; // this could also be macos, although it is unlikely
            else if (Environment.OSVersion.Platform is PlatformID.Other)
                ; // this could be wasm, although it is unlikely

            return KnownOS.UnknownUnix; // probably some kind of unix-like system
        }
    }

    public static bool IsWindows => CurrentOS is KnownOS.Windows;

    public static bool IsLinux => CurrentOS is KnownOS.Linux or KnownOS.WSL or KnownOS.Docker;

    public static bool IsFreeBSD => RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD);

    public static bool IsUnix => IsOneOf(
        KnownOS.Linux,
        KnownOS.UnknownUnix,
        KnownOS.FreeBSD,
        KnownOS.WSL,
        KnownOS.Docker,
        KnownOS.MacOS,
        KnownOS.MacCatalyst
    );

    public static bool IsOSX => CurrentOS is KnownOS.MacOS or KnownOS.MacCatalyst;

    public static bool IsAndroid => CurrentOS is KnownOS.Android;

    public static bool IsApple => IsOneOf(
        KnownOS.MacOS,
        KnownOS.MacCatalyst,
        KnownOS.iOS,
        KnownOS.TvOS,
        KnownOS.WatchOS
    );

    public static bool IsBrowser => CurrentOS is KnownOS.Browser;

    public static bool IsMobile => IsOneOf(
        KnownOS.Android,
        KnownOS.iOS,
        KnownOS.TvOS,
        KnownOS.WatchOS
    );

    public static bool IsPOSIXCompatible => IsOneOf(
        KnownOS.Linux,
        KnownOS.UnknownUnix,
        KnownOS.FreeBSD,
        KnownOS.WSL,
        KnownOS.Docker,
        KnownOS.MacOS,
        KnownOS.MacCatalyst,
        KnownOS.Android,
        KnownOS.iOS,
        KnownOS.TvOS,
        KnownOS.WatchOS
    );

    public static bool IsInsideDocker => File.Exists(DOCKER_INDICATOR); // TODO : more checks?

    // TODO : check if is running inside a container (container, snap, etc.)

    public static bool IsInsideWSL
    {
        get
        {
            try
            {
                if (File.Exists(WSL_INDICATOR))
                    return true;
            }
            catch
            {
            }

            // TODO : check if the current execution context is WSL
            // TODO : check "uname -a" for "microsoft" substring.

            return false;
        }
    }

    // TODO : check if is running inside a VM (vmware, virtualbox, qemu, etc.)
    //public static bool IsInsideVirtualMachine => throw new NotImplementedException();


    public static bool IsOneOf(params KnownOS[] os) => IsOneOf(os as IEnumerable<KnownOS>);

    public static bool IsOneOf(IEnumerable<KnownOS> os) => os.Contains(CurrentOS);

    /// <summary>
    /// Executes the given bash command
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [SupportedOSPlatform(LNX)]
    [SupportedOSPlatform(MAC)]
    [SupportedOSPlatform(MACC)]
    public static string? ExecuteBashCommand(string command)
    {
        using Process? process = Process.Start(new ProcessStartInfo
        {
            FileName = "/bin/bash",
            Arguments = $"-c \"{command.Replace("\"", "\\\"")}\"",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = false,
        });
        string? result = process?.StandardOutput.ReadToEnd();

        process?.WaitForExit();

        return result;
    }

    [SupportedOSPlatform(WIN)]
    [SupportedOSPlatform(LNX)]
    [SupportedOSPlatform(MAC)]
    [SupportedOSPlatform(MACC)]
    public static unsafe void CreateBluescreenOfDeath()
    {
#pragma warning disable CA1416 // Validate platform compatibility
        if (IsWindows)
        {
            NativeInterop.RtlAdjustPrivilege(19, true, false, out _);
            NativeInterop.NtRaiseHardError(0xc0000420u, 0, 0, null, 6, out _);
        }
        else
        {
            ExecuteBashCommand("echo 1 > /proc/sys/kernel/sysrq");
            ExecuteBashCommand("echo c > /proc/sysrq-trigger");
        }
#pragma warning restore CA1416
    }

    // TODO : throw on unsupported OS functionality
}
