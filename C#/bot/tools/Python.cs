using System.Diagnostics;

namespace OmniDownloader.bot.tools
{
    public static class Python
    {
        public static async Task<byte[]?> GetVideoBytesAsync(string exePath, string videoUrl)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = exePath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            startInfo.ArgumentList.Add(videoUrl);

            using var process = Process.Start(startInfo);

            if (process != null)
            {
                using var ms = new MemoryStream();

                await process.StandardOutput.BaseStream.CopyToAsync(ms);
                await process.WaitForExitAsync();
                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    await Logger.LogAction("Process (download_video.exe) exited successfully.", ConsoleColor.Magenta);
                    return ms.ToArray();
                }
                else
                {
                    await Logger.LogAction($"Process (download_video.exe) exited with code {process.ExitCode}.", ConsoleColor.Red);
                    return null;
                }
            }
            else
            {
                await Logger.LogAction("Failed to start process (download_video.exe).", ConsoleColor.Magenta);
                return null;
            }
        }


    }
}
