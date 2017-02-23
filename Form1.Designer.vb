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
        Me.CustomButton1 = New WindowsApplication3.CustomButton()
        Me.SuspendLayout()
        '
        'CustomButton1
        '
        Me.CustomButton1.animating = False
        Me.CustomButton1.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer))
        Me.CustomButton1.Customization = "152,69,255,220,220,220,255,28,28,28,CustomButton1,Segoe UI,9,False,False,False,Fa" &
    "lse"
        Me.CustomButton1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.CustomButton1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(28, Byte), Integer), CType(CType(28, Byte), Integer))
        Me.CustomButton1.Image = Nothing
        Me.CustomButton1.Location = New System.Drawing.Point(72, 94)
        Me.CustomButton1.Name = "CustomButton1"
        Me.CustomButton1.Size = New System.Drawing.Size(152, 69)
        Me.CustomButton1.TabIndex = 0
        Me.CustomButton1.Text = "CustomButton1"
        Me.CustomButton1.Transparent = False
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(309, 232)
        Me.Controls.Add(Me.CustomButton1)
        Me.ForeColor = System.Drawing.Color.Gainsboro
        Me.MinimumSize = New System.Drawing.Size(100, 39)
        Me.Name = "Form1"
        Me.Opacity = 1.0R
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        Me.TransparencyKey = System.Drawing.Color.Fuchsia
        Me.windowstyle = WindowsApplication3.CustomWindow.style.VS
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents CustomButton1 As CustomButton
End Class
