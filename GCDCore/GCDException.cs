using System;
using System.Diagnostics;

namespace GCDCore
{
    public struct GCDException
    {
        public static void HandleException(System.Exception ex, string UIMessage = "")
        {
            string appName = System.IO.Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.ModuleName);

            if (appName.ToLower().Contains("arcmap"))
                ex.Data[appName] = Process.GetCurrentProcess().MainModule.FileVersionInfo.FileVersion;

            ex.Data["GCD"] = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Trim();
            naru.error.ExceptionUI.HandleException(ex, UIMessage, Properties.Resources.NewIssueURL);
        }
    }
}
