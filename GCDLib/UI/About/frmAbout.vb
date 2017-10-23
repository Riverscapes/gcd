Namespace UI.About

    Public Class frmAbout

        Private Sub AboutForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Panel2.Controls.Add(New ucAcknowledgements)

            Dim sProduct As String = "Standalone"
            If Reflection.Assembly.GetEntryAssembly().FullName.ToLower().Contains("arcmap") Then
                sProduct = "Addin"
            End If

            Dim ass As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly
            lblVersion.Text = String.Format("{0} {1}", sProduct, ass.GetName.Version)

            Try
                'lblRMVersion.Text = RasterManager.GetFileVersion().FileVersion.ToString()
                'lblGCDCoreVersion.Text = GCDCore.GetFileVersion().FileVersion.ToString()
            Catch ex As Exception
                naru.error.ExceptionUI.HandleException(ex)
            End Try

        End Sub

        Private Sub LinkLabel8_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
            Process.Start("http://www.essa.com/team/index.html#fp")
        End Sub

        Private Sub LinkLabel9_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
            Process.Start("http://www.essa.com/team/index.html#no")
        End Sub

        Private Sub LinkLabel6_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
            Process.Start("http://northarrowresearch.com/north-arrow-research/people/")
        End Sub

        Private Sub LinkLabel4_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
            Process.Start(My.Resources.PeopleJoeWheatonURL)
        End Sub

        'TODO ask Philip if this should be a link label, requires user to have outlook or other email service set up and is causing errors becasue of that
        Private Sub LinkLabel3_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
            'hard coded path to GCD 6 Forum, perhaps the GCD 6 forum needs a more descriptive web address
            Process.Start("http://forum.bluezone.usu.edu/gcd/viewforum.php?f=42&sid=d221c1def27836aff25624551c6e56d1")
        End Sub

        Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
            Process.Start(My.Resources.GCDWebSiteURL)
        End Sub

        Private Sub LinkLabel2_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
            Process.Start(My.Resources.HelpBaseURL)
        End Sub

    End Class

End Namespace