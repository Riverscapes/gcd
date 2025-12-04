using ArcGIS.Desktop.Internal.Catalog.PropertyPages.NetworkDataset;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace GCDViewer
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();

            //this.Icon = new BitmapImage(new Uri("pack://application:,,,/Images/viewer16.png"));

            //imgLogo.Source = new BitmapImage(new Uri("pack://application:,,,/Images/viewer256.png"));
            txtVersion.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            SetHyperlink(lnkChangeLog, Properties.Resources.ChangeLog);
            SetHyperlink(lnkWebsite, Properties.Resources.HelpUrl);
            SetHyperlink(lnkAcknlowledgements, Properties.Resources.AcknowledgementsURL);
        }

        private void ChangeLog_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(Properties.Resources.ChangeLog) { UseShellExecute = true });
            e.Handled = true;
        }

        private void WebSite_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(Properties.Resources.HelpUrl) { UseShellExecute = true });
            e.Handled = true;
        }

        private void Acknowledgements_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(Properties.Resources.AcknowledgementsURL) { UseShellExecute = true });
            e.Handled = true;
        }

        private void SetHyperlink(Hyperlink hypControl, string url)
        {
            // Set the display text
            hypControl.Inlines.Clear();
            hypControl.Inlines.Add(url);

            // Set the NavigateUri
            hypControl.NavigateUri = new Uri(url);
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
