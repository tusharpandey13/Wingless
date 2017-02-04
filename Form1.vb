Public Class Form1
    Private Sub CustomButton1_Click(sender As Object, e As EventArgs)
    End Sub

    Private Sub Custom_Material_Button1_Click(sender As Object, e As EventArgs) Handles Custom_Material_Button1.Click
        Dim u As New UserControl1
        Me.Controls.Add(u)
    End Sub
End Class