Imports System.ComponentModel
Imports System.Drawing.Drawing2D

Class CustomTab
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
            For Each p As TabPage In TabPages
                p.BackColor = blendcol(_backcolor, rescol(_backcolor), 0.95)
            Next
        End Set
    End Property
    Public Property BackgroundColor As Color
        Get
            Return _backcolor
        End Get
        Set(value As Color)
            _backcolor = value
            refreshbitmap()
            For Each p As TabPage In TabPages
                p.BackColor = blendcol(_backcolor, rescol(_backcolor), 0.95)
            Next
        End Set
    End Property
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        For Each p As TabPage In TabPages
            p.BackColor = blendcol(_backcolor, rescol(_backcolor), 0.95)
            'Try
            '    p.BackColor = Color.Transparent
            'Catch ex As Exception
            'End Try
        Next
        ItemSize = New Size(25, 30)

    End Sub
    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Alignment = TabAlignment.Top
        For Each p As TabPage In TabPages
            p.BackColor = blendcol(_backcolor, rescol(_backcolor), 0.95)
            'Try
            '    p.BackColor = Color.Transparent
            'Catch ex As Exception
            'End Try
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
        bg.FillRectangle(mb(blendcol(_backcolor, rescol(_backcolor), 0.95)), 1, 34, Width - 3, Height - 4 - 34)
        bg.FillRectangle(mb(_backcolor), rct(1, 1, Width - 3, 34))
        bg.DrawLine(mp(col(60, 0)), 1, 33, Width - 3, 33)
        'bg.DrawLine(mp(col(30, 255)), 1, 33, Width - 3, 33)
        'bg.FillRectangle(mb(col(20, 255)), 0, 0, Width - 3, 33)
        bg.DrawLine(mp(col(60, 0)), 1, 1, Width - 3, 1)
        bg.Dispose()
        Invalidate()
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
    Protected Overrides Sub Dispose(disposing As Boolean)
        MyBase.Dispose(disposing)
        'animatedcontrols.Remove(Me)
    End Sub
End Class

