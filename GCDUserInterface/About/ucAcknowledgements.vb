Imports System.Windows.Forms

Namespace About

    Public Class ucAcknowledgements

        Private Sub ucAcknowledgements_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

            Try
                AddLink(lnkNSF, "Award #1226127", "http://www.nsf.gov/awardsearch/showAward?AWD_ID=1226127")
                AddLink(lnkNSF, "ZCloud Tools", "http://zcloudtools.boisestate.edu")

                lnkELR.Links.Clear()
                AddLink(lnkELR, "Eco Logical Research", "https://sites.google.com/a/ecologicalresearch.net/ecologicalreseach-net")

                lnkGCMRC.Links.Add(2, lnkGCMRC.Text.Length - 2, "http://www.gcmrc.gov/gcmrc.aspx")
                lnkUSACE.Links.Add(2, lnkUSACE.Text.Length - 2, "http://www.nwk.usace.army.mil/")

                lnkICRRR.Links.Clear()
                AddLink(lnkICRRR, "ICRRR", "https://www.cnr.usu.edu/icrrr/")

                AddLink(lnkjDevelopers, "Joe Wheaton", "http://gcd.joewheaton.org/")
                AddLink(lnkjDevelopers, "Utah State University Department of Watershed Sciences", "http://www.cnr.usu.edu/wats/")
                AddLink(lnkjDevelopers, "James Brasington", "http://www.geog.qmul.ac.uk/staff/brasingtonj.html")
                AddLink(lnkjDevelopers, "Queen Mary University", "http://www.geog.qmul.ac.uk/staff/brasingtonj.html")
                AddLink(lnkjDevelopers, "North Arrow Research", "http://northarrowresearch.com/")
                AddLink(lnkjDevelopers, "ET-AL", "http://etal.joewheaton.org/")
                AddLink(lnkjDevelopers, "Philip Bailey", "http://northarrowresearch.com/people/")
                AddLink(lnkjDevelopers, "James Hensleigh", "http://etal.joewheaton.org/people/researchers-technicians/james-hensleigh")
                AddLink(lnkjDevelopers, "Frank Poulsen", "http://essa.com/about-essa/our-team/frank-poulsen/")
                AddLink(lnkjDevelopers, "ESSA", "http://essa.com/")
                AddLink(lnkjDevelopers, "Chris Gerrard", "http://www.gis.usu.edu/~chrisg/")
                AddLink(lnkjDevelopers, "RSGIS Lab", "http://www.gis.usu.edu/")

            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try

        End Sub

        Private Sub AddLink(ByRef lnk As LinkLabel, sText As String, sURL As String)

            Dim nStart As Integer = lnk.Text.IndexOf(sText)
            Dim nLength As Integer = sText.Length
            'lnkUSACE.Links.Add(nStart, nLength, sURL)

            ' do proper validation and add only proper links
            ' this will help you to avoid the exception 
            If Not String.IsNullOrEmpty(sText) AndAlso nStart >= 0 AndAlso (nStart + nLength <= lnk.Text.Length) Then
                Dim link As New LinkLabel.Link()
                'link.Description = textDescription.Text.ToString()
                link.LinkData = sURL
                link.Name = sText
                'link.Enabled = checkBoxEnabled.Checked
                'link.Visited = checkBoxVisited.Checked
                link.Start = nStart
                link.Length = nLength
                Try
                    lnk.Links.Add(link)
                Catch exception As InvalidOperationException
                    ' links can't overlap 
                    MessageBox.Show(exception.Message)
                End Try
            End If
        End Sub


        Private Sub LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles _
        lnkNSF.LinkClicked,
        lnkELR.LinkClicked,
        lnkGCMRC.LinkClicked,
        lnkUSACE.LinkClicked,
        lnkICRRR.LinkClicked,
        lnkjDevelopers.LinkClicked

            ' Displays the appropriate link based on the value of the LinkData property of the Link object. 
            Dim target As String = CType(e.Link.LinkData, String)
            If target IsNot Nothing Then
                System.Diagnostics.Process.Start(target)
            End If
        End Sub
    End Class

End Namespace