Imports System.ComponentModel
Imports System.Drawing.Drawing2D


#Region "Components"
<ToolboxItem(False)> Class picker : Inherits customControl
#Region "DECLARE"
    Public Property DontChange As Boolean = False
    Private hp() As Byte = {0, 0}
    Private tbt() As Bitmap = {New Bitmap(20, 360), New Bitmap(20, 200)}
    Private ia() As Byte = {1, 1, 1}
    Public Event ColorChanged()
    Private _h! : Public Property h!
        Get
            Return clamp(_y2, 0, 200) * 1.8
        End Get
        Set(value!)
            If DontChange Then Return
            _h = clamp(value, 0, 360)
            _y2 = _h / 360 * 200
            ia(0) = 1
            ia(1) = 1
            Invalidate()
        End Set
    End Property
    Private _s! : Public Property s!
        Get
            Return clamp(_x, 0, 200) / 200
        End Get
        Set(value!)
            If DontChange Then Return
            _s = clamp(value, 0, 1)
            _x = clamp(_s * 200, 0, 200)
            ia(0) = 1
            Invalidate()
        End Set
    End Property
    Private _v! : Public Property v!
        Get
            Return clamp((200 - _y1), 0, 200) / 200
        End Get
        Set(value!)
            If DontChange Then Return
            _v = clamp(value, 0, 1)
            _y1 = clamp(200 - _v * 200, 0, 200)
            ia(0) = 1
            Invalidate()
        End Set
    End Property
    Private _a! : Public Property a!
        Get
            Return clamp(_y3, 0, 200) / 200 * 255
        End Get
        Set(value!)
            If DontChange Then Return
            _a = clamp(value, 0, 255)
            _y3 = clamp(_a / 255 * 200, 0, 200)
            ia(2) = 1
            Invalidate()
        End Set
    End Property
    Private _x!, _y1!, _y2!, _y3!
#End Region
#Region "CTOR"
    Sub New(h!, s!, v!, a!)
        Me.h = h
        Me.s = s
        Me.v = v
        Me.a = a
        Left = 1 : Top = 1
        Width = 250 : Height = 200
    End Sub
#End Region
#Region "PAINT"
    Protected Overrides Sub PaintHook()

        G.Clear(col(51))

        If ia(0) Then
            G.SetClip(rct(0, 0, 200, 200))
            G.Clear(Color.White)
            'Debug.Write(h)
            Dim l1 As New LinearGradientBrush(rct(0, 0, 200, 200), col(0, 0), hsvtorgb(h, 1, 1), 0)
            Dim l2 As New LinearGradientBrush(rct(l1), col(0, 0), col(0), 90)
            G.FillRectangle(l1, rct(l1))
            G.FillRectangle(l2, rct(l2))

            G.SmoothingMode = 2

            mp(col(150, invert(rescol(hsvtorgb(360 - h, 0, 1 - v)))), tp)
            G.SetClip(rct(_x - 1, _y1 - 1, 3, 3), CombineMode.Exclude)
            G.DrawLine(tp, 0, _y1, 200, _y1)
            G.DrawLine(tp, _x, 0, _x, 200)
            G.ResetClip()
            G.DrawRectangle(tp, rct(_x - 2, _y1 - 2, 4, 4))
        End If 'map


        If Not hp(0) Then
            With Graphics.FromImage(tbt(0))
                .Clear(Color.Transparent)
                For i = 0 To 359
                    mp(hsvtorgb(i, 1, 1), tp)
                    .DrawLine(tp, 0, i, 20, i)
                Next
            End With
            hp(0) = 1
        End If

        If ia(1) Then
            G.SetClip(rct(211, 0, 20, 200))
            G.Clear(Color.White)
            G.DrawImage(tbt(0), 211, 0, 20, 200)
            mp(Color.Black, tp)
            G.SmoothingMode = 2
            G.DrawLine(tp, 211, _y2, 231, _y2)
            mb(Color.Black, tb)
            G.FillPolygon(tb, {pt(210, _y2 - 5), pt(210 + 5, _y2), pt(210, _y2)})
            G.FillPolygon(tb, {pt(210, _y2 + 5), pt(210 + 5, _y2), pt(210, _y2)})
            G.ResetClip()
        End If 'hue


        If ia(2) Then
            G.SetClip(rct(231, 0, 20, 200))
            G.Clear(col(51))
            For i = 0 To 9
                mb(col(50, 255), tb)
                G.FillRectangle(tb, rct(231, i * 20, 10, 10))
                G.FillRectangle(tb, rct(241, i * 20 + 10, 10, 10))
            Next
            Dim l As New LinearGradientBrush(rct(231, 0, 20, 200), col(0, 0), hsvtorgb(h, s, v), 90)
            G.SmoothingMode = 2
            G.FillRectangle(l, rct(l))
            mp(invert(hsvtorgb(h, s, v)), tp)
            G.DrawLine(tp, 231, _y3, 251, _y3)
            mb(invert(rescol(hsvtorgb(360 - h, 0, 1 - v))), tb)
            G.FillPolygon(tb, {pt(230, _y3 - 5), pt(230 + 5, _y3), pt(230, _y3)})
            G.FillPolygon(tb, {pt(230, _y3 + 5), pt(230 + 5, _y3), pt(230, _y3)})
            G.ResetClip()
        End If 'alpha


    End Sub
#End Region
#Region "INPUT"
    Protected Overrides Sub mousedownmove(e As MouseEventArgs)
        MyBase.mousedownmove(e)
        If Not (e.Y > -1 Or e.Y < 211) Then Return
        'ia(0) = 0 : ia(1) = 0 : ia(2) = 0
        If e.X > -1 And e.X < 211 Then
            _x = e.X
            _y1 = e.Y
            ia(0) = 1
            Invalidate(rct(-1, -1, 211, 211))
            Invalidate(rct(231, 0, 20, 200))
        ElseIf e.X > 200 And e.X < 231 Then
            _y2 = e.Y
            ia(1) = 1
            Invalidate(rct(-1, 0, 232, 200))
        ElseIf e.X > 230 And e.X < 251 Then
            _y3 = e.Y
            ia(2) = 1
            Invalidate(rct(231, 0, 20, 200))
        End If
        RaiseEvent ColorChanged()
    End Sub
#End Region
End Class
#End Region



Class customColorPicker : Inherits CustomWindow
#Region "DECLARE"
    Dim dc% = 0
    Dim p As New picker(0, 0, 0, 0)
    Dim t(4) As NumericUpDown
    Dim l As Label
    Dim pr As New Panel With {.Top = 200, .Left = 1, .Width = 250, .Height = 45}
    Public Property Color As Color
#End Region

    Sub New()
        custompaint = False


        Controls.Add(p)
        AddHandler p.ColorChanged, AddressOf ColorChangedbypicker

        Dim s() As Char = {"R", "G", "B", "A"}

        For i = 1 To 4
            t(i - 1) = New NumericUpDown With {.TabIndex = i - 1, .Left = 14 * i + 45 * (i - 1), .Top = 250 + 20, .BorderStyle = BorderStyle.FixedSingle,
                                        .BackColor = col(51), .ForeColor = col(190), .Width = 45, .Font = New Font("Consolas", 10), .Minimum = 0, .Maximum = 255}
            Controls.Add(t(i - 1))
            AddHandler t(i - 1).ValueChanged, AddressOf ColorChangedbytext
        Next

        l = New Label With {.Left = 14, .Top = 250, .ForeColor = col(190), .Font = New Font("Consolas", 10), .Text = "  R      G       B      A", .Width = 250}
        Controls.Add(l)


        Controls.Add(pr)
        AddHandler pr.Click, AddressOf bye
    End Sub

    Sub ColorChangedbypicker()
        dc = 1
        p.DontChange = True
        Color = col(p.a, hsvtorgb(p.h, p.s, p.v))
        pr.BackColor = col(255, Color)
        t(0).Value = Color.R
        t(1).Value = Color.G
        t(2).Value = Color.B
        t(3).Value = Color.A
        p.DontChange = False
        dc = 0
    End Sub
    Sub ColorChangedbytext()
        If dc = 1 Then Return
        If dc = 0 Then p.DontChange = False
        Color = col(t(3).Value, t(0).Value, t(1).Value, t(2).Value)
        pr.BackColor = col(255, Color)
        Dim hsv() As Single = rgbtohsv(Color.R, Color.G, Color.B)
        p.h = hsv(0)
        p.s = hsv(1)
        p.v = hsv(2)
        p.a = Color.A
    End Sub

    Sub bye()
        Close()
    End Sub
#Region "Other"
    Protected Overrides Sub create()
        MyBase.create()
        BackColor = col(51)
        SetWindowPos(Handle, vbNull, Cursor.Position.X - 240, Cursor.Position.Y - 240, 252, 302, SetWindowPosFlags.FrameChanged)
    End Sub
#End Region
End Class
