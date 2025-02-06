using System.Runtime.CompilerServices;
using System.IO;
using System;

namespace Unknown6656.Runtime;


public static class LibGDIPlusInstaller
{
    private static string[] LibraryCandidates = [
        "libgdiplus.so",
        "libgdiplus.so.0",
        "libgdiplus.so.0.0.0",
        "libgdi+.so",
        "libgdi+.so.0",
        "libgdi+.so.0.0.0",
        "libgdiplus.dll.so",
        "libgdiplus.dll.so.0",
        "libgdi+.dll.so",
        "libgdi+.dll.so.0",
    ];
    private const string USR_LIB = "/usr/lib";


    public static bool IsGDIInstalled => GDILibraryPath is not null;

    public static FileInfo? GDILibraryPath { get; private set; } = null;


    internal static void ResolveLibGDIPlus()
    {
        static FileInfo? resolve()
        {
            if (OS.IsWindows)
                return new("gdi32.dll");

            FileInfo? fi = null;

            try
            {
                foreach (string path in LibraryCandidates)
                    if ((fi = new(path)).Exists)
                        return fi;
            }
            catch
            {
            }

            try
            {
                foreach (string path in LibraryCandidates)
                    if ((fi = new($"{USR_LIB}/{path}")).Exists)
                        break;
            }
            catch
            {
            }

            if (fi?.Exists ?? false)
            {
                Environment.SetEnvironmentVariable("LD_LIBRARY_PATH", USR_LIB);
                Environment.SetEnvironmentVariable("MONO_PATH", USR_LIB);
                Environment.SetEnvironmentVariable("PATH", $"{Environment.GetEnvironmentVariable("PATH")}:{USR_LIB}");

                return fi;
            }

            // TODO : ?

            return null;
        }

        if (!IsGDIInstalled)
            GDILibraryPath = resolve();
    }

    public static void EnsureGDIPlusInstalled()
    {
        if (!IsGDIInstalled)
            throw new InvalidOperationException("'GDI32.DLL'/'libgdiplus.so' is not installed or could not be found.");
    }

    public static void InstallGDIPlus()
    {
        throw new NotImplementedException();


        ResolveLibGDIPlus(); // refresh
    }
}
