using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace GCDCore
{
    /// <summary>
    /// Lightweight telemetry client that sends anonymous usage pings
    /// to the Riverscapes Telemetry API.
    /// </summary>
    public sealed class TelemetryClient : IDisposable
    {
        private readonly HttpClient _http;
        private readonly string _endpoint;
        private readonly string _token;
        private readonly string _appName;
        private readonly string _appVersion;
        private readonly string _osPlatform;
        private readonly string _clientId;

        /// <summary>
        /// Set to false to disable telemetry entirely (e.g. user opt-out).
        /// </summary>
        public bool Enabled { get; set; } = true;

        public TelemetryClient(
            string endpoint,
            string token,
            string appName,
            string appVersion)
        {
            _endpoint = endpoint.TrimEnd('/') + "/ingest/ping";
            _token = token;
            _appName = appName;
            _appVersion = appVersion;
            _osPlatform = GetOsPlatform();
            _clientId = GetOrCreateClientId();

            _http = new HttpClient();
            _http.DefaultRequestHeaders.Add("x-telemetry-token", _token);
            _http.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _http.Timeout = TimeSpan.FromSeconds(5);
        }

        /// <summary>
        /// Send a telemetry event. Fire-and-forget — failures are silently ignored
        /// so telemetry never interferes with the user's workflow.
        /// </summary>
        public async Task SendAsync(string eventName)
        {
            if (!Enabled) return;

#if DEBUG
            // Skip telemetry when debugging. Comment this out to test telementry.
            return;
#endif

            try
            {
                var payload = new
                {
                    app_name = _appName,
                    app_version = _appVersion,
                    os_platform = _osPlatform,
                    client_id = _clientId,
                    @event = eventName
                };

                // Use Newtonsoft.Json for serialization
                var json = JsonConvert.SerializeObject(payload);

                using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    using (var response = await _http.PostAsync(_endpoint, content).ConfigureAwait(false))
                    {
                        System.Diagnostics.Debug.Print("Telemetry Response: {0}", response);
                        // We don't throw on failure — telemetry should be invisible
                    }
                }
            }
            catch
            {
                // Swallow all exceptions. Telemetry must never crash the app.
            }
        }

        /// <summary>
        /// Fire-and-forget wrapper for use in synchronous contexts.
        /// </summary>
        public void Send(string eventName)
        {
            _ = Task.Run(() => SendAsync(eventName));
        }

        public void Dispose()
        {
            _http?.Dispose();
        }

        // ── Private helpers ────────────────────────────────────

        private static string GetOsPlatform()
        {
            // .NET Framework 4.8 compatible OS detection
            var platform = Environment.OSVersion.Platform;
            switch (platform)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                    return "win32";
                case PlatformID.MacOSX:
                    return "darwin";
                case PlatformID.Unix:
                    // Distinguish between Linux and macOS if possible
                    // On .NET Framework, MacOSX is rarely used, so Unix is usually Linux
                    return "linux";
                default:
                    return platform.ToString().ToLowerInvariant();
            }
        }

        /// <summary>
        /// Reads or creates a persistent anonymous client ID stored in the
        /// user's local application data folder.
        /// </summary>
        private static string GetOrCreateClientId()
        {
            if (string.IsNullOrEmpty(Properties.Settings.Default.InstallationID))
            {
                Properties.Settings.Default.InstallationID = Guid.NewGuid().ToString();
                Properties.Settings.Default.Save();
            }

            return Properties.Settings.Default.InstallationID;
        }
    }
}