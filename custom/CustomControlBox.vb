Imports System.Drawing.Drawing2D


#Region "Custom ControlBox"
Class customcontrolbox

    Inherits customControl

#Region "Color Functions"
    Sub mp(c As Byte)
        p = New Pen(col(c))
    End Sub
    Sub mp(c As Color)
        p = New Pen(c)
    End Sub
    Sub mp(a As Byte, c As Color)
        p = New Pen(col(a, c))
    End Sub
    Function bwp(c As Byte) As Color
        Dim a As Color() = {col(c), BackColor}
        Return blend(a)
    End Function
    Sub px(c As Color, x!, y!, g As Graphics)
        br = New SolidBrush(c)
        g.FillRectangle(br, x, y, 1, 1)
        br.Dispose()
    End Sub
#End Region

#Region "Variables"
    Public l As Integer = 3
    'Public s As MouseState
    Dim c As Color
    Dim p As Pen
    Dim gb As LinearGradientBrush
    Dim minb, rb, mb, cb As New Bitmap(103, 33)
    Dim minbl, rbl, mbl, cbl As New Bitmap(103, 33)
    Dim br As SolidBrush
    Dim x! = -1000
#End Region

#Region "Settings"
    Sub New()
        SetStyle(ControlStyles.ResizeRedraw Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)
        DoubleBuffered = True
        Anchor = AnchorStyles.Top Or AnchorStyles.Right
        LockHeight = 33
        LockWidth = 103
    End Sub
#End Region

#Region "Draw the Bitmap Icons"

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        Dim g As Graphics



        g = Graphics.FromImage(minb)
        g.CompositingQuality = 2
        g.InterpolationMode = 7


        mp(70)
        g.DrawLine(p, 11, 20, 21, 20)
        mp(97)
        g.DrawLine(p, 11, 21, 21, 21)


        mp(col(70, Color.Black))
        g.DrawLine(p, 12, 22, 20, 22)
        mp(col(40, Color.Black))
        g.DrawLine(p, 12, 19, 20, 19)
        px(col(40, Color.Black), 10, 20, g)
        px(col(35, Color.Black), 10, 21, g)
        px(col(40, Color.Black), 22, 20, g)
        px(col(35, Color.Black), 22, 21, g)




        g = Graphics.FromImage(mb)
        g.CompositingQuality = 2
        g.InterpolationMode = 7


        g.SetClip(New Rectangle(48, 13, 7, 7), CombineMode.Exclude)
        gb = New LinearGradientBrush(New Rectangle(46, 12, 11, 11), col(85), col(100), 90.0F)
        g.FillRectangle(gb, New Rectangle(46, 11, 11, 11))
        mp(col(22, Color.Black))
        g.DrawRectangle(p, 46, 12, 10, 9)
        mp(col(3, Color.White))
        g.DrawLine(p, 46, 13, 56, 13)
        mp(60)
        g.DrawLine(p, 46, 11, 56, 11)
        mp(51)
        g.DrawLine(p, 48, 20, 54, 20)



        g.ResetClip()
        mp(col(50, Color.Black))
        g.DrawLine(p, 48, 13, 54, 13)
        mp(col(24, Color.Black))
        g.DrawLine(p, 48, 13, 48, 19)
        mp(col(24, Color.Black))
        g.DrawLine(p, 54, 13, 54, 19)
        mp(col(12, Color.Black))
        g.DrawLine(p, 48, 19, 54, 19)

        mp(col(16, Color.Black))
        g.DrawLine(p, 46, 10, 56, 10)
        mp(col(38, Color.Black))
        g.DrawLine(p, 46, 22, 56, 22)
        mp(col(24, Color.Black))
        g.DrawLine(p, 45, 11, 45, 21)
        mp(col(24, Color.Black))
        g.DrawLine(p, 57, 11, 57, 21)

        px(col(2, Color.Black), 45, 10, g)
        px(col(2, Color.Black), 57, 10, g)
        px(col(8, Color.Black), 45, 22, g)
        px(col(2, Color.Black), 57, 22, g)




        g = Graphics.FromImage(rb)
        g.CompositingQuality = 2
        g.InterpolationMode = 7

        mp(64)
        g.DrawLine(p, 46, 15, 48, 15)
        g.DrawLine(p, 50, 11, 56, 11)
        mp(56)
        g.DrawLine(p, 48, 20, 50, 20)
        g.DrawLine(p, 51, 19, 52, 19)
        g.DrawLine(p, 52, 16, 54, 16)

        gb = New LinearGradientBrush(New Rectangle(50, 12, 7, 6), col(105), col(89), 90.0F)
        g.SetClip(New Rectangle(52, 13, 3, 4), CombineMode.Exclude)
        g.FillRectangle(gb, gb.Rectangle)
        g.ResetClip()

        mp(105)
        g.DrawLine(p, 46, 16, 48, 16)
        mp(102)
        g.DrawLine(p, 46, 17, 47, 17)
        mp(99)
        g.DrawLine(p, 46, 18, 47, 18)
        mp(95)
        g.DrawLine(p, 46, 19, 47, 19)
        mp(92)
        g.DrawLine(p, 51, 20, 52, 20)
        g.DrawLine(p, 46, 20, 47, 20)
        mp(89)
        g.DrawLine(p, 46, 21, 52, 21)


        mp(col(50, Color.Black))
        px(col(50, Color.Black), 48, 17, g)
        g.DrawLine(p, 46, 22, 52, 22)
        g.DrawLine(p, 50, 18, 56, 18)
        g.DrawLine(p, 52, 13, 54, 13)




        Static im As New Bitmap(13, 13)
        g = Graphics.FromImage(cb)
        g.InterpolationMode = 7
        im = New Bitmap(Image.FromStream(New System.IO.MemoryStream(Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAA0AAAANCAYAAABy6+R8AAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwAAADsABataJCQAAABp0RVh0U29mdHdhcmUAUGFpbnQuTkVUIHYzLjUuMTAw9HKhAAAAxUlEQVQoU5WPQQqDMBREvZOiC1u1e8GiAau1YhEt3v8A00zgpzFSShcvPzPvBzQA8Df2UhQXPY4LguvNkecFuq4DpwgX31uxbRva9oYsy4wQmNnTS2clWdcXmkbhdDrriICTmb3skN0jsiwLlFJI09RMZn9nF4R5nlHXNTh9Rw5FkiSYpqeF2d/ZhTiOzT+M46gjAk5m9rJD7CWKIvNJw/DQ8bPAzJ5eOnOEYYiquqLv71a4sKfnHrMVZVma4huuP8jfIHgDP+3Ac95JZCsAAAAASUVORK5CYII="))))

        Static cab As New Bitmap(103 - 80, 33 - 10)
        g = Graphics.FromImage(cab)
        g.InterpolationMode = 7
        g.CompositingQuality = 2


        px(col(16, Color.Black), 2, 0, g)
        px(col(16, Color.Black), 1, 1, g)
        px(col(16, Color.Black), 9, 1, g)
        px(col(16, Color.Black), 10, 0, g)
        px(col(16, Color.Black), 11, 1, g)

        px(col(14, Color.Black), 4, 2, g)
        px(col(14, Color.Black), 5, 3, g)
        px(col(14, Color.Black), 6, 4, g)
        px(col(14, Color.Black), 7, 3, g)
        px(col(14, Color.Black), 8, 2, g)
        px(col(14, Color.Black), 3, 10, g)
        px(col(14, Color.Black), 9, 10, g)

        px(col(12, Color.Black), 2, 8, g)
        px(col(12, Color.Black), 1, 9, g)
        px(col(12, Color.Black), 0, 10, g)
        px(col(12, Color.Black), 10, 8, g)
        px(col(12, Color.Black), 11, 9, g)
        px(col(12, Color.Black), 12, 10, g)

        px(col(38, Color.Black), 1, 11, g)
        px(col(38, Color.Black), 2, 12, g)
        px(col(38, Color.Black), 3, 11, g)
        px(col(38, Color.Black), 9, 11, g)
        px(col(38, Color.Black), 10, 12, g)
        px(col(38, Color.Black), 11, 11, g)

        px(col(40, Color.Black), 6, 8, g)
        px(col(40, Color.Black), 5, 9, g)
        px(col(40, Color.Black), 4, 10, g)
        px(col(40, Color.Black), 7, 9, g)
        px(col(40, Color.Black), 8, 10, g)

        px(col(46, Color.Black), 0, 2, g)
        px(col(46, Color.Black), 1, 3, g)
        px(col(46, Color.Black), 12, 2, g)
        px(col(46, Color.Black), 11, 3, g)

        px(col(44, Color.Black), 2, 4, g)
        px(col(44, Color.Black), 3, 5, g)
        px(col(44, Color.Black), 10, 4, g)
        px(col(44, Color.Black), 9, 5, g)

        px(col(36, Color.Black), 4, 6, g)
        px(col(36, Color.Black), 8, 6, g)


        g = Graphics.FromImage(cb)
        g.CompositingQuality = 2
        g.InterpolationMode = 7
        g.DrawImageUnscaled(im, 80, 10)
        g.DrawImageUnscaled(cab, 80, 10)


        g = Graphics.FromImage(minbl)
        g.CompositingQuality = 2
        g.InterpolationMode = 7
        mp(70)
        g.DrawLine(p, 11, 20, 21, 20)
        mp(97)
        g.DrawLine(p, 11, 21, 21, 21)

        If rescol(BackColor) = Color.White Then
            mp(col(51, Color.Black))
            g.DrawLine(p, 12, 20, 20, 20)
        Else
            mp(col(70, Color.White))
            g.DrawLine(p, 12, 22, 20, 22)
            mp(col(40, Color.White))
            g.DrawLine(p, 12, 19, 20, 19)
            px(col(40, Color.White), 10, 20, g)
            px(col(35, Color.White), 10, 21, g)
            px(col(40, Color.White), 22, 20, g)
            px(col(35, Color.White), 22, 21, g)
        End If '                                                                     minimize





        g = Graphics.FromImage(mbl)
        g.CompositingQuality = 2
        g.InterpolationMode = 7
        g.SetClip(New Rectangle(48, 13, 7, 7), CombineMode.Exclude)
        gb = New LinearGradientBrush(New Rectangle(46, 12, 11, 11), col(85), col(100), 90.0F)
        g.FillRectangle(gb, New Rectangle(46, 11, 11, 11))
        mp(col(22, Color.Black))
        g.DrawRectangle(p, 46, 12, 10, 9)
        mp(col(3, Color.White))
        g.DrawLine(p, 46, 13, 56, 13)
        mp(60)
        g.DrawLine(p, 46, 11, 56, 11)
        mp(51)
        g.DrawLine(p, 48, 20, 54, 20)
        g.ResetClip()
        mp(col(50, Color.White))
        g.DrawLine(p, 48, 13, 54, 13)
        mp(col(24, Color.White))
        g.DrawLine(p, 48, 13, 48, 19)
        mp(col(24, Color.White))
        g.DrawLine(p, 54, 13, 54, 19)
        mp(col(12, Color.White))
        g.DrawLine(p, 48, 19, 54, 19)

        mp(col(16, Color.White))
        g.DrawLine(p, 46, 10, 56, 10)
        mp(col(38, Color.White))
        g.DrawLine(p, 46, 22, 56, 22)
        mp(col(24, Color.White))
        g.DrawLine(p, 45, 11, 45, 21)
        mp(col(24, Color.White))
        g.DrawLine(p, 57, 11, 57, 21)

        px(col(2, Color.White), 45, 10, g)
        px(col(2, Color.White), 57, 10, g)
        px(col(8, Color.White), 45, 22, g)
        px(col(2, Color.White), 57, 22, g) '                                                                            maximize                                             




        g = Graphics.FromImage(rbl)
        g.CompositingQuality = 2
        g.InterpolationMode = 7
        mp(64)
        g.DrawLine(p, 46, 15, 48, 15)
        g.DrawLine(p, 50, 11, 56, 11)
        mp(56)
        g.DrawLine(p, 48, 20, 50, 20)
        g.DrawLine(p, 51, 19, 52, 19)
        g.DrawLine(p, 52, 16, 54, 16)

        gb = New LinearGradientBrush(New Rectangle(50, 12, 7, 6), col(105), col(89), 90.0F)
        g.SetClip(New Rectangle(52, 13, 3, 4), CombineMode.Exclude)
        g.FillRectangle(gb, gb.Rectangle)
        g.ResetClip()

        mp(105)
        g.DrawLine(p, 46, 16, 48, 16)
        mp(102)
        g.DrawLine(p, 46, 17, 47, 17)
        mp(99)
        g.DrawLine(p, 46, 18, 47, 18)
        mp(95)
        g.DrawLine(p, 46, 19, 47, 19)
        mp(92)
        g.DrawLine(p, 51, 20, 52, 20)
        g.DrawLine(p, 46, 20, 47, 20)
        mp(89)
        g.DrawLine(p, 46, 21, 52, 21)

        mp(col(50, Color.White))
        px(col(50, Color.White), 48, 17, g)
        g.DrawLine(p, 46, 22, 52, 22)
        g.DrawLine(p, 50, 18, 56, 18)
        g.DrawLine(p, 52, 13, 54, 13) '                                                                      restore






        Static cabl As New Bitmap(103 - 80, 33 - 10)
        g = Graphics.FromImage(cabl)
        g.InterpolationMode = 7
        g.CompositingQuality = 2

        px(col(16, Color.White), 2, 0, g)
        px(col(16, Color.White), 1, 1, g)
        px(col(16, Color.White), 9, 1, g)
        px(col(16, Color.White), 10, 0, g)
        px(col(16, Color.White), 11, 1, g)

        px(col(14, Color.White), 4, 2, g)
        px(col(14, Color.White), 5, 3, g)
        px(col(14, Color.White), 6, 4, g)
        px(col(14, Color.White), 7, 3, g)
        px(col(14, Color.White), 8, 2, g)
        px(col(14, Color.White), 3, 10, g)
        px(col(14, Color.White), 9, 10, g)

        px(col(12, Color.White), 2, 8, g)
        px(col(12, Color.White), 1, 9, g)
        px(col(12, Color.White), 0, 10, g)
        px(col(12, Color.White), 10, 8, g)
        px(col(12, Color.White), 11, 9, g)
        px(col(12, Color.White), 12, 10, g)

        px(col(38, Color.White), 1, 11, g)
        px(col(38, Color.White), 2, 12, g)
        px(col(38, Color.White), 3, 11, g)
        px(col(38, Color.White), 9, 11, g)
        px(col(38, Color.White), 10, 12, g)
        px(col(38, Color.White), 11, 11, g)

        px(col(40, Color.White), 6, 8, g)
        px(col(40, Color.White), 5, 9, g)
        px(col(40, Color.White), 4, 10, g)
        px(col(40, Color.White), 7, 9, g)
        px(col(40, Color.White), 8, 10, g)

        px(col(46, Color.White), 0, 2, g)
        px(col(46, Color.White), 1, 3, g)
        px(col(46, Color.White), 12, 2, g)
        px(col(46, Color.White), 11, 3, g)

        px(col(44, Color.White), 2, 4, g)
        px(col(44, Color.White), 3, 5, g)
        px(col(44, Color.White), 10, 4, g)
        px(col(44, Color.White), 9, 5, g)

        px(col(36, Color.White), 4, 6, g)
        px(col(36, Color.White), 8, 6, g)


        g = Graphics.FromImage(cbl)
        g.CompositingQuality = 2
        g.InterpolationMode = 7
        g.DrawImageUnscaled(im, 80, 10)
        g.DrawImageUnscaled(cabl, 80, 10) '                                                                   close





    End Sub

#End Region



    Protected Overrides Sub PaintHook()

        G.Clear(BackColor)
        G.SmoothingMode = 0
        G.CompositingQuality = 4
        G.InterpolationMode = 7

        Select Case State
            Case MouseState.Over
                'MsgBox("fe")
                G.FillRectangle(New SolidBrush(col(25, rescol(Parent.ForeColor))), x, 0, 34, Height)
            Case MouseState.Down
                G.FillRectangle(New SolidBrush(col(35, ForeColor)), x, 0, 34, Height)
        End Select

        If rescol(BackColor).R = 255 Then 'dark
            'Minimize;
            G.DrawImageUnscaled(minb, 0, 0)
            'Maximize and Restore;
            If Parent.FindForm.WindowState = FormWindowState.Normal Then G.DrawImageUnscaled(mb, 0, 0) Else G.DrawImageUnscaled(rb, 0, 0)

            'Close;
            G.DrawImageUnscaled(cb, 0, 0)
        Else
            'Minimize;
            G.DrawImageUnscaled(minbl, 0, 0)
            'Maximize and Restore;
            If Parent.FindForm.WindowState = FormWindowState.Normal Then G.DrawImageUnscaled(mbl, 0, 0) Else G.DrawImageUnscaled(rbl, 0, 0)

            'Close;
            G.DrawImageUnscaled(cbl, 0, 0)
        End If

        'Get rid of the Pen and GradientBrush;
        p.Dispose()
        gb.Dispose()

    End Sub


#Region "Calculate Mouse State"

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        State = MouseState.Over
        If e.X > 0 And e.X < 34 Then
            l = 0
            x = 0
        ElseIf e.X > 34 And e.X < 68 Then
            l = 1
            x = 35
        ElseIf e.X > 68 Then
            l = 2
            x = 69
        Else
            l = 3
            x = -1000
        End If
        Invalidate()
        'Parent.Invalidate()
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        l = 3
        State = MouseState.None
        Invalidate()
        'Parent.Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        l = 3
        State = MouseState.None
        Invalidate()
        'Parent.Invalidate()
    End Sub

#End Region

#Region "Button Click Actions"

    Protected Overrides Sub OnClick(e As EventArgs)
        MyBase.OnClick(e)
        State = MouseState.Down
        Select Case l
            Case 0
                FindForm.WindowState = FormWindowState.Minimized
            Case 1
                If FindForm.WindowState = FormWindowState.Normal Then
                    FindForm.WindowState = FormWindowState.Maximized
                ElseIf FindForm.WindowState = FormWindowState.Maximized Then
                    FindForm.WindowState = FormWindowState.Normal
                End If
            Case 2
                FindForm.Close()
            Case Else
        End Select
    End Sub

#End Region

End Class
#End Region





















'G.DrawLine(New Pen(ntext, 2), 10, 15, 19, 15)

'If FindForm.WindowState = FormWindowState.Normal Then
'    G.DrawLine(New Pen(ntext, 3), 38, 10, 48, 10)
'    G.DrawLine(New Pen(ntext, 1), 38, 10, 38, 18)
'    G.DrawLine(New Pen(ntext, 1), 47, 10, 47, 18)
'    G.DrawLine(New Pen(ntext, 1), 38, 18, 47, 18)
'ElseIf FindForm.WindowState = FormWindowState.Maximized Then

'    G.DrawLine(New Pen(ntext, 2), 40, 10, 48, 10)
'    G.DrawLine(New Pen(ntext, 1), 40, 10, 40, 16)
'    G.DrawLine(New Pen(ntext, 1), 47, 10, 47, 16)
'    G.DrawLine(New Pen(ntext, 1), 40, 16, 47, 16)

'    G.DrawLine(New Pen(ntext, 2), 38, 13, 46, 13)
'    G.DrawLine(New Pen(ntext, 1), 38, 13, 38, 18)
'    G.DrawLine(New Pen(ntext, 1), 45, 13, 45, 18)
'    G.DrawLine(New Pen(ntext, 1), 38, 18, 45, 18)

'    G.FillRectangle(New SolidBrush(Parent.ForeColor), new rectangle(39, 14, 6, 4))
'End If

'G.DrawLine(New Pen(ntext, 2), 65, 9, 74, 18)
'G.DrawLine(New Pen(ntext, 2), 65, 18, 74, 9)
'G.FillRectangle(New SolidBrush(Parent.ForeColor), 65, 9, 1, 1)
'G.FillRectangle(New SolidBrush(Parent.ForeColor), 74, 18, 1, 1)
'G.FillRectangle(New SolidBrush(Parent.ForeColor), 65, 18, 1, 1)
'G.FillRectangle(New SolidBrush(Parent.ForeColor), 74, 9, 1, 1)


'Select Case State
'    Case MouseState.Over
'        Select Case l
'            Case 0
'                G.FillRectangle(New SolidBrush(backh), 0, 0, 28, Height)
'                G.DrawLine(New Pen(htext, 2), 10, 15, 19, 15)

'            Case 1
'                G.FillRectangle(New SolidBrush(backh), 29, 0, 28, Height)
'                If FindForm.WindowState = FormWindowState.Normal Then
'                    G.DrawLine(New Pen(htext, 3), 38, 10, 48, 10)
'                    G.DrawLine(New Pen(htext, 1), 38, 10, 38, 18)
'                    G.DrawLine(New Pen(htext, 1), 47, 10, 47, 18)
'                    G.DrawLine(New Pen(htext, 1), 38, 18, 47, 18)
'                ElseIf FindForm.WindowState = FormWindowState.Maximized Then

'                    G.DrawLine(New Pen(htext, 2), 40, 10, 48, 10)
'                    G.DrawLine(New Pen(htext, 1), 40, 10, 40, 16)
'                    G.DrawLine(New Pen(htext, 1), 47, 10, 47, 16)
'                    G.DrawLine(New Pen(htext, 1), 40, 16, 47, 16)

'                    G.DrawLine(New Pen(htext, 2), 38, 13, 46, 13)
'                    G.DrawLine(New Pen(htext, 1), 38, 13, 38, 18)
'                    G.DrawLine(New Pen(htext, 1), 45, 13, 45, 18)
'                    G.DrawLine(New Pen(htext, 1), 38, 18, 45, 18)

'                    G.FillRectangle(New SolidBrush(backh), new rectangle(39, 14, 6, 4))
'                End If
'            Case 2
'                G.FillRectangle(New SolidBrush(backh), 56, 0, 28, Height)
'                G.DrawLine(New Pen(htext, 2), 65, 9, 74, 18)
'                G.DrawLine(New Pen(htext, 2), 65, 18, 74, 9)
'                G.FillRectangle(New SolidBrush(backh), 65, 9, 1, 1)
'                G.FillRectangle(New SolidBrush(backh), 74, 18, 1, 1)
'                G.FillRectangle(New SolidBrush(backh), 65, 18, 1, 1)
'                G.FillRectangle(New SolidBrush(backh), 74, 9, 1, 1)

'            Case Else

'        End Select

'    Case MouseState.Down
'        Select Case l
'            Case 0
'                G.FillRectangle(New SolidBrush(backd), 0, 0, 28, Height)
'                G.DrawLine(New Pen(dtext, 2), 10, 15, 19, 15)

'            Case 1
'                G.FillRectangle(New SolidBrush(backd), 29, 0, 28, Height)
'                If FindForm.WindowState = FormWindowState.Normal Then
'                    G.DrawLine(New Pen(dtext, 3), 38, 10, 48, 10)
'                    G.DrawLine(New Pen(dtext, 1), 38, 10, 38, 18)
'                    G.DrawLine(New Pen(dtext, 1), 47, 10, 47, 18)
'                    G.DrawLine(New Pen(dtext, 1), 38, 18, 47, 18)
'                ElseIf FindForm.WindowState = FormWindowState.Maximized Then

'                    G.DrawLine(New Pen(dtext, 2), 40, 10, 48, 10)
'                    G.DrawLine(New Pen(dtext, 1), 40, 10, 40, 16)
'                    G.DrawLine(New Pen(dtext, 1), 47, 10, 47, 16)
'                    G.DrawLine(New Pen(dtext, 1), 40, 16, 47, 16)

'                    G.DrawLine(New Pen(dtext, 2), 38, 13, 46, 13)
'                    G.DrawLine(New Pen(dtext, 1), 38, 13, 38, 18)
'                    G.DrawLine(New Pen(dtext, 1), 45, 13, 45, 18)
'                    G.DrawLine(New Pen(dtext, 1), 38, 18, 45, 18)

'                    G.FillRectangle(New SolidBrush(backd), new rectangle(39, 14, 6, 4))
'                End If
'            Case 2
'                G.FillRectangle(New SolidBrush(backd), 56, 0, 29, Height)
'                G.DrawLine(New Pen(dtext, 2), 65, 9, 74, 18)
'                G.DrawLine(New Pen(dtext, 2), 65, 18, 74, 9)
'                G.FillRectangle(New SolidBrush(backd), 65, 9, 1, 1)
'                G.FillRectangle(New SolidBrush(backd), 74, 18, 1, 1)
'                G.FillRectangle(New SolidBrush(backd), 65, 18, 1, 1)
'                G.FillRectangle(New SolidBrush(backd), 74, 9, 1, 1)

'            Case Else

'        End Select

'End Select'old











