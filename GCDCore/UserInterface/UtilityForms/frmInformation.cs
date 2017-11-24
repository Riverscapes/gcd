using System;
using System.Windows.Forms;

namespace GCDCore.UserInterface.UtilityForms
{
    public partial class frmInformation : Form
    {
        private string m_sFormTitle;
        private string m_sMessage;

        public frmInformation()
        {
            InitializeComponent();
        }

        public void InitializeSizeable(string sTitle, string sMessage, int nMaxWidth, int nMaxHeight)
        {
            m_sFormTitle = sTitle;
            m_sMessage = sMessage;
            this.MaximumSize = new System.Drawing.Size(nMaxWidth, nMaxHeight);
        }

        public void InitializeFixedDialog(string sTitle, string sMessage)
        {
            m_sFormTitle = sTitle;
            m_sMessage = sMessage;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        }

        private void InformationForm_Load(object sender, System.EventArgs e)
        {
            this.Text = m_sFormTitle;
            txtMessage.Text = m_sMessage;
        }
    }
}