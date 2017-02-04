Imports System.Drawing.Drawing2D
Public Class UserControl1

    Private Class map : Inherits customControl
        Dim mapy! = 0
        Dim mb As Bitmap
        Dim isd% = 0
        Public Event colorchanged()
        Sub New()
            Width = 20 : Height = 200
            Left = 200 : Top = -1
        End Sub
        Protected Overrides Sub PaintHook()
            G.DrawImage(mb, 0, 0, 20, 200)
            mp(invert(hsltorgb(mapy / 200 * 360, 1, 1)), tp)
            G.DrawLine(tp, 0, mapy, 20, mapy)
        End Sub
        Protected Overrides Sub OnCreateControl()
            mb = New Bitmap(20, 360)
            MyBase.OnCreateControl()
            With Graphics.FromImage(mb)
                .Clear(Color.Transparent)
                For i = 0 To 360
                    mp(hsltorgb(i, 1, 0.5), tp)
                    .DrawLine(tp, 0, i, 20, i)
                Next
            End With
        End Sub
        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            MyBase.OnMouseDown(e)
            isd = 1
        End Sub
        Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
            MyBase.OnMouseMove(e)
            If isd = 1 And e.Y <= 200 And e.Y >= 0 And e.X <= 20 And e.X >= 0 Then
                mapy = e.Y
                Invalidate()
                RaiseEvent colorchanged()
            End If
        End Sub
        Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
            MyBase.OnMouseUp(e)
            isd = 0
        End Sub
        Public Function gethue!()
            Return mapy / 200 * 360
        End Function
    End Class : Dim map1 As New map()
    Private Class grid : Inherits customControl
        Sub New()
            Width = 200 : Height = 200
            Top = 0 : Left = 0
            draw()
        End Sub
        Dim _hue! = 40 : Public Property hue!
            Get
                Return _hue
            End Get
            Set(value!)
                If Not _hue = value Then
                    _hue = value
                    draw()
                    Invalidate()
                End If
            End Set
        End Property
        Dim tbt As New Bitmap(200, 200)
        Dim x!, y!
        Dim isd% = 0
        Public Event colorchanged()
        Protected Overrides Sub PaintHook()
            G.SmoothingMode = 2 : G.InterpolationMode = 7
            G.Clear(Color.Black)
            G.DrawImageUnscaled(tbt, -1, -1)
            mp(col(90, 255), tp)
            G.DrawRectangle(tp, -2, -2, x + 1, y + 1)
            G.DrawRectangle(tp, x + 1, -1, 201 - x, y)
            G.DrawRectangle(tp, -2, y + 1, x + 1, 201 - y)
            G.DrawRectangle(tp, x + 1, y + 1, 201 - x, 201 - y)
            mp(col(170, 0), tp)
            G.DrawLine(tp, 0, y, x - 1, y)
            G.DrawLine(tp, x + 1, y, 199, y)
            G.DrawLine(tp, x, 0, x, y - 1)
            G.DrawLine(tp, x, y + 1, x, 199)
        End Sub
        Sub draw()
            'Dim l1 As New LinearGradientBrush(rct(0, 0, 200, 200), col(255), col(0, 0), 90)
            Dim l2 As New LinearGradientBrush(rct(0, 0, 200, 200), col(0, 0), hsvtorgb(_hue, 1, 0.5), 0)
            Dim l3 As New LinearGradientBrush(rct(0, 0, 200, 200), col(0, 0), col(0), 90)
            With Graphics.FromImage(tbt)
                .SmoothingMode = 2 : .InterpolationMode = 7
                .Clear(Color.White)
                '.FillRectangle(l1, rct(0, 0, 200, 200))
                .FillRectangle(l2, rct(0, 0, 200, 200))
                .FillRectangle(l3, rct(0, 0, 200, 200))
            End With
            l3.Dispose() : l2.Dispose()
        End Sub
        Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
            MyBase.OnMouseMove(e)
            If isd Then
                x = e.X : y = e.Y
                Invalidate()
                RaiseEvent colorchanged()
            End If
        End Sub
        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            MyBase.OnMouseDown(e)
            isd = 1
        End Sub
        Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
            MyBase.OnMouseUp(e)
            isd = 0
        End Sub
        Public Function gethsv() As Single()
            Dim hsv(3) As Single
            hsv(0) = _hue
            hsv(1) = Math.Max(0, Math.Min(200, x + 1)) / 200
            hsv(2) = (200 - Math.Max(0, Math.Min(200, y + 1))) / 200
            Return hsv
        End Function
        Public Function getrgb() As Color
            Return hsvtorgb(gethsv(0), gethsv(1), gethsv(2))
        End Function
    End Class : Dim grid1 As New grid()
    Private Class alpha : Inherits customControl
        Dim ay! = 0
        Dim ab As Bitmap
        Dim isd% = 0
        Private _c As Color : Public Property c As Color
            Set(value As Color)
                _c = value
                Invalidate()
            End Set
            Get
                Return _c
            End Get
        End Property
        Public Event colorchanged()

        Sub New()
            Width = 20 : Height = 200
            Left = 221 : Top = -1
        End Sub
        Protected Overrides Sub PaintHook()
            G.DrawImageUnscaled(ab, 0, 0)
            Dim l As New LinearGradientBrush(rct(Me), col(255, _c), col(0, 0), 90)
            G.FillRectangle(l, rct(Me))
            G.DrawLine(Pens.White, 0, ay - 1, 20, ay - 1)
            G.DrawLine(Pens.Black, 0, ay, 20, ay)
            G.DrawLine(Pens.White, 0, ay + 1, 20, ay + 1)
        End Sub
        Protected Overrides Sub OnCreateControl()
            ab = New Bitmap(20, 200)
            MyBase.OnCreateControl()
            mb(col(50, 255), tb)
            With Graphics.FromImage(ab)
                .Clear(col(28, 28, 28))
                For i = 0 To 10
                    .FillRectangle(tb, rct(0, 20 * i, 10, 10))
                    .FillRectangle(tb, rct(10, 20 * i + 10, 10, 10))
                Next
            End With
        End Sub
        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            MyBase.OnMouseDown(e)
            isd = 1
        End Sub
        Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
            MyBase.OnMouseMove(e)
            If isd = 1 And e.Y <= 200 And e.Y >= 0 And e.X <= 20 And e.X >= 0 Then
                ay = e.Y
                Invalidate()
                RaiseEvent colorchanged()
            End If
        End Sub
        Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
            MyBase.OnMouseUp(e)
            isd = 0
        End Sub
        Public Function getalpha!()
            Return Math.Min(255, Math.Max(0, 200 - ay)) / 200
        End Function
    End Class : Dim alpha1 As New alpha()
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()        ' Add any initialization after the InitializeComponent() call.


        Me.Controls.Add(map1)
        Me.Controls.Add(grid1)
        Me.Controls.Add(alpha1)

        AddHandler map1.colorchanged, AddressOf colchanged
        AddHandler grid1.colorchanged, AddressOf colchanged
        AddHandler alpha1.colorchanged, AddressOf colchanged

    End Sub
    Private Sub UserControl1_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        grid1.hue = map1.gethue
        grid1.Invalidate()
    End Sub
    Sub colchanged()
        grid1.hue = map1.gethue
        alpha1.c = grid1.getrgb

    End Sub
End Class
