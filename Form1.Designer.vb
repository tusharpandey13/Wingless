<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits CustomWindow

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
        Me.Custom_Material_Button1 = New WindowsApplication3.custom_Material_Button()
        Me.CustomTrackBar1 = New WindowsApplication3.CustomTrackBar()
        Me.SuspendLayout()
        '
        'Custom_Material_Button1
        '
        Me.Custom_Material_Button1.animating = False
        Me.Custom_Material_Button1.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.Custom_Material_Button1.Customization = "169,80,255,240,240,240,255,51,51,51,Custom_Material_Button1,Segoe UI,9,False,Fals" &
    "e,False,False"
        Me.Custom_Material_Button1.FloodColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Custom_Material_Button1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Custom_Material_Button1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(51, Byte), Integer), CType(CType(51, Byte), Integer))
        Me.Custom_Material_Button1.Image = Nothing
        Me.Custom_Material_Button1.Location = New System.Drawing.Point(344, 108)
        Me.Custom_Material_Button1.myProperty = ""
        Me.Custom_Material_Button1.Name = "Custom_Material_Button1"
        Me.Custom_Material_Button1.Size = New System.Drawing.Size(169, 80)
        Me.Custom_Material_Button1.TabIndex = 0
        Me.Custom_Material_Button1.Text = "Custom_Material_Button1"
        Me.Custom_Material_Button1.Transparent = False
        '
        'CustomTrackBar1
        '
        Me.CustomTrackBar1.animating = False
        Me.CustomTrackBar1.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.CustomTrackBar1.Customization = "262,60,255,240,240,240,255,51,51,51,CustomTrackBar1,Segoe UI,9,False,False,False," &
    "False"
        Me.CustomTrackBar1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.CustomTrackBar1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(51, Byte), Integer), CType(CType(51, Byte), Integer))
        Me.CustomTrackBar1.HandleColor = System.Drawing.Color.Empty
        Me.CustomTrackBar1.Image = Nothing
        Me.CustomTrackBar1.Location = New System.Drawing.Point(81, 94)
        Me.CustomTrackBar1.Maximum = 100
        Me.CustomTrackBar1.Name = "CustomTrackBar1"
        Me.CustomTrackBar1.Size = New System.Drawing.Size(262, 60)
        Me.CustomTrackBar1.TabIndex = 1
        Me.CustomTrackBar1.Text = "CustomTrackBar1"
        Me.CustomTrackBar1.Transparent = False
        Me.CustomTrackBar1.Value = 0
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(525, 368)
        Me.Controls.Add(Me.CustomTrackBar1)
        Me.Controls.Add(Me.Custom_Material_Button1)
        Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(51, Byte), Integer), CType(CType(51, Byte), Integer))
        Me.MinimumSize = New System.Drawing.Size(100, 39)
        Me.Name = "Form1"
        Me.Opacity = 1.0R
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        Me.TransparencyKey = System.Drawing.Color.Fuchsia
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Custom_Material_Button1 As custom_Material_Button
    Friend WithEvents CustomTrackBar1 As CustomTrackBar
End Class
