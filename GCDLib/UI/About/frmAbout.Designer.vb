Namespace UI.About
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class frmAbout
        Inherits System.Windows.Forms.Form

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAbout))
            Me.btnOK = New System.Windows.Forms.Button()
            Me.Panel1 = New System.Windows.Forms.Panel()
            Me.lblGCDCoreVersion = New System.Windows.Forms.Label()
            Me.Label9 = New System.Windows.Forms.Label()
            Me.lblRMVersion = New System.Windows.Forms.Label()
            Me.Label7 = New System.Windows.Forms.Label()
            Me.GroupBox2 = New System.Windows.Forms.GroupBox()
            Me.Panel2 = New System.Windows.Forms.Panel()
            Me.GroupBox1 = New System.Windows.Forms.GroupBox()
            Me.LinkLabel3 = New System.Windows.Forms.LinkLabel()
            Me.LinkLabel2 = New System.Windows.Forms.LinkLabel()
            Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
            Me.Label5 = New System.Windows.Forms.Label()
            Me.Label4 = New System.Windows.Forms.Label()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.lblVersion = New System.Windows.Forms.Label()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.lblAppTitle = New System.Windows.Forms.Label()
            Me.PictureBox1 = New System.Windows.Forms.PictureBox()
            Me.Panel1.SuspendLayout()
            Me.GroupBox2.SuspendLayout()
            Me.GroupBox1.SuspendLayout()
            CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'btnOK
            '
            Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.btnOK.Location = New System.Drawing.Point(590, 637)
            Me.btnOK.Name = "btnOK"
            Me.btnOK.Size = New System.Drawing.Size(75, 23)
            Me.btnOK.TabIndex = 0
            Me.btnOK.Text = "OK"
            Me.btnOK.UseVisualStyleBackColor = True
            '
            'Panel1
            '
            Me.Panel1.BackColor = System.Drawing.SystemColors.Window
            Me.Panel1.Controls.Add(Me.lblGCDCoreVersion)
            Me.Panel1.Controls.Add(Me.Label9)
            Me.Panel1.Controls.Add(Me.lblRMVersion)
            Me.Panel1.Controls.Add(Me.Label7)
            Me.Panel1.Controls.Add(Me.GroupBox2)
            Me.Panel1.Controls.Add(Me.GroupBox1)
            Me.Panel1.Controls.Add(Me.lblVersion)
            Me.Panel1.Controls.Add(Me.Label1)
            Me.Panel1.Controls.Add(Me.lblAppTitle)
            Me.Panel1.Controls.Add(Me.PictureBox1)
            Me.Panel1.Location = New System.Drawing.Point(12, 12)
            Me.Panel1.Name = "Panel1"
            Me.Panel1.Size = New System.Drawing.Size(653, 619)
            Me.Panel1.TabIndex = 1
            '
            'lblGCDCoreVersion
            '
            Me.lblGCDCoreVersion.AutoSize = True
            Me.lblGCDCoreVersion.Location = New System.Drawing.Point(357, 83)
            Me.lblGCDCoreVersion.Name = "lblGCDCoreVersion"
            Me.lblGCDCoreVersion.Size = New System.Drawing.Size(40, 13)
            Me.lblGCDCoreVersion.TabIndex = 15
            Me.lblGCDCoreVersion.Text = "5.0.0.0"
            '
            'Label9
            '
            Me.Label9.AutoSize = True
            Me.Label9.Location = New System.Drawing.Point(258, 83)
            Me.Label9.Name = "Label9"
            Me.Label9.Size = New System.Drawing.Size(95, 13)
            Me.Label9.TabIndex = 14
            Me.Label9.Text = "GCD Core version:"
            '
            'lblRMVersion
            '
            Me.lblRMVersion.AutoSize = True
            Me.lblRMVersion.Location = New System.Drawing.Point(357, 60)
            Me.lblRMVersion.Name = "lblRMVersion"
            Me.lblRMVersion.Size = New System.Drawing.Size(40, 13)
            Me.lblRMVersion.TabIndex = 13
            Me.lblRMVersion.Text = "5.0.0.0"
            '
            'Label7
            '
            Me.Label7.AutoSize = True
            Me.Label7.Location = New System.Drawing.Point(230, 60)
            Me.Label7.Name = "Label7"
            Me.Label7.Size = New System.Drawing.Size(123, 13)
            Me.Label7.TabIndex = 12
            Me.Label7.Text = "Raster Manager version:"
            '
            'GroupBox2
            '
            Me.GroupBox2.Controls.Add(Me.Panel2)
            Me.GroupBox2.Location = New System.Drawing.Point(3, 198)
            Me.GroupBox2.Name = "GroupBox2"
            Me.GroupBox2.Size = New System.Drawing.Size(637, 417)
            Me.GroupBox2.TabIndex = 11
            Me.GroupBox2.TabStop = False
            Me.GroupBox2.Text = "Acknowledgements"
            '
            'Panel2
            '
            Me.Panel2.AutoScroll = True
            Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.Panel2.Location = New System.Drawing.Point(3, 16)
            Me.Panel2.Name = "Panel2"
            Me.Panel2.Size = New System.Drawing.Size(631, 398)
            Me.Panel2.TabIndex = 0
            '
            'GroupBox1
            '
            Me.GroupBox1.Controls.Add(Me.LinkLabel3)
            Me.GroupBox1.Controls.Add(Me.LinkLabel2)
            Me.GroupBox1.Controls.Add(Me.LinkLabel1)
            Me.GroupBox1.Controls.Add(Me.Label5)
            Me.GroupBox1.Controls.Add(Me.Label4)
            Me.GroupBox1.Controls.Add(Me.Label3)
            Me.GroupBox1.Location = New System.Drawing.Point(230, 102)
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.Size = New System.Drawing.Size(410, 90)
            Me.GroupBox1.TabIndex = 10
            Me.GroupBox1.TabStop = False
            Me.GroupBox1.Text = "Support"
            '
            'LinkLabel3
            '
            Me.LinkLabel3.AutoSize = True
            Me.LinkLabel3.Location = New System.Drawing.Point(74, 66)
            Me.LinkLabel3.Name = "LinkLabel3"
            Me.LinkLabel3.Size = New System.Drawing.Size(109, 13)
            Me.LinkLabel3.TabIndex = 6
            Me.LinkLabel3.TabStop = True
            Me.LinkLabel3.Text = "gcd@joewheaton.org"
            '
            'LinkLabel2
            '
            Me.LinkLabel2.AutoSize = True
            Me.LinkLabel2.LinkColor = System.Drawing.Color.Blue
            Me.LinkLabel2.Location = New System.Drawing.Point(74, 43)
            Me.LinkLabel2.Name = "LinkLabel2"
            Me.LinkLabel2.Size = New System.Drawing.Size(158, 13)
            Me.LinkLabel2.TabIndex = 5
            Me.LinkLabel2.TabStop = True
            Me.LinkLabel2.Text = "http://gcd6help.joewheaton.org"
            '
            'LinkLabel1
            '
            Me.LinkLabel1.AutoSize = True
            Me.LinkLabel1.Location = New System.Drawing.Point(74, 20)
            Me.LinkLabel1.Name = "LinkLabel1"
            Me.LinkLabel1.Size = New System.Drawing.Size(132, 13)
            Me.LinkLabel1.TabIndex = 4
            Me.LinkLabel1.TabStop = True
            Me.LinkLabel1.Text = "http://gcd.joewheaton.org"
            '
            'Label5
            '
            Me.Label5.AutoSize = True
            Me.Label5.Location = New System.Drawing.Point(7, 66)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New System.Drawing.Size(35, 13)
            Me.Label5.TabIndex = 3
            Me.Label5.Text = "Email:"
            '
            'Label4
            '
            Me.Label4.AutoSize = True
            Me.Label4.Location = New System.Drawing.Point(7, 43)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New System.Drawing.Size(63, 13)
            Me.Label4.TabIndex = 2
            Me.Label4.Text = "Online help:"
            '
            'Label3
            '
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(7, 20)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(52, 13)
            Me.Label3.TabIndex = 1
            Me.Label3.Text = "Web site:"
            '
            'lblVersion
            '
            Me.lblVersion.AutoSize = True
            Me.lblVersion.Location = New System.Drawing.Point(357, 37)
            Me.lblVersion.Name = "lblVersion"
            Me.lblVersion.Size = New System.Drawing.Size(40, 13)
            Me.lblVersion.TabIndex = 9
            Me.lblVersion.Text = "5.0.0.0"
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(283, 37)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(70, 13)
            Me.Label1.TabIndex = 8
            Me.Label1.Text = "GCD version:"
            '
            'lblAppTitle
            '
            Me.lblAppTitle.AutoSize = True
            Me.lblAppTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblAppTitle.Location = New System.Drawing.Point(230, 5)
            Me.lblAppTitle.Name = "lblAppTitle"
            Me.lblAppTitle.Size = New System.Drawing.Size(298, 24)
            Me.lblAppTitle.TabIndex = 7
            Me.lblAppTitle.Text = "Geomorphic Change Detection"
            '
            'PictureBox1
            '
            Me.PictureBox1.Image = My.Resources.Resources.GCD_SplashLogo_200
            Me.PictureBox1.Location = New System.Drawing.Point(3, 3)
            Me.PictureBox1.Name = "PictureBox1"
            Me.PictureBox1.Size = New System.Drawing.Size(221, 189)
            Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
            Me.PictureBox1.TabIndex = 0
            Me.PictureBox1.TabStop = False
            '
            'AboutForm
            '
            Me.AcceptButton = Me.btnOK
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.btnOK
            Me.ClientSize = New System.Drawing.Size(677, 672)
            Me.Controls.Add(Me.Panel1)
            Me.Controls.Add(Me.btnOK)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "AboutForm"
            Me.Text = "About the Geomorphic Change Detection ArcGIS AddIn"
            Me.Panel1.ResumeLayout(False)
            Me.Panel1.PerformLayout()
            Me.GroupBox2.ResumeLayout(False)
            Me.GroupBox1.ResumeLayout(False)
            Me.GroupBox1.PerformLayout()
            CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents btnOK As System.Windows.Forms.Button
        Friend WithEvents Panel1 As System.Windows.Forms.Panel
        Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
        Friend WithEvents lblVersion As System.Windows.Forms.Label
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents lblAppTitle As System.Windows.Forms.Label
        Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
        Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
        Friend WithEvents LinkLabel3 As System.Windows.Forms.LinkLabel
        Friend WithEvents LinkLabel2 As System.Windows.Forms.LinkLabel
        Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents Panel2 As System.Windows.Forms.Panel
        Friend WithEvents lblGCDCoreVersion As System.Windows.Forms.Label
        Friend WithEvents Label9 As System.Windows.Forms.Label
        Friend WithEvents lblRMVersion As System.Windows.Forms.Label
        Friend WithEvents Label7 As System.Windows.Forms.Label
    End Class
End Namespace