using GCDCore.Project;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
namespace GCDUserInterface.ChangeDetection
{

	public partial class frmCoherenceProperties
	{


		private int m_nFilterSize;
		public int FilterSize {
			get { return m_nFilterSize; }
			set { m_nFilterSize = value; }
		}

		public int PercentLess {
			get { return numLess.Value; }
			set { numLess.Value = value; }
		}

		public int PercentGreater {
			get { return numGreater.Value; }
			set { numGreater.Value = value; }
		}


		private void CoherencePropertiesForm_Load(System.Object sender, System.EventArgs e)
		{
			string sFilterSizeText = m_nFilterSize.ToString() + " x " + m_nFilterSize.ToString();
			cboFilterSize.SelectedItem = sFilterSizeText;
		}


		private void cboFilterSize_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int i = cboFilterSize.Text.IndexOf(" ");
			if (i > 0) {
				m_nFilterSize = cboFilterSize.Text.Substring(0, i);
			}
		}


		public frmCoherenceProperties()
		{
			Load += CoherencePropertiesForm_Load;
			// This call is required by the designer.
			InitializeComponent();

			// Default filter size.
			m_nFilterSize = 5;
		}

		private void btnHelp_Click(System.Object sender, System.EventArgs e)
		{
			//Process.Start(My.Resources.HelpBaseURL & "")
		}
	}

}
