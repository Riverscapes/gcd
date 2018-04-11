using System;

namespace GCDCore.UserInterface
{
    public class OnlineHelp
    {
        public static void Show(string helpKey)
        {
            string helpURL = GCDCore.Properties.Resources.GCDWebSiteURL;
            try
            {
                helpURL = string.Format("{0}?APPKEY={1}", helpURL, helpKey);
                System.Diagnostics.Process.Start(helpURL);
            }
            catch(Exception ex)
            {
                ex.Data["Help Key"] = helpKey;
                ex.Data["Help URL"] = helpURL;
                GCDException.HandleException(ex, "Error launching online help.");
            }
        }
    }
}
