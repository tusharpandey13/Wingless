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
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.CustomTab1.SuspendLayout()
        Me.SuspendLayout()
        '
        'CustomTab1
        '
        Me.CustomTab1.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(250, Byte), Integer), CType(CType(250, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.CustomTab1.Controls.Add(Me.TabPage1)
        Me.CustomTab1.Controls.Add(Me.TabPage2)
        Me.CustomTab1.HighlightColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(82, Byte), Integer), CType(CType(82, Byte), Integer))
        Me.CustomTab1.ItemSize = New System.Drawing.Size(25, 30)
        Me.CustomTab1.Location = New System.Drawing.Point(93, 56)
        Me.CustomTab1.Name = "CustomTab1"
        Me.CustomTab1.SelectedIndex = 0
        Me.CustomTab1.Size = New System.Drawing.Size(473, 358)
        Me.CustomTab1.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(237, Byte), Integer))
        Me.TabPage1.Location = New System.Drawing.Point(4, 34)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(465, 320)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "TabPage1"
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(237, Byte), Integer))
        Me.TabPage2.Location = New System.Drawing.Point(4, 34)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(465, 320)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "TabPage2"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(688, 449)
        Me.Controls.Add(Me.CustomTab1)
        Me.ForeColor = System.Drawing.Color.Gainsboro
        Me.MinimumSize = New System.Drawing.Size(100, 39)
        Me.Name = "Form1"
        Me.Opacity = 1.0R
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        Me.TransparencyKey = System.Drawing.Color.Fuchsia
        Me.CustomTab1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents CustomTab1 As CustomTab
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents TabPage2 As TabPage
End Class
