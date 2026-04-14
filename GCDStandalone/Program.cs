using GCDCore;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Forms;

namespace GCDStandalone
{
    static class Program
    {
        // Keep a single instance for the lifetime of the app
        public static TelemetryClient Telemetry { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Telemetry = LoadTelemetry();
            GCDCore.Project.ProjectManager.Telemetry = Telemetry;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }

        private static TelemetryClient LoadTelemetry()
        {
            try
            {
                string folder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string path = Path.Combine(folder, "telemetry.json");

                if (!File.Exists(path))
                    return null;

                TelemetryConfig config = JsonConvert.DeserializeObject<TelemetryConfig>(File.ReadAllText(path));

                if (string.IsNullOrWhiteSpace(config?.Endpoint) || string.IsNullOrWhiteSpace(config?.Token))
                    return null;

                return new TelemetryClient(
                    endpoint: config.Endpoint,
                    token: config.Token,
                    appName: GCDCore.Properties.Resources.ApplicationNameShort,
                    appVersion: System.Reflection.Assembly.GetAssembly(typeof(GCDCore.Project.ProjectManager)).GetName().Version.ToString()
                );
            }
            catch
            {
                return null;
            }
        }

        private class TelemetryConfig
        {
            [JsonProperty("api-url")]
            public string Endpoint { get; set; }

            [JsonProperty("api-token")]
            public string Token { get; set; }
        }
    }
}
