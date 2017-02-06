Public Class Form1
    Private Sub CustomButton1_Click(sender As Object, e As EventArgs)
    End Sub

    Private Sub Custom_Material_Button1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Custom_Material_Button1_Click_1(sender As Object, e As EventArgs) Handles Custom_Material_Button1.Click
        Dim u As New customColorPicker
        u.Show()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class