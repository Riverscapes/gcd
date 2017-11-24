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
using naru.db;
using System.Windows.Forms;

namespace GCDUserInterface.SurveyLibrary
{

	public partial class frmSurveyDateTime
	{


		private SurveyDateTime m_dateTime;
		public frmSurveyDateTime(SurveyDateTime sdt)
		{
			Load += frmSurveyDateTime_Load;
			// This call is required by the designer.
			InitializeComponent();
			m_dateTime = sdt;
		}

		public SurveyDateTime SurveyDateTime {
			get { return m_dateTime; }
		}


		private void frmSurveyDateTime_Load(System.Object sender, System.EventArgs e)
		{
			int nIndex = 0;

			cboYear.Items.Add(new NamedObject(0, "YYYY"));
			cboYear.SelectedIndex = 0;
			for (int nYear = 1970; nYear <= DateAndTime.Now.Year + 5; nYear++) {
				nIndex = cboYear.Items.Add(new NamedObject(nYear, nYear));
				if (nYear == m_dateTime.Year) {
					cboYear.SelectedIndex = nIndex;
				}
			}

			cboMonth.Items.Add(new NamedObject(0, "MM"));
			cboMonth.SelectedIndex = 0;
			for (int nMonth = 1; nMonth <= 12; nMonth++) {
				DateTime dt = new DateTime(1970, nMonth, 1);
				nIndex = cboMonth.Items.Add(new NamedObject(nMonth, dt.ToString("MMM")));
				if (nMonth == m_dateTime.Month) {
					cboMonth.SelectedIndex = nMonth;
				}
			}

			// Note that the days of the month are loaded repeatedly
			// when the year or month dropdowns change their selection
			// and that this is triggered by the lines of code above
			ReLoadDaysOfMonth(m_dateTime.Day);

			cboHour.Items.Add(new NamedObject(-1, "HH"));
			cboHour.SelectedIndex = 0;
			for (int nHour = 0; nHour <= 24; nHour++) {
				nIndex = cboHour.Items.Add(new NamedObject(nHour, nHour.ToString("00")));
				if (nHour == m_dateTime.Hour) {
					cboHour.SelectedIndex = nIndex;
				}
			}

			cboMinute.Items.Add(new naru.db.NamedObject(-1, "MM"));
			cboMinute.SelectedIndex = 0;
			for (int nMin = 0; nMin <= 59; nMin++) {
				nIndex = cboMinute.Items.Add(new NamedObject(nMin, nMin.ToString("00")));
				if (nMin == m_dateTime.Minute) {
					cboMinute.SelectedIndex = nIndex;
				}
			}

		}

		private bool ValidateForm()
		{

			if (((NamedObject)cboYear.SelectedItem).ID == 0 && ((NamedObject)cboMonth.SelectedItem).ID != 0) {
				MessageBox.Show("You must select a year if you want to specify a month.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return false;
			}

			if (((NamedObject)cboYear.SelectedItem).ID == 0 && ((NamedObject)cboMonth.SelectedItem).ID == 0 && ((NamedObject)cboDay.SelectedItem).ID != 0) {
				MessageBox.Show("You must select a year and month if you want to specify a day.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return false;
			}

			if (((NamedObject)cboHour.SelectedItem).ID == -1 && ((NamedObject)cboMinute.SelectedItem).ID > -1) {
				MessageBox.Show("You must also select the hour if you want to specify the minute of the hour.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return false;
			}

			return true;

		}


		private void cmdSave_Click(object sender, System.EventArgs e)
		{
			if (!ValidateForm()) {
				this.DialogResult = System.Windows.Forms.DialogResult.None;
				return;
			}

			m_dateTime = new GCDCore.Project.SurveyDateTime();
			m_dateTime.Year = ((NamedObject)cboYear.SelectedItem).ID;
			m_dateTime.Month = ((NamedObject)cboMonth.SelectedItem).ID;
			m_dateTime.Day = ((NamedObject)cboDay.SelectedItem).ID;

			m_dateTime.Hour = ((NamedObject)cboHour.SelectedItem).ID;
			m_dateTime.Minute = ((NamedObject)cboMinute.SelectedItem).ID;

		}

		/// <summary>
		/// Reload the days of the month when the year or month changes
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks></remarks>

		private void cboYear_SelectedIndexChanged(System.Object sender, System.EventArgs e)
		{
			int nCurrentDay = 0;
			if (cboDay.SelectedItem is NamedObject) {
				nCurrentDay = ((NamedObject)cboDay.SelectedItem).ID;
			}

			ReLoadDaysOfMonth(nCurrentDay);

		}


		private void ReLoadDaysOfMonth(int nSelectDay)
		{
			int nMaxDays = 31;
			if (cboYear.SelectedItem is NamedObject) {
				ushort nYear = ((NamedObject)cboYear.SelectedItem).ID;
				if (nYear > 0) {
					if (cboMonth.SelectedItem is NamedObject) {
						byte nMonth = ((NamedObject)cboMonth.SelectedItem).ID;
						if (nMonth > 0) {
							nMaxDays = DateTime.DaysInMonth(nYear, nMonth);
						}
					}
				}
			}

			cboDay.Items.Clear();
			cboDay.Items.Add(new NamedObject(0, "DD"));
			cboDay.SelectedIndex = 0;
			for (int nDay = 1; nDay <= nMaxDays; nDay++) {
				int nIndex = cboDay.Items.Add(new NamedObject(nDay, nDay));
				if (nDay == nSelectDay) {
					cboDay.SelectedIndex = nIndex;
				}
			}
		}
	}

}
