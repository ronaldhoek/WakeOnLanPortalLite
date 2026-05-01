using System;
using System.Diagnostics;
using System.IO;

public static class RDPSigning
{
    //public GetS
    public static void SignRDPFile(string rdpFilePath, string certThumbprint)
    {
        // Het pad naar de Windows rdpsign utility
        string rdpsignPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "rdpsign.exe");

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = rdpsignPath,
            // Gebruik /sha256 voor moderne beveiliging
            Arguments = $"/sha256 {certThumbprint} \"{rdpFilePath}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (Process process = Process.Start(startInfo))
        {
            process.WaitForExit();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            if (process.ExitCode != 0)
            {
                throw new Exception($"RDP Signing failed: {error}");
            }
        }
    }
}