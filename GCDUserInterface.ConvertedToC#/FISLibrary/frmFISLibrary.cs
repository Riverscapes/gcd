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
namespace GCDUserInterface.FISLibrary
{

	public partial class frmFISLibrary
	{


		private void btnAddFIS_Click(System.Object sender, System.EventArgs e)
		{
			frmAddFIS AddFISForm = new frmAddFIS();
			AddFISForm.ShowDialog();

		}


		private void btnDeleteFIS_Click(System.Object sender, System.EventArgs e)
		{
			DataRowView CurrentRow = null;
			CurrentRow = FISTableBindingSource.Current;


			if ((CurrentRow != null)) {
				MsgBoxResult response = default(MsgBoxResult);
				response = Interaction.MsgBox("Are you sure you want to remove the selected FIS file from the GCD Software? Note that this will not delete the associated *.fis file.", MsgBoxStyle.YesNo | MsgBoxStyle.Question, GCDCore.Properties.Resources.ApplicationNameLong);
				if (response == MsgBoxResult.Yes) {
					if ((CurrentRow != null)) {
						//Delete the selected item from the dataset and write this new information to the XML file at the specified location
						throw new NotImplementedException();
					}
				}
			}

		}


		private void FISLibraryForm_Load(System.Object sender, System.EventArgs e)
		{
			ttpTooltip.SetToolTip(btnAddFIS, "Add a FIS file to the GCD FIS Library.");
			ttpTooltip.SetToolTip(btnEditFIS, "Edit the selected FIS file.");
			ttpTooltip.SetToolTip(btnDeleteFIS, "Delete the selected FIS file.");

			if (DataGridView1.Columns.Count == 2) {
				DataGridView1.Columns[1].Width = DataGridView1.Width - DataGridView1.Columns[0].Width - 5;
			}

			//XMLHandling.XMLReadFIS(Me.FISLibrary)

		}


		private void btnEditFIS_Click(System.Object sender, System.EventArgs e)
		{
			//Dim CurrentRow As DataRowView = FISTableBindingSource.Current
			//If Not CurrentRow Is Nothing Then
			//    If TypeOf CurrentRow.Row Is GCDLib.FISLibrary.FISTableRow Then
			//        Dim fisRow As GCDLib.FISLibrary.FISTableRow = CurrentRow.Row
			//        If IO.File.Exists(fisRow.Path) Then
			//            Try
			//                Dim frm As New frmEditFIS(fisRow.Path)
			//                frm.ShowDialog()
			//            Catch ex As Exception
			//                Dim ex2 As New Exception("Error showing FIS form.", ex)
			//                ex2.Data.Add("FIS Path", fisRow.Path)
			//                Throw ex2
			//            End Try
			//        Else
			//            MsgBox("The specified FIS file does not exist.", MsgBoxStyle.Exclamation, GCDCore.Properties.Resources.ApplicationNameLong)
			//        End If
			//    End If
			//End If
		}

		private void btnHelp_Click(System.Object sender, System.EventArgs e)
		{
			Process.Start(GCDCore.Properties.Resources.HelpBaseURL + "gcd-command-reference/customize-menu/fis-library");
		}

		private void btnFISRepo_Click(System.Object sender, System.EventArgs e)
		{
			Process.Start(GCDCore.Properties.Resources.FISRepositoryWebsite);
		}
		public frmFISLibrary()
		{
			Load += FISLibraryForm_Load;
			InitializeComponent();
		}
	}
}
