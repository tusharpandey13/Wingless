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
		Me.CustomTab1 = New WindowsApplication3.CustomTab()
		Me.TabPage1 = New System.Windows.Forms.TabPage()
		Me.Custom_Material_Button1 = New WindowsApplication3.custom_Material_Button()
		Me.TabPage2 = New System.Windows.Forms.TabPage()
		Me.CustomButton1 = New WindowsApplication3.CustomButton()
		Me.CustomTrackBar1 = New WindowsApplication3.CustomTrackBar()
		Me.CustomTab1.SuspendLayout()
		Me.TabPage1.SuspendLayout()
		Me.SuspendLayout()
		'
		'CustomTab1
		'
		Me.CustomTab1.animating = False
		Me.CustomTab1.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(250, Byte), Integer), CType(CType(250, Byte), Integer), CType(CType(250, Byte), Integer))
		Me.CustomTab1.Controls.Add(Me.TabPage1)
		Me.CustomTab1.Controls.Add(Me.TabPage2)
		Me.CustomTab1.HighlightColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(82, Byte), Integer), CType(CType(82, Byte), Integer))
		Me.CustomTab1.ItemSize = New System.Drawing.Size(25, 30)
		Me.CustomTab1.Location = New System.Drawing.Point(25, 61)
		Me.CustomTab1.Name = "CustomTab1"
		Me.CustomTab1.SelectedIndex = 0
		Me.CustomTab1.Size = New System.Drawing.Size(314, 358)
		Me.CustomTab1.TabIndex = 0
		'
		'TabPage1
		'
		Me.TabPage1.BackColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(237, Byte), Integer))
		Me.TabPage1.Controls.Add(Me.Custom_Material_Button1)
		Me.TabPage1.Location = New System.Drawing.Point(4, 34)
		Me.TabPage1.Name = "TabPage1"
		Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
		Me.TabPage1.Size = New System.Drawing.Size(306, 320)
		Me.TabPage1.TabIndex = 0
		Me.TabPage1.Text = "TabPage1"
		'
		'Custom_Material_Button1
		'
		Me.Custom_Material_Button1.animating = False
		Me.Custom_Material_Button1.BackColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(237, Byte), Integer))
		Me.Custom_Material_Button1.Customization = "154,50,255,237,237,237,255,112,128,144,Custom_Material_Button1,Segoe UI,9,False,F" &
	"alse,False,False"
		Me.Custom_Material_Button1.FloodColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
		Me.Custom_Material_Button1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
		Me.Custom_Material_Button1.ForeColor = System.Drawing.Color.SlateGray
		Me.Custom_Material_Button1.Image = Nothing
		Me.Custom_Material_Button1.Location = New System.Drawing.Point(67, 89)
		Me.Custom_Material_Button1.Name = "Custom_Material_Button1"
		Me.Custom_Material_Button1.Size = New System.Drawing.Size(154, 50)
		Me.Custom_Material_Button1.TabIndex = 0
		Me.Custom_Material_Button1.Text = "Custom_Material_Button1"
		Me.Custom_Material_Button1.Transparent = False
		'
		'TabPage2
		'
		Me.TabPage2.BackColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(237, Byte), Integer))
		Me.TabPage2.Location = New System.Drawing.Point(4, 34)
		Me.TabPage2.Name = "TabPage2"
		Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
		Me.TabPage2.Size = New System.Drawing.Size(306, 320)
		Me.TabPage2.TabIndex = 1
		Me.TabPage2.Text = "TabPage2"
		'
		'CustomButton1
		'
		Me.CustomButton1.animating = False
		Me.CustomButton1.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer))
		Me.CustomButton1.Customization = "120,60,255,220,220,220,255,28,28,28,CustomButton1,Segoe UI,9,False,False,False,Fa" &
	"lse"
		Me.CustomButton1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
		Me.CustomButton1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(28, Byte), Integer), CType(CType(28, Byte), Integer))
		Me.CustomButton1.Image = Nothing
		Me.CustomButton1.Location = New System.Drawing.Point(447, 107)
		Me.CustomButton1.Name = "CustomButton1"
		Me.CustomButton1.Size = New System.Drawing.Size(120, 60)
		Me.CustomButton1.TabIndex = 1
		Me.CustomButton1.Text = "CustomButton1"
		Me.CustomButton1.Transparent = False
		'
		'CustomTrackBar1
		'
		Me.CustomTrackBar1.AccessibleDescription = "Animated Controll"
		Me.CustomTrackBar1.animating = False
		Me.CustomTrackBar1.Customization = "234,60,255,255,255,255,255,220,220,220,CustomTrackBar1,Segoe UI,9,False,False,Fal" &
	"se,False"
		Me.CustomTrackBar1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
		Me.CustomTrackBar1.HandleColor = System.Drawing.Color.Empty
		Me.CustomTrackBar1.Image = Nothing
		Me.CustomTrackBar1.Location = New System.Drawing.Point(397, 283)
		Me.CustomTrackBar1.Maximum = 100
		Me.CustomTrackBar1.Name = "CustomTrackBar1"
		Me.CustomTrackBar1.Size = New System.Drawing.Size(234, 60)
		Me.CustomTrackBar1.TabIndex = 2
		Me.CustomTrackBar1.Text = "CustomTrackBar1"
		Me.CustomTrackBar1.Transparent = False
		Me.CustomTrackBar1.Value = 0
		'
		'Form1
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.Color.White
		Me.ClientSize = New System.Drawing.Size(688, 449)
		Me.Controls.Add(Me.CustomTrackBar1)
		Me.Controls.Add(Me.CustomButton1)
		Me.Controls.Add(Me.CustomTab1)
		Me.ForeColor = System.Drawing.Color.Gainsboro
		Me.MinimumSize = New System.Drawing.Size(100, 39)
		Me.Name = "Form1"
		Me.Opacity = 1.0R
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Form1"
		Me.TransparencyKey = System.Drawing.Color.Fuchsia
		Me.CustomTab1.ResumeLayout(False)
		Me.TabPage1.ResumeLayout(False)
		Me.ResumeLayout(False)

	End Sub

	Friend WithEvents CustomTab1 As CustomTab
	Friend WithEvents TabPage1 As TabPage
	Friend WithEvents TabPage2 As TabPage
	Friend WithEvents CustomButton1 As CustomButton
	Friend WithEvents Custom_Material_Button1 As custom_Material_Button
	Friend WithEvents CustomTrackBar1 As CustomTrackBar
End Class
