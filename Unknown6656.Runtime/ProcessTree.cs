using System.Diagnostics;
using System.Management;
using System.Linq;
using System.IO;
using System;

namespace Unknown6656.Runtime;


public static class ProcessTree
{
    public static Process? GetParentProcess() => GetParentProcess(Process.GetCurrentProcess());

    public static Process? GetParentProcess(Process child)
    {
        int pid = -1;

        try
        {
            if (OS.IsWindows)
            {
#pragma warning disable CA1416 // Validate platform compatibility
                using ManagementObjectSearcher searcher = new($"SELECT ParentProcessId FROM Win32_Process WHERE ProcessId = {child.Id}");

                pid = Convert.ToInt32(searcher.Get().Cast<ManagementObject>().FirstOrDefault()?["ParentProcessId"] ?? -1);
#pragma warning restore CA1416
            }
            else
            {
                string stat = File.ReadAllText($"/proc/{pid}/stat");
                string[] parts = stat.Split(' ');

                pid = int.Parse(parts[3]);
            }
        }
        catch
        {
        }

        return pid > 0 ? Process.GetProcessById(pid) : null;
    }
}
