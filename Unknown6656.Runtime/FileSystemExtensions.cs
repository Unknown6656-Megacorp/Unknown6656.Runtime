using System.Linq;
using System.Text;
using System.IO;

using System.Runtime.Versioning;

namespace Unknown6656.Runtime;


public static unsafe class FileSystemExtensions
{
    // TODO : all extensions from 'tabbedexplorer'


    [SupportedOSPlatform(OS.WIN)]
    public static string GetShortPath(string path)
    {
        StringBuilder sb = new(256);
        int length = NativeInterop.GetShortPathName(path, sb, sb.Capacity);

        return length == 0 ? throw new IOException($"Failed to get short path for '{path}'.") : sb.ToString();
    }

    public static FileSystemInfo GetSystemInfo(string path)
    {
        try
        {
            if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
                return new DirectoryInfo(path);
        }
        catch
        {
        }

        return new FileInfo(path);
    }

    public static FileSystemInfo[] ResolveWildCards(string path_with_wildcards)
    {
        string dir = Path.GetDirectoryName(path_with_wildcards) ?? ".";
        string file = Path.GetFileName(path_with_wildcards);
        DirectoryInfo d = new(dir);

        return d.EnumerateFileSystemInfos(file).ToArray();
    }

    [SupportedOSPlatform(OS.WIN)]
    public static bool CreateNTFSHardLink(string link_name, string target) => NativeInterop.CreateHardLink(link_name, target, null);

    //public static Tree<FileSystemInfo> GetDirectoryTree(string directory_path) => GetDirectoryTree(new DirectoryInfo(directory_path));

    //public static Tree<FileSystemInfo> GetDirectoryTree(DirectoryInfo directory) => throw new System.NotImplementedException(); // TODO

    public static DirectoryInfo GetTemporaryDirectory()
    {
        DirectoryInfo dir;

        do
            dir = new(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));
        while (dir.Exists);

        dir.Create();

        return dir;
    }
}
