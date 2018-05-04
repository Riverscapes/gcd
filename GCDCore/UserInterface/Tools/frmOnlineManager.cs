using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.IO.Compression;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;

namespace GCDCore.UserInterface.Tools
{

    public partial class frmOnlineManager : Form
    {
        private static string APIKEY = "uvfenq3h5y10GEgQwCkmx9loJ5QAnV6X7mhyUbBE";

        private BackgroundWorker jobWorker;
        public event EventHandler<string> LoggerHandler;

        private CancellationTokenSource CancelTokenSource;
        CancellationToken token;

        protected void SendLogMessage(string msg) { LoggerHandler?.Invoke(this, msg); }

        public frmOnlineManager()
        {
            InitializeComponent();
            RecalcState();
        }


        private void RecalcState()
        {
            SetText(txtProg, "");
            SetEnabled(btnUpload, true);
            SetVisible(btnCancel, false);

            //SetText(lblProg, "");
            SetText(lnklblurl, "");
            SetValue(prgUpload, 0);

            if (GCDCore.Project.ProjectManager.Project.OnlineParams.ContainsKey("GCDUUID"))
            {
                SetText(lblStatus, "Online");
                SetText(btnUpload, "Re-Upload");
                SetForeColor(lblStatus, Color.Green);
                SetVisible(btnDelete, true);
                SetEnabled(btnDelete, true);
                SetText(lnklblurl, String.Format("{0}{1}", GCDCore.Properties.Resources.UploadPublicUrl, GCDCore.Project.ProjectManager.Project.OnlineParams["GCDUUID"]));
            }
            else
            {
                SetText(lblStatus, "Not Uploaded");
                SetText(btnUpload, "Upload");
                SetText(btnUpload, "Upload");
                SetForeColor(lblStatus, Color.Red);
                SetVisible(btnDelete, false);
                SetEnabled(btnDelete, false);
            }


        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            RecalcState();

            jobWorker = new BackgroundWorker();

            jobWorker.WorkerReportsProgress = true;
            jobWorker.WorkerSupportsCancellation = true;
            SetText(lblProg, "");

            CancelTokenSource = new CancellationTokenSource();
            token = CancelTokenSource.Token;

            jobWorker.DoWork += new DoWorkEventHandler(jobWorker_DoWork);
            jobWorker.ProgressChanged += new ProgressChangedEventHandler(jobWorker_ProgressChanged);
            jobWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(jobWorker_UploadCompleted);

            jobWorker.RunWorkerAsync();

        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            // Now get and store the credentials we need to upload directly to S3
            SendLogMessage(String.Format("{0} Calling Delete API...", Environment.NewLine));
            SetText(lblProg, "");

            WebRequest request = WebRequest.Create(String.Format("{0}/project/{1}", 
                GCDCore.Properties.Resources.UploadAPIUrl,
                GCDCore.Project.ProjectManager.Project.OnlineParams["GCDUUID"]));
            request.Headers.Add("GCDAUTH", GCDCore.Project.ProjectManager.Project.OnlineParams["PubKey"]);
            request.Headers.Add("x-api-key", APIKEY);
            request.Method = "DELETE";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Now wipe all the evidence and save the project
            GCDCore.Project.ProjectManager.Project.OnlineParams = new Dictionary<string, string>();
            GCDCore.Project.ProjectManager.Project.Save();
            SendLogMessage(String.Format("Deletion Complete"));

        }


        /// <summary>
        /// This is the asynchronous job handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void jobWorker_DoWork(object sender, DoWorkEventArgs e)
        {

            // Now hook up asynchronous job eventhandlers to the right connectors
            // We do the unassign first to make sure we don't get two handles 
            // -= can safely be called even when it doesn't exist
            LoggerHandler += new EventHandler<string>(loggerhandler);

            SetVisible(btnCancel, true);
            SetEnabled(btnCancel, false);
            SetEnabled(btnUpload, false);
            SetEnabled(btnDelete, false);
            try
            {
                // First zip up the contents of the folder and put them somewhere:
                SendLogMessage(String.Format("{0} Making temporary zip file...", Environment.NewLine));
                DirectoryInfo gcdprojpath = GCDCore.Project.ProjectManager.Project.ProjectFile.Directory;
                FileInfo tempzipfile = new FileInfo(Path.GetTempPath() + Guid.NewGuid().ToString() + ".zip");

                List<string> files = Directory.GetFiles(gcdprojpath.FullName, "*", SearchOption.AllDirectories).ToList();

                //Creates a new, blank zip file to work with - the file will be
                //finalized when the using statement completes
                using (ZipArchive zfile = ZipFile.Open(tempzipfile.FullName, ZipArchiveMode.Create))
                {
                    // CompressionLevel.BestCompression
                    foreach (string abspath in files)
                    {
                        string relPath = MakeRelativePath(gcdprojpath.FullName, abspath);
                        // Rename the project file so we can find it easily online
                        if (relPath.EndsWith(".gcd") && !relPath.Contains(Path.DirectorySeparatorChar))
                            relPath = "project.gcd";
                        // We normalize the slashes here to make things easier for linux to deal with
                        zfile.CreateEntryFromFile(abspath, relPath.Replace(Path.DirectorySeparatorChar, '/'));
                    }

                }
                SendLogMessage(String.Format("created."));

                // Now get and store the credentials we need to upload directly to S3
                SendLogMessage(String.Format("{0} Retrieving upload url...", Environment.NewLine));
                APIReturn jsonret;
                using (WebClient wcreq = new WebClient())
                {

                    wcreq.Headers["x-api-key"] = APIKEY;
                    string jsonString;
                    if (GCDCore.Project.ProjectManager.Project.OnlineParams.ContainsKey("GCDUUID"))
                    {
                        // This is actually an overwrite
                        wcreq.Headers["GCDAUTH"] = GCDCore.Project.ProjectManager.Project.OnlineParams["PubKey"];
                        jsonString = wcreq.UploadString(String.Format("{0}/project/{1}", GCDCore.Properties.Resources.UploadAPIUrl, 
                            GCDCore.Project.ProjectManager.Project.OnlineParams["GCDUUID"]), "POST", "");
                    }
                    // This is a new project. Upload as per usual
                    else
                        jsonString = wcreq.UploadString(String.Format("{0}/project/new", GCDCore.Properties.Resources.UploadAPIUrl), "POST", "");

                    jsonret = JsonConvert.DeserializeObject<APIReturn>(jsonString);
                    SendLogMessage(String.Format("retrieved."));
                }

                // Upload the file to S3
                SendLogMessage(String.Format("{0} Uploading zip...", Environment.NewLine));

                // Read file data
                FileStream fs = new FileStream(tempzipfile.FullName, FileMode.Open, FileAccess.Read);
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                fs.Close();

                // Generate post objects by first adding in all the fields we got back from S3 then the binary data
                Dictionary<string, object> postParameters = new Dictionary<string, object>();
                foreach (KeyValuePair<string, string> kvp in jsonret.upload_fields.fields)
                    postParameters.Add(kvp.Key, kvp.Value);
                postParameters.Add("file", new FormUpload.FileParameter(data, "project.zip", "application/zip"));

                // Create request and receive response
                FormUpload fUp = new FormUpload();
                using(HttpWebResponse webResponse = fUp.MultipartFormDataPost(jsonret.upload_fields.url, "Someone", postParameters))
                {
                    // from: https://stackoverflow.com/questions/20726797/showing-progress-in-percentage-while-uploading-and-downloading-using-httpwebrequhttps://stackoverflow.com/questions/20726797/showing-progress-in-percentage-while-uploading-and-downloading-using-httpwebrequhttps://stackoverflow.com/questions/20726797/showing-progress-in-percentage-while-uploading-and-downloading-using-httpwebrequhttps://stackoverflow.com/questions/20726797/showing-progress-in-percentage-while-uploading-and-downloading-using-httpwebrequhttps://stackoverflow.com/questions/20726797/showing-progress-in-percentage-while-uploading-and-downloading-using-httpwebrequhttps://stackoverflow.com/questions/20726797/showing-progress-in-percentage-while-uploading-and-downloading-using-httpwebrequhttps://stackoverflow.com/questions/20726797/showing-progress-in-percentage-while-uploading-and-downloading-using-httpwebrequhttps://stackoverflow.com/questions/20726797/showing-progress-in-percentage-while-uploading-and-downloading-using-httpwebrequhttps://stackoverflow.com/questions/20726797/showing-progress-in-percentage-while-uploading-and-downloading-using-httpwebrequhttps://stackoverflow.com/questions/20726797/showing-progress-in-percentage-while-uploading-and-downloading-using-httpwebrequhttps://stackoverflow.com/questions/20726797/showing-progress-in-percentage-while-uploading-and-downloading-using-httpwebrequhttps://stackoverflow.com/questions/20726797/showing-progress-in-percentage-while-uploading-and-downloading-using-httpwebrequ
                    // Allocate space for the content
                    // Process response
                    StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());

                    //var readdata = new byte[fUp.contentLength];
                    int currentIndex = 0;
                    int bytesReceived = 0;
                    var buffer = new char[256];
                    //do
                    //{
                    //    bytesReceived = responseReader.Read(buffer, 0, 256);
                    //    //Array.Copy(buffer, 0, readdata, currentIndex, bytesReceived);
                    //    currentIndex += bytesReceived;

                    //    // Report percentage
                    //    double percentage = (double)currentIndex / fUp.contentLength;
                    //    jobWorker.ReportProgress((int)(percentage * 100));
                    //} while (currentIndex < fUp.contentLength);

                    jobWorker.ReportProgress(100);
                    //string fullResponse = responseReader.ReadToEnd();
                    webResponse.Close();
                }

                // Now set the response values so we can find them later
                GCDCore.Project.ProjectManager.Project.OnlineParams["GCDUUID"] = jsonret.uuid;
                GCDCore.Project.ProjectManager.Project.OnlineParams["PubKey"] = jsonret.pubKey;

                // Clean up the temporary file
                SendLogMessage(String.Format("{0} Cleaning up...", Environment.NewLine));
                tempzipfile.Refresh();
                if (tempzipfile.Exists)
                    tempzipfile.Delete();
                SendLogMessage(String.Format("done!"));
                GCDCore.Project.ProjectManager.Project.Save();
                SendLogMessage(String.Format("{0} Process Complete", Environment.NewLine));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("EXCEPTION");
            }
            // Report something having changed.
            RecalcState();
        }


        /// <summary>
        /// When the job worker is complete we show an alert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void jobWorker_UploadCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            prgUpload.Value = 100;
        }



        private void jobWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            SetValue(prgUpload, e.ProgressPercentage);
        }


        private void loggerhandler(object somesender, string msg) { AppendText(txtProg, msg); }

        delegate void SetEnabledCallback(Control ctl, bool val);
        private void SetEnabled(Control ctl, bool val)
        {
            if (ctl.InvokeRequired)
            {
                SetEnabledCallback d = new SetEnabledCallback(SetEnabled);
                Invoke(d, new object[] { ctl, val });
            }
            else
                ctl.Enabled = val;
        }

        delegate void SetTextCallback(Control ctl, string text);
        private void SetText(Control ctl, string text)
        {
            if (ctl.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                Invoke(d, new object[] { ctl, text });
            }
            else
                ctl.Text = text;
        }

        delegate void SetVisibleCallback(Control ctl, bool vis);
        private void SetVisible(Control ctl, bool vis)
        {
            if (ctl.InvokeRequired)
            {
                SetVisibleCallback d = new SetVisibleCallback(SetVisible);
                Invoke(d, new object[] { ctl, vis });
            }
            else
                ctl.Visible = vis;
        }

        delegate void AppendTextCallback(TextBox ctl, string text);
        private void AppendText(TextBox ctl, string text)
        {
            if (ctl.InvokeRequired)
            {
                AppendTextCallback d = new AppendTextCallback(AppendText);
                Invoke(d, new object[] { ctl, text });
            }
            else
                ctl.AppendText(text);
        }

        delegate void SetValueCallback(ProgressBar ctl, int value);
        private void SetValue(ProgressBar ctl, int value)
        {
            if (ctl.InvokeRequired)
            {
                SetValueCallback d = new SetValueCallback(SetValue);
                Invoke(d, new object[] { ctl, value });
            }
            else
                ctl.Value = value;
        }

        delegate void SetForeColorCallback(Control ctl, Color value);
        private void SetForeColor(Control ctl, Color value)
        {
            if (ctl.InvokeRequired)
            {
                SetForeColorCallback d = new SetForeColorCallback(SetForeColor);
                Invoke(d, new object[] { ctl, value });
            }
            else
                ctl.ForeColor = value;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // clear the queue
            CancelTokenSource.Cancel();
            if (jobWorker != null && jobWorker.IsBusy) jobWorker.CancelAsync();

            SetVisible(btnCancel, false);
            SetEnabled(btnCancel, false);
            SetEnabled(btnUpload, true);
            SetEnabled(btnDelete, true);
        }





        /// <summary>
        /// Creates a relative path from one file or folder to another.
        /// </summary>
        /// <param name="fromPath">Contains the directory that defines the start of the relative path.</param>
        /// <param name="toPath">Contains the path that defines the endpoint of the relative path.</param>
        /// <returns>The relative path from the start directory to the end path or <c>toPath</c> if the paths are not related.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static String MakeRelativePath(String fromPath, String toPath)
        {
            if (String.IsNullOrEmpty(fromPath)) throw new ArgumentNullException("fromPath");
            if (String.IsNullOrEmpty(toPath)) throw new ArgumentNullException("toPath");

            FileAttributes attr = File.GetAttributes(fromPath);

            if (attr.HasFlag(FileAttributes.Directory))
                fromPath += Path.DirectorySeparatorChar;

            Uri fromUri = new Uri(fromPath);
            Uri toUri = new Uri(toPath);

            if (fromUri.Scheme != toUri.Scheme) { return toPath; } // path can't be made relative.

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            String relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }

        private void lnklblurl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(lnklblurl.Text as String);
        }

        private void frmOnlineManager_Load(object sender, EventArgs e)
        {
            tTip.SetToolTip(btnUpload, "Uploads the current GCD project into the cloud and returns the URL at which it can then be accessed.");
            tTip.SetToolTip(btnDelete, "Delete the online copy of the project. This does not impact the local copy on your computer.");
            tTip.SetToolTip(lnklblurl, "Click this link to open a web browser to view the online copy of this project once it has been uploaded.");
        }
    }

    /// <summary>
    /// This is just a convenience class to get Newtonsoft something it can deseriealize against
    /// </summary>
    public class APIReturn
    {
        public class APIUploadFields
        {
            public string url { get; set; }
            public IDictionary<string, string> fields { get; set; }
        }

        public string pubKey { get; set; }
        public string uuid { get; set; }
        // Header fields we will use for our upload
        public APIUploadFields upload_fields { get; set; }
    }


}