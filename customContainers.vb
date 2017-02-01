Imports System.ComponentModel
Imports System.Drawing.Drawing2D

Class GhostTabControlLagFree
    Inherits TabControl
    Private bb As Bitmap = New Bitmap(1, 1)
    Private _forecolor As Color = col(255, 82, 82)
    Private _backcolor As Color = Color.White
    Public Property HighlightColor As Color
        Get
            Return _forecolor
        End Get
        Set(value As Color)
            _forecolor = value
            refreshbitmap()
        End Set
    End Property
    Public Property BackgroundColor As Color
        Get
            Return _backcolor
        End Get
        Set(value As Color)
            _backcolor = value
            refreshbitmap()
        End Set
    End Property
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        For Each p As TabPage In TabPages
            Try
                p.BackColor = Color.Black
                p.BackColor = Color.Transparent
            Catch ex As Exception
            End Try
        Next
        ItemSize = New Size(25, 30)

    End Sub
    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Alignment = TabAlignment.Top
        For Each p As TabPage In TabPages
            Try
                p.BackColor = Color.Black
                p.BackColor = Color.Transparent
            Catch ex As Exception
            End Try
        Next
    End Sub
    Protected Overrides Sub OnSizeChanged(e As EventArgs)
        MyBase.OnSizeChanged(e)
        refreshbitmap()
    End Sub
    Sub refreshbitmap()
        bb = New Bitmap(Width, Height)
        Dim bg = Graphics.FromImage(bb)
        bg.Clear(col(0, 0))
        bg.SetClip(rct(0, 1, Width, Height))
        bg.DrawImage(DrawRoundRectangle1(rct(Me), 5, 4, _backcolor, col(100, 0), -0.5), 0, 0, Width, Height)
        'bg.FillRectangle(mb(blendcol(_backcolor, Color.GreenYellow)), 1, 34, Width - 3, Height - 4 - 34)
        bg.FillRectangle(mb(_backcolor), rct(1, 1, Width - 3, 34))
        bg.DrawLine(mp(col(60, 0)), 1, 34, Width - 3, 34)
        'bg.DrawLine(mp(col(30, 255)), 1, 33, Width - 3, 33)
        'bg.FillRectangle(mb(col(20, 255)), 0, 0, Width - 3, 33)
        bg.DrawLine(mp(col(60, 0)), 1, 1, Width - 3, 1)
        bg.Dispose()
        Invalidate()
        'MsgBox(blendcol(Color.GreenYellow, Color.Tan).R)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)

        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        G.Clear(Parent.BackColor)

        G.DrawImageUnscaled(bb, 0, 0)
        G.FillRectangle(mb(col(255, 82, 82)), rct(GetTabRect(SelectedIndex), -1, 29, 0, -27))

        For i = 0 To TabCount - 1
            Dim x2 As Rectangle = New Rectangle(GetTabRect(i).X, GetTabRect(i).Y + 2, GetTabRect(i).Width + 2, GetTabRect(i).Height - 1)
            If Not i = SelectedIndex Then
                G.DrawString(TabPages(i).Text, Font, mb(col(rescol(_backcolor), 0.7)), x2, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
            Else
                G.DrawString(TabPages(i).Text, Font, mb(invert(_forecolor)), x2, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
            End If
        Next

        e.Graphics.DrawImage(B.Clone, 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

Class GhostListBoxPretty
    Inherits customControl
    Public WithEvents LBox As New ListBox
    Private __Items As String() = {""}
    Public Property Items As String()
        Get
            Return __Items
            Invalidate()
        End Get
        Set(ByVal value As String())
            __Items = value
            LBox.Items.Clear()
            Invalidate()
            LBox.Items.AddRange(value)
            Invalidate()
        End Set
    End Property

    Public ReadOnly Property SelectedItem() As String
        Get
            Return LBox.SelectedItem
        End Get
    End Property

    Sub New()
        Controls.Add(LBox)
        Size = New Size(131, 101)

        LBox.BackColor = Color.FromArgb(0, 0, 0)
        LBox.BorderStyle = BorderStyle.None
        LBox.DrawMode = DrawMode.OwnerDrawVariable
        LBox.Location = New Point(3, 3)
        LBox.ForeColor = Color.White
        LBox.ItemHeight = 20
        LBox.Items.Clear()
        LBox.IntegralHeight = False
        Invalidate()
    End Sub

    Protected Overrides Sub OnResize(e As System.EventArgs)
        MyBase.OnResize(e)
        LBox.Width = Width - 4
        LBox.Height = Height - 4
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Color.Black)
        G.DrawRectangle(Pens.Black, 0, 0, Width - 2, Height - 2)
        G.DrawRectangle(New Pen(Color.FromArgb(90, 90, 90)), 1, 1, Width - 3, Height - 3)
        LBox.Size = New Size(Width - 5, Height - 5)
    End Sub
    Sub DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles LBox.DrawItem
        If e.Index < 0 Then Exit Sub
        e.DrawBackground()
        e.DrawFocusRectangle()
        If InStr(e.State.ToString, "Selected,") > 0 Then
            e.Graphics.FillRectangle(Brushes.Black, e.Bounds)
            Dim x2 As Rectangle = New Rectangle(e.Bounds.Location, New Size(e.Bounds.Width - 1, e.Bounds.Height))
            Dim x3 As Rectangle = New Rectangle(x2.Location, New Size(x2.Width, (x2.Height / 2) - 2))
            Dim G1 As New LinearGradientBrush(New Point(x2.X, x2.Y), New Point(x2.X, x2.Y + x2.Height), Color.FromArgb(60, 60, 60), Color.FromArgb(50, 50, 50))
            Dim H As New HatchBrush(HatchStyle.DarkDownwardDiagonal, Color.FromArgb(15, Color.Black), Color.Transparent)
            e.Graphics.FillRectangle(G1, x2) : G1.Dispose()
            e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(25, Color.White)), x3)
            e.Graphics.FillRectangle(H, x2) : G1.Dispose()
            e.Graphics.DrawString(" " & LBox.Items(e.Index).ToString(), Font, Brushes.White, e.Bounds.X, e.Bounds.Y + 2)
        Else
            e.Graphics.DrawString(" " & LBox.Items(e.Index).ToString(), Font, Brushes.White, e.Bounds.X, e.Bounds.Y + 2)
        End If
    End Sub
    Sub AddRange(ByVal Items As Object())
        LBox.Items.Remove("")
        LBox.Items.AddRange(Items)
        Invalidate()
    End Sub
    Sub AddItem(ByVal Item As Object)
        LBox.Items.Remove("")
        LBox.Items.Add(Item)
        Invalidate()
    End Sub
End Class



Class GhostComboBox
    Inherits ComboBox

    Private X As Integer
    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)
        DrawMode = DrawMode.OwnerDrawFixed
        ItemHeight = 20
        BackColor = Color.FromArgb(30, 30, 30)
        DropDownStyle = ComboBoxStyle.DropDownList
    End Sub

    Protected Overrides Sub OnMouseMove(e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        X = e.X
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(e As System.EventArgs)
        MyBase.OnMouseLeave(e)
        X = -1
        Invalidate()
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        If Not DropDownStyle = ComboBoxStyle.DropDownList Then DropDownStyle = ComboBoxStyle.DropDownList
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        G.Clear(Color.FromArgb(50, 50, 50))
        Dim GradientBrush As LinearGradientBrush = New LinearGradientBrush(New Rectangle(0, 0, Width, Height / 5 * 2), Color.FromArgb(20, 0, 0, 0), Color.FromArgb(15, Color.White), 90.0F)
        G.FillRectangle(GradientBrush, New Rectangle(0, 0, Width, Height / 5 * 2))
        Dim hatch As HatchBrush
        hatch = New HatchBrush(HatchStyle.DarkDownwardDiagonal, Color.FromArgb(20, Color.Black), Color.FromArgb(0, Color.Gray))
        G.FillRectangle(hatch, 0, 0, Width, Height)

        Dim S1 As Integer = G.MeasureString("...", Font).Height
        If SelectedIndex <> -1 Then
            G.DrawString(Items(SelectedIndex), Font, New SolidBrush(Color.White), 4, Height \ 2 - S1 \ 2)
        Else
            If Not Items Is Nothing And Items.Count > 0 Then
                G.DrawString(Items(0), Font, New SolidBrush(Color.White), 4, Height \ 2 - S1 \ 2)
            Else
                G.DrawString("...", Font, New SolidBrush(Color.White), 4, Height \ 2 - S1 \ 2)
            End If
        End If

        If MouseButtons = MouseButtons.None And X > Width - 25 Then
            G.FillRectangle(New SolidBrush(Color.FromArgb(7, Color.White)), Width - 25, 1, Width - 25, Height - 3)
        ElseIf MouseButtons = MouseButtons.None And X < Width - 25 And X >= 0 Then
            G.FillRectangle(New SolidBrush(Color.FromArgb(7, Color.White)), 2, 1, Width - 27, Height - 3)
        End If

        G.DrawRectangle(Pens.Black, 0, 0, Width - 1, Height - 1)
        G.DrawRectangle(New Pen(Color.FromArgb(90, 90, 90)), 1, 1, Width - 3, Height - 3)
        G.DrawLine(New Pen(Color.FromArgb(90, 90, 90)), Width - 25, 1, Width - 25, Height - 3)
        G.DrawLine(Pens.Black, Width - 24, 0, Width - 24, Height)
        G.DrawLine(New Pen(Color.FromArgb(90, 90, 90)), Width - 23, 1, Width - 23, Height - 3)

        G.FillPolygon(Brushes.Black, Triangle(New Point(Width - 14, Height \ 2), New Size(5, 3)))
        G.FillPolygon(Brushes.White, Triangle(New Point(Width - 15, Height \ 2 - 1), New Size(5, 3)))

        e.Graphics.DrawImage(B.Clone, 0, 0)
        G.Dispose() : B.Dispose()
    End Sub

    Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)
        If e.Index < 0 Then Exit Sub
        Dim rect As New Rectangle()
        rect.X = e.Bounds.X
        rect.Y = e.Bounds.Y
        rect.Width = e.Bounds.Width - 1
        rect.Height = e.Bounds.Height - 1

        e.DrawBackground()
        If e.State = 785 Or e.State = 17 Then
            e.Graphics.FillRectangle(Brushes.Black, e.Bounds)
            Dim x2 As Rectangle = New Rectangle(e.Bounds.Location, New Size(e.Bounds.Width - 1, e.Bounds.Height))
            Dim x3 As Rectangle = New Rectangle(x2.Location, New Size(x2.Width, (x2.Height / 2) - 1))
            Dim G1 As New LinearGradientBrush(New Point(x2.X, x2.Y), New Point(x2.X, x2.Y + x2.Height), Color.FromArgb(60, 60, 60), Color.FromArgb(50, 50, 50))
            Dim H As New HatchBrush(HatchStyle.DarkDownwardDiagonal, Color.FromArgb(15, Color.Black), Color.Transparent)
            e.Graphics.FillRectangle(G1, x2) : G1.Dispose()
            e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(25, Color.White)), x3)
            e.Graphics.FillRectangle(H, x2) : G1.Dispose()
            e.Graphics.DrawString(" " & Items(e.Index).ToString(), Font, Brushes.White, e.Bounds.X, e.Bounds.Y + 2)
        Else
            e.Graphics.FillRectangle(New SolidBrush(BackColor), e.Bounds)
            e.Graphics.DrawString(" " & Items(e.Index).ToString(), Font, Brushes.White, e.Bounds.X, e.Bounds.Y + 2)
        End If
        MyBase.OnDrawItem(e)
    End Sub

    Public Function Triangle(ByVal Location As Point, ByVal Size As Size) As Point()
        Dim ReturnPoints(0 To 3) As Point
        ReturnPoints(0) = Location
        ReturnPoints(1) = New Point(Location.X + Size.Width, Location.Y)
        ReturnPoints(2) = New Point(Location.X + Size.Width \ 2, Location.Y + Size.Height)
        ReturnPoints(3) = Location

        Return ReturnPoints
    End Function

    Private Sub GhostComboBox_DropDownClosed(sender As Object, e As System.EventArgs) Handles Me.DropDownClosed
        DropDownStyle = ComboBoxStyle.Simple
        Application.DoEvents()
        DropDownStyle = ComboBoxStyle.DropDownList
    End Sub

    Private Sub GhostCombo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.TextChanged
        Invalidate()
    End Sub
End Class
