/**
*    Copyright(C) G1ANT Ltd, All rights reserved
*    Solution G1ANT.Addon, Project G1ANT.Addon.Ocr
*    www.g1ant.com
*
*    Licensed under the G1ANT license.
*    See License.txt file in the project root for full license information.
*
*/

using System.Diagnostics;
using System.Threading;

namespace G1ANT.Addon.Ocr.Google.Tests
{
    public class OcrGoogleTests
    {
        public const int TestTimeout = 120000;

        public static void StartPaint(string imagePath)
        {
            var processInfo = new ProcessStartInfo("mspaint.exe", $"\"{imagePath}\"");
            processInfo.WindowStyle = ProcessWindowStyle.Maximized;
            var proces = Process.Start(processInfo);
            proces.WaitForInputIdle();
            Thread.Sleep(3000);
        }

        public static void KillAllPaints()
        {
            foreach (var process in Process.GetProcessesByName("mspaint"))
            {
                try
                {
                    process.Kill();
                }
                catch { }
            }
        }
    }
}
