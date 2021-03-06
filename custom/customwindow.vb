﻿Imports System.Runtime.InteropServices
Imports System.Drawing.Drawing2D


Public Class CustomWindow : Inherits Form : Implements AnimatedObject


#Region "DECLARE"
    Private Const BorderWidth As Integer = 6
    Dim tp As Pen = Pens.Black : Dim tb As SolidBrush = Brushes.Black : Dim tgb As LinearGradientBrush
    Dim lb(4), db(4) As Bitmap
    Dim ot% = 0
    Dim fxt% = 0
    Dim cbx% = 0
    Dim ms% = 0
    Public Property animating As Boolean = 0 Implements AnimatedObject.animating
    Public ReadOnly Property designing As Boolean Implements AnimatedObject.designing
        Get
            Return DesignMode
        End Get
    End Property
#End Region

#Region "DWM"


#Region "Consts"
    Private Const WM_NCLBUTTONDOWN As Integer = &HA1
    Private HTNOWHERE As Integer = 0
    Private HTCLIENT As Integer = 1
    Private HTCAPTION As Integer = 2
    Private HTGROWBOX As Integer = 4
    Private HTSIZE As Integer = HTGROWBOX
    Private HTMINBUTTON As Integer = 8
    Private HTMAXBUTTON As Integer = 9
    Private HTLEFT As Integer = 10
    Private HTRIGHT As Integer = 11
    Private HTTOP As Integer = 12
    Private HTTOPLEFT As Integer = 13
    Private HTTOPRIGHT As Integer = 14
    Private HTBOTTOM As Integer = 15
    Private HTBOTTOMLEFT As Integer = 16
    Private HTBOTTOMRIGHT As Integer = 17
    Private HTREDUCE As Integer = HTMINBUTTON
    Private HTZOOM As Integer = HTMAXBUTTON
    Private HTSIZEFIRST As Integer = HTLEFT
    Private HTSIZELAST As Integer = HTBOTTOMRIGHT
    Dim WM_NCCALCSIZE As Integer = &H83
    Dim WM_NCHITTEST As Integer = &H84
#End Region
#Region "Fields"
    <StructLayout(LayoutKind.Sequential)>
    Public Structure MARGINS
        Public cxLeftWidth As Integer
        Public cxRightWidth As Integer
        Public cyTopHeight As Integer
        Public cyBottomHeight As Integer
        Public Sub New(ByVal Left As Integer, ByVal Right As Integer, ByVal Top As Integer, ByVal Bottom As Integer)
            Me.cxLeftWidth = Left
            Me.cxRightWidth = Right
            Me.cyTopHeight = Top
            Me.cyBottomHeight = Bottom
        End Sub
    End Structure

    Private dwmMargins As MARGINS
    Private _marginOk As Boolean
#End Region
#Region "Methods"
    Public Shared Function LoWord(ByVal dwValue As Integer) As Integer
        Return dwValue And &HFFFF
    End Function
    ''' <summary>
    ''' Equivalent to the HiWord C Macro
    ''' </summary>
    ''' <param name="dwValue"></param>
    ''' <returns></returns>
    Public Shared Function HiWord(ByVal dwValue As Integer) As Integer
        Return (dwValue >> 16) And &HFFFF
    End Function
    <StructLayout(LayoutKind.Explicit)>
    Public Structure RECT
        ' Fields
        <FieldOffset(12)>
        Public bottom As Integer
        <FieldOffset(0)>
        Public left As Integer
        <FieldOffset(8)>
        Public right As Integer
        <FieldOffset(4)>
        Public top As Integer

        ' Methods
        Public Sub New(ByVal rect As Rectangle)
            Me.left = rect.Left
            Me.top = rect.Top
            Me.right = rect.Right
            Me.bottom = rect.Bottom
        End Sub

        Public Sub New(ByVal left As Integer, ByVal top As Integer, ByVal right As Integer, ByVal bottom As Integer)
            Me.left = left
            Me.top = top
            Me.right = right
            Me.bottom = bottom
        End Sub

        Public Sub [Set]()
            Me.left = InlineAssignHelper(Me.top, InlineAssignHelper(Me.right, InlineAssignHelper(Me.bottom, 0)))
        End Sub

        Public Sub [Set](ByVal rect As Rectangle)
            Me.left = rect.Left
            Me.top = rect.Top
            Me.right = rect.Right
            Me.bottom = rect.Bottom
        End Sub

        Public Sub [Set](ByVal left As Integer, ByVal top As Integer, ByVal right As Integer, ByVal bottom As Integer)
            Me.left = left
            Me.top = top
            Me.right = right
            Me.bottom = bottom
        End Sub

        Public Function ToRectangle() As Rectangle
            Return New Rectangle(Me.left, Me.top, Me.right - Me.left, Me.bottom - Me.top)
        End Function

        ' Properties
        Public ReadOnly Property Height() As Integer
            Get
                Return (Me.bottom - Me.top)
            End Get
        End Property

        Public ReadOnly Property Size() As Size
            Get
                Return New Size(Me.Width, Me.Height)
            End Get
        End Property

        Public ReadOnly Property Width() As Integer
            Get
                Return (Me.right - Me.left)
            End Get
        End Property
        Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, ByVal value As T) As T
            target = value
            Return value
        End Function
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Structure NCCALCSIZE_PARAMS
        Public rect0 As RECT, rect1 As RECT, rect2 As RECT
        ' Can't use an array here so simulate one
        Private lppos As IntPtr
    End Structure
    <DllImport("dwmapi.dll")>
    Public Shared Function DwmDefWindowProc(ByVal hwnd As IntPtr, ByVal msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr, ByRef result As IntPtr) As Integer
    End Function
    <DllImport("user32.dll")>
    Public Shared Function ReleaseCapture() As Boolean
    End Function
    <DllImport("user32.dll")>
    Public Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    End Function

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        If DesignMode Then Exit Sub
        ms = 2
        If Me.Width - BorderWidth > e.Location.X AndAlso
                    e.Location.X > BorderWidth AndAlso e.Location.Y > BorderWidth Then
            MoveControl(Me.Handle)
        End If
        MyBase.OnMouseDown(e)
    End Sub
    Private Sub MoveControl(ByVal hWnd As IntPtr)
        If DesignMode Then Exit Sub
        ReleaseCapture()
        SendMessage(hWnd, WM_NCLBUTTONDOWN, HTCAPTION, 0)
    End Sub
#End Region
#Region "Ctor"
    Public Sub New()
        Opacity = 0
        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint, True)
        DoubleBuffered = True
        FormBorderStyle = FormBorderStyle.None
    End Sub
#End Region
    Protected Overloads Overrides Sub WndProc(ByRef m As Message)


        Dim result As IntPtr
        Dim dwmHandled As Integer = DwmDefWindowProc(m.HWnd, m.Msg, m.WParam, m.LParam, result)

        If dwmHandled = 1 Then
            m.Result = result
            Exit Sub
        End If

        If m.Msg = WM_NCCALCSIZE AndAlso CInt(m.WParam) = 1 Then
            Dim nccsp As NCCALCSIZE_PARAMS =
              DirectCast(Marshal.PtrToStructure(m.LParam,
              GetType(NCCALCSIZE_PARAMS)), NCCALCSIZE_PARAMS)

            ' Adjust (shrink) the client rectangle to accommodate the border:
            nccsp.rect0.top += 0
            nccsp.rect0.bottom += 0
            nccsp.rect0.left += 0
            nccsp.rect0.right += 0

            If Not _marginOk Then
                'Set what client area would be for passing to 
                'DwmExtendIntoClientArea. Also remember that at least 
                'one of these values NEEDS TO BE > 1, else it won't work.
                dwmMargins.cyTopHeight = 6
                dwmMargins.cxLeftWidth = 6
                dwmMargins.cyBottomHeight = 6
                dwmMargins.cxRightWidth = 6
                _marginOk = True
            End If

            Marshal.StructureToPtr(nccsp, m.LParam, False)

            m.Result = IntPtr.Zero


        ElseIf m.Msg = WM_NCHITTEST AndAlso CInt(m.Result) = 0 Then
            m.Result = HitTestNCA(m.HWnd, m.WParam, m.LParam)
        Else : MyBase.WndProc(m)
        End If
    End Sub
    Private Function HitTestNCA(ByVal hwnd As IntPtr, ByVal wparam _
                                      As IntPtr, ByVal lparam As IntPtr) As IntPtr

        Dim p As New Point(LoWord(CInt(lparam)), HiWord(CInt(lparam)))

        Dim topleft As Rectangle = RectangleToScreen(New Rectangle(0, 0, dwmMargins.cxLeftWidth, dwmMargins.cxLeftWidth))
        Dim topright As Rectangle = RectangleToScreen(New Rectangle(Width - dwmMargins.cxRightWidth, 0, dwmMargins.cxRightWidth, dwmMargins.cxRightWidth))
        Dim botleft As Rectangle = RectangleToScreen(New Rectangle(0, Height - dwmMargins.cyBottomHeight, dwmMargins.cxLeftWidth, dwmMargins.cyBottomHeight))
        Dim botright As Rectangle = RectangleToScreen(New Rectangle(Width - dwmMargins.cxRightWidth, Height - dwmMargins.cyBottomHeight, dwmMargins.cxRightWidth, dwmMargins.cyBottomHeight))
        Dim top As Rectangle = RectangleToScreen(New Rectangle(0, 0, Width, dwmMargins.cxLeftWidth))
        Dim cap As Rectangle = RectangleToScreen(New Rectangle(0, dwmMargins.cxLeftWidth, Width, dwmMargins.cyTopHeight - dwmMargins.cxLeftWidth))
        Dim left As Rectangle = RectangleToScreen(New Rectangle(0, 0, dwmMargins.cxLeftWidth, Height))
        Dim right As Rectangle = RectangleToScreen(New Rectangle(Width - dwmMargins.cxRightWidth, 0, dwmMargins.cxRightWidth, Height))
        Dim bottom As Rectangle = RectangleToScreen(New Rectangle(0, Height - dwmMargins.cyBottomHeight, Width, dwmMargins.cyBottomHeight))


        If topleft.Contains(p) Then Return New IntPtr(HTTOPLEFT)
        If topright.Contains(p) Then Return New IntPtr(HTTOPRIGHT)
        If botleft.Contains(p) Then Return New IntPtr(HTBOTTOMLEFT)
        If botright.Contains(p) Then Return New IntPtr(HTBOTTOMRIGHT)
        If top.Contains(p) Then Return New IntPtr(HTTOP)
        If cap.Contains(p) Then Return New IntPtr(HTCAPTION)
        If left.Contains(p) Then Return New IntPtr(HTLEFT)
        If right.Contains(p) Then Return New IntPtr(HTRIGHT)
        If bottom.Contains(p) Then Return New IntPtr(HTBOTTOM)

        Return New IntPtr(HTCLIENT)
    End Function
    Protected Overrides Sub SetBoundsCore(x As Integer, y As Integer, width As Integer, height As Integer, specified As BoundsSpecified)
        If DesignMode Then MyBase.SetBoundsCore(x, y, width, height, specified)
    End Sub

#End Region

#Region "VISUALS"


    Protected Overrides Sub OnPaintBackground(e As PaintEventArgs)



        'If fxt <> 0 Then closefx()
        Dim g = e.Graphics
        Static bc As Color
        Dim d As Boolean = 0
        g.Clear(BackColor) 'prep



        If rescol(BackColor) = Color.White Then
            'dark
            d = 1
            bc = col(10, 255)
            tp = Helpers.mp(col(20, 255), 0)
            g.DrawRectangle(tp, 0, 0, Width - 1, Height - 1)
            tp = Helpers.mp(col(15, 255))
        Else 'light
            d = 0
            bc = col(60, 0)
            tp = Helpers.mp(col(50, 0), 0)
            g.DrawRectangle(tp, 0, 0, Width - 1, Height - 1)
            tp = Helpers.mp(col(30, 0))
        End If
        g.FillRectangle(mb(ForeColor), rct(Me))
        g.FillRectangle(mb(BackColor), rct(0, 34, Width - 0, Height - 3 - 34))
        g.DrawRectangle(tp, 0, 0, Width - 1, Height - 1) 'border'fill and border


        mp(col(30, 0))
        'g.DrawLine(tp, 0, 33, Width, 33)
        'mp(col(100, 255)) : g.DrawLine(tp, 0, 34, Width, 34)

        Dim t As Byte : If d Then t = 20 Else t = 60

        Dim inp As New Interpolation
        Dim a!
        For j = 0 To 10
            a = inp.GetValue(0, t, j, 10, Interpolation.Type.EaseIn, Interpolation.EasingMethods.Exponent, 3)
            g.DrawLine(Helpers.mp(col(a, 255)), 0, 31 - (10 - j), Width, 31 - (10 - j))
            a = inp.GetValue(0, 128, j, 10, Interpolation.Type.EaseIn, Interpolation.EasingMethods.Exponent, 1)
            g.DrawLine(Helpers.mp(col(a, 0)), 0, 34 + (10 - j), Width, 34 + (10 - j))
        Next

        If d Then t = 60 Else t = 40

        mp(col(t, 0)) : g.DrawLine(tp, 0, 32, Width, 32)
        mp(col(t + 10, 0)) : g.DrawLine(tp, 0, 33, Width, 33)



        For i! = 0 To 23
            a = inp.GetValue(0, 128, i, 50, Interpolation.Type.Smootherstep, 3)
            g.DrawLine(New Pen(col(a * 2, 0)), 0, Height - 3 - 23 + i, Width, Height - 3 - 23 + i)
        Next 'shadows


        If rescol(ForeColor) = Color.White Then '							dark
            g.DrawImageUnscaled(lb(0), pt(Width - 99 - BorderWidth / 2, 0))
            g.DrawImageUnscaled(lb(3), pt(Width - 99 - BorderWidth / 2 + 66, 0))
            If WindowState = FormWindowState.Normal Then g.DrawImageUnscaled(lb(1), pt(Width - 99 - BorderWidth / 2 + 33, 0)) Else g.DrawImageUnscaled(lb(2), pt(Width - 99 - BorderWidth / 2 + 33, 0))
        Else
            g.DrawImageUnscaled(db(0), pt(Width - 99 - BorderWidth / 2, 0)) '			light
            g.DrawImageUnscaled(db(3), pt(Width - 99 - BorderWidth / 2 + 66, 0))
            If WindowState = FormWindowState.Normal Then g.DrawImageUnscaled(db(1), pt(Width - 99 - BorderWidth / 2 + 33, 0)) Else g.DrawImageUnscaled(db(2), pt(Width - 99 - BorderWidth / 2 + 33, 0))
        End If

        If ms = 1 And cbx > 0 Then
            tb = mb(bc)
            g.FillRectangle(tb, cbx, 0, 33, 32)
        End If 'controlbox



        px(Color.Fuchsia, 0, 0, g)
        px(Color.Fuchsia, Width - 1, 0, g)
        px(Color.Fuchsia, 0, Height - 1, g)
        px(Color.Fuchsia, Width - 1, Height - 1, g) 'corners'corners


        g.TextRenderingHint = 5
        g.DrawString(Text, Font, mb(col(100, 0)), 10, 11)
        g.DrawString(Text, Font, mb(BackColor), 10, 10)

        tp.Dispose() : tb.Dispose()
    End Sub
    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        ms = 1
        MyBase.OnMouseMove(e)
        For Each c As Control In Controls
            'On Error Resume Next : DirectCast(c, customControl).leavemouse(e)
        Next
        Dim r = Width - 99 - BorderWidth / 2
        If e.X > r And e.X < Width - BorderWidth / 2 And e.Y < 34 And e.Y > 0 Then
            If e.X > r And e.X < r + 34 Then cbx = r
            If e.X > r + 33 And e.X < r + 67 Then cbx = r + 33
            If e.X > r + 65 Then cbx = r + 66
        Else : cbx = 0
        End If
        Invalidate()
    End Sub
    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        cbx = 0
    End Sub
    Public Sub leavemouse(e As EventArgs) Implements AnimatedObject.leavemouse
        Me.OnMouseLeave(e)
    End Sub
    Private Sub custom_invalidate() Implements AnimatedObject.invalidate
        Invalidate()
    End Sub
#End Region

#Region "FORM LOADING and CLOSING"
    Private Sub me_Load(sender As Object, e As EventArgs) Handles Me.Load
        Opacity = 1
        If DesignMode Then
            FormBorderStyle = FormBorderStyle.None
        Else
            Shadow.BackColor = Color.Black
            Shadow.Visible = 1
            FormBorderStyle = FormBorderStyle.Sizable
            fxt = 0
            Top = (Screen.PrimaryScreen.WorkingArea.Height - Height)
        End If
        MinimumSize = New Size(100, 34)

        TransparencyKey = Color.Fuchsia
        StartPosition = FormStartPosition.CenterScreen
    End Sub
    '    Private Sub CustomWindow_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
    '        e.Cancel = True
    '        Shadow.Hide()
    '        Shadow.Visible = False
    '        Shadow.Dispose()
    '        For Each c As Control In Controls
    '            c.Dispose()
    '        Next
    '        fxt = 1


    '        '_________________̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲
    '        'End the process  ͟___________________________________________________̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲̲ 
    '        '‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾																													
    '        Dim PR() As Process = Process.GetProcessesByName("magnify") '            
    '        For Each Process As Process In PR '                                                    
    '            On Error GoTo e  '    '                                                                
    '            Process.Kill() '                                                                                 
    '        Next '																 ̲ ̲  FUCK YOU MAGNIFYIER ̲͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟͟ ̲̲̲̲ 
    '        '																	  ‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾	 
    '        '̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅‾̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅‾̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅‾̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅̅ ̅̅‾̅̅‾̅̅‾̅̅‾̅̅‾̅̅‾̅̅‾̅̅‾̅̅‾̅̅‾̅̅‾̅̅‾̅̅‾̅̅̅ ̅̅̅̅̅̅̅̅̅̅̅̅̅̅̅̅̅̅̅


    'e:
    '        Me.AccessibleDescription = "Animated Form"
    '    End Sub
    Protected Overrides Sub Dispose(disposing As Boolean)
        MyBase.Dispose(disposing)
        'animatedforms.Remove(Me)
        'If animatedforms.Count = 0 Then stopTimer()
    End Sub
#End Region

#Region "SHADOW"
#Region "declare"
    Private Shadow As New ShadowForm(Me, 14)
    Private Rounding!
#End Region
    Public op# = 1.0#
    Private Sub DrawShadow() Handles Me.SizeChanged
        If DesignMode Or IsHandleCreated = False Then Exit Sub
        Dim B As New Bitmap(CInt(Shadow.Size.Width), CInt(Shadow.Size.Height))
        Dim G As Graphics = Graphics.FromImage(B)
        G.InterpolationMode = 7
        G.SmoothingMode = 2 : G.PixelOffsetMode = 2
        Shadow.BackColor = Color.Black
        Static br As SolidBrush
        With G
            Dim s = Shadow.ShadowSize
            .SetClip(New Rectangle(Shadow.ShadowSize + 2, Shadow.ShadowSize + 2, Width - 4, Height - 5 - 4), CombineMode.Exclude)
            For i = s To 0 Step -1
                Rounding = 5 + s - i
                Dim pth As GraphicsPath = DM.CreateRoundRectangle(i, i, Shadow.Width - 1 - (i * 2), Shadow.Height - 1 - (i * 2) - 3, Rounding, , , , )
                mb(col((i ^ (0.111 * i) * op), Shadow.BackColor), br)
                .FillPath(br, pth)
                pth.Dispose()
            Next


            ''(i ^ (i / 9))          Rounding +  (Shadow.ShadowSize - i)
            'Rounding = 1.5 * (Shadow.ShadowSize)
            ''(Shadow.ShadowSize + i) ^ 1.6
            'mb(col((i ^ (0.117647 * i)), Shadow.BackColor), br)




        End With

        G.Dispose()
        Shadow.SetBits(B)
        B.Dispose()

    End Sub
    'below by blackcap
    Friend Class DM

        Public Shared Function CreateRoundRectangle(ByVal rectangle As Rectangle, ByVal radius As Integer, Optional ByVal TopLeft As Boolean = True, Optional ByVal TopRigth As Boolean = True, Optional ByVal BottomRigth As Boolean = True, Optional ByVal BottomLeft As Boolean = True) As GraphicsPath
            Dim path As New GraphicsPath()
            Dim l As Integer = rectangle.Left
            Dim t As Integer = rectangle.Top
            Dim w As Integer = rectangle.Width
            Dim h As Integer = rectangle.Height
            Dim d As Integer = radius << 1

            If radius <= 0 Then
                path.AddRectangle(rectangle)
                Return path
            End If

            If TopLeft Then
                path.AddArc(l, t, d, d, 180, 90)
                If TopRigth Then path.AddLine(l + radius, t, l + w - radius, t) Else path.AddLine(l + radius, t, l + w, t)
            Else
                If TopRigth Then path.AddLine(l, t, l + w - radius, t) Else path.AddLine(l, t, l + w, t)
            End If

            If TopRigth Then
                path.AddArc(l + w - d, t, d, d, 270, 90)
                If BottomRigth Then path.AddLine(l + w, t + radius, l + w, t + h - radius) Else path.AddLine(l + w, t + radius, l + w, t + h)
            Else
                If BottomRigth Then path.AddLine(l + w, t, l + w, t + h - radius) Else path.AddLine(l + w, t, l + w, t + h)
            End If

            If BottomRigth Then
                path.AddArc(l + w - d, t + h - d, d, d, 0, 90)
                If BottomLeft Then path.AddLine(l + w - radius, t + h, l + radius, t + h) Else path.AddLine(l + w - radius, t + h, l, t + h)
            Else
                If BottomLeft Then path.AddLine(l + w, t + h, l + radius, t + h) Else path.AddLine(l + w, t + h, l, t + h)
            End If

            If BottomLeft Then
                path.AddArc(l, t + h - d, d, d, 90, 90)
                If TopLeft Then path.AddLine(l, t + h - radius, l, t + radius) Else path.AddLine(l, t + h - radius, l, t)
            Else
                If TopLeft Then path.AddLine(l, t + h, l, t + radius) Else path.AddLine(l, t + h, l, t)
            End If

            path.CloseFigure()
            Return path
        End Function
        Public Shared Function CreateRoundRectangle(x As Integer, y As Integer, w As Integer, h As Integer, radius As Integer, Optional ByVal TopLeft As Boolean = True, Optional ByVal TopRigth As Boolean = True, Optional ByVal BottomRigth As Boolean = True, Optional ByVal BottomLeft As Boolean = True) As GraphicsPath
            Return CreateRoundRectangle(New Rectangle(x, y, w, h), radius, TopLeft, TopRigth, BottomRigth, BottomLeft)
        End Function

    End Class
    'above by blackcap
#End Region



#Region "CONTROL BOX"

#Region "Functions"
    Sub mp(c As Byte)
        tp = New Pen(col(c))
    End Sub
    Sub mp(c As Color)
        tp = New Pen(c)
    End Sub
    Sub mp(a As Byte, c As Color)
        tp = New Pen(col(a, c))
    End Sub
    Sub px(c As Color, x!, y!, g As Graphics)
        tb = New SolidBrush(c)
        g.FillRectangle(tb, x, y, 1, 1)
        tb.Dispose()
    End Sub
    Function gri(b As Bitmap) As Graphics
        Return Graphics.FromImage(b)
    End Function
#End Region

#Region "Draw"
    Sub drawmin(gd As Graphics, gl As Graphics)

        gl.InterpolationMode = 7


        mp(col(170, 255))
        gl.DrawLine(tp, 11, 21, 21, 21)


        mp(col(10, 0))
        gl.DrawLine(tp, 11, 19, 21, 19)
        mp(col(40, 0))
        gl.DrawLine(tp, 11, 22, 21, 22)
        mp(col(100, 0))
        gl.DrawLine(tp, 11, 20, 21, 20)


        px(col(40, 0), 10, 20, gl)
        px(col(35, 0), 10, 21, gl)
        px(col(40, 0), 22, 20, gl)
        px(col(35, 0), 22, 21, gl) '									light version

        gd.InterpolationMode = 7
        mp(70)
        gd.DrawLine(tp, 11, 20, 21, 20)
        mp(97)
        gd.DrawLine(tp, 11, 21, 21, 21)

        If rescol(BackColor) = Color.White Then
            mp(col(51, 0))
            gd.DrawLine(tp, 12, 20, 20, 20)
        Else
            mp(col(50, 255))
            gd.DrawLine(tp, 12, 22, 20, 22)
            mp(col(40, 255))
            gd.DrawLine(tp, 12, 19, 20, 19)
            px(col(40, 255), 10, 20, gd)
            px(col(35, 255), 10, 21, gd)
            px(col(40, 255), 22, 20, gd)
            px(col(35, 255), 22, 21, gd)
        End If '									dark version

    End Sub


#Region "drawmax stale code"
    '  tgb = New LinearGradientBrush(New Rectangle(46, 12, 11, 11), col(120, 255), col(170, 255), 90.0F)
    'gl.FillRectangle(tgb, New Rectangle(46, 11, 11, 11))
    'mp(col(22, 0))
    'gl.DrawRectangle(tp, 46, 12, 10, 9)
    'mp(col(3, 255))
    'gl.DrawLine(tp, 46, 13, 56, 13)
    'mp(60)
    'gl.DrawLine(tp, 46, 11, 56, 11)
    'mp(col(80, 0))
    'gl.DrawLine(tp, 48, 20, 54, 20)



    'gl.ResetClip()
    'mp(col(40, 0))
    'gl.DrawLine(tp, 48, 13, 54, 13)
    'mp(col(24, 0))
    'gl.DrawLine(tp, 48, 13, 48, 19)
    'mp(col(24, 0))
    'gl.DrawLine(tp, 54, 13, 54, 19)
    'mp(col(12, 0))
    'gl.DrawLine(tp, 48, 19, 54, 19)

    'mp(col(16, 0))
    'gl.DrawLine(tp, 46, 10, 56, 10)
    'mp(col(38, 0))
    'gl.DrawLine(tp, 46, 22, 56, 22)
    'mp(col(24, 0))
    'gl.DrawLine(tp, 45, 11, 45, 21)
    'mp(col(24, 0))
    'gl.DrawLine(tp, 57, 11, 57, 21)
    '      px(col(2, 0), 45, 10, gl)
    '      px(col(2, 0), 57, 10, gl)
    'px(col(8, 0), 45, 22, gl)
    '      px(col(2, 0), 57, 22, gl)
#End Region


    Sub drawmax(gd As Graphics, gl As Graphics)

        gl.InterpolationMode = 7


        gl.SetClip(New Rectangle(48, 13, 7, 7), CombineMode.Exclude)
        ' gl.Clear(col(28, Color.Yellow))
        mp(col(80, 0))
        gl.DrawRectangle(tp, 11, 10, 11, 11)
        mp(col(10, 0))
        gl.DrawRectangle(tp, 11, 9, 11, 11)
        mp(col(40, 0))
        gl.DrawRectangle(tp, 11, 12, 11, 11)
        gl.DrawLine(tp, 10, 10, 10, 22)
        gl.DrawLine(tp, 23, 10, 23, 22)
        mp(col(20, 0))
        gl.DrawLine(tp, 12, 12, 12, 21)
        gl.DrawLine(tp, 21, 12, 21, 21)


        mp(col(195))
        gl.DrawRectangle(tp, 11, 11, 11, 11)
        '									light version ( used when back is dark )

        gd.InterpolationMode = 7
        gd.RenderingOrigin = pt(-34, 0)
        gd.SetClip(New Rectangle(48 - 34, 13, 7, 7), CombineMode.Exclude)
        tgb = New LinearGradientBrush(New Rectangle(46, 12, 11, 11), col(85), col(100), 90.0F)
        gd.FillRectangle(tgb, New Rectangle(46 - 34, 11, 11, 11))

        mp(col(22, 0))
        gd.DrawRectangle(tp, 46, 12, 10, 9)
        mp(col(3, 255))
        gd.DrawLine(tp, 46, 13, 56, 13)
        mp(60)
        gd.DrawLine(tp, 46, 11, 56, 11)
        mp(51)
        gd.DrawLine(tp, 48, 20, 54, 20)
        gd.ResetClip()
        mp(col(50, 255))
        gd.DrawLine(tp, 48, 13, 54, 13)
        mp(col(24, 255))
        gd.DrawLine(tp, 48, 13, 48, 19)
        mp(col(24, 255))
        gd.DrawLine(tp, 54, 13, 54, 19)
        mp(col(12, 255))
        gd.DrawLine(tp, 48, 19, 54, 19)

        mp(col(16, 255))
        gd.DrawLine(tp, 46, 10, 56, 10)
        mp(col(38, 255))
        gd.DrawLine(tp, 46, 22, 56, 22)
        mp(col(24, 255))
        gd.DrawLine(tp, 45, 11, 45, 21)
        mp(col(24, 255))
        gd.DrawLine(tp, 57, 11, 57, 21)

        px(col(2, 255), 45, 10, gd)
        px(col(2, 255), 57, 10, gd)
        px(col(8, 255), 45, 22, gd)
        px(col(2, 255), 57, 22, gd) '									dark version

    End Sub
    Sub drawrestore(gd As Graphics, gl As Graphics)

        gl.InterpolationMode = 7

        mp(64)
        gl.DrawLine(tp, 46, 15, 48, 15)
        gl.DrawLine(tp, 50, 11, 56, 11)
        mp(56)
        gl.DrawLine(tp, 48, 20, 50, 20)
        gl.DrawLine(tp, 51, 19, 52, 19)
        gl.DrawLine(tp, 52, 16, 54, 16)

        tgb = New LinearGradientBrush(New Rectangle(50, 12, 7, 6), col(105), col(89), 90.0F)
        gl.SetClip(New Rectangle(52, 13, 3, 4), CombineMode.Exclude)
        gl.FillRectangle(tgb, tgb.Rectangle)
        gl.ResetClip()

        mp(105)
        gl.DrawLine(tp, 46, 16, 48, 16)
        mp(102)
        gl.DrawLine(tp, 46, 17, 47, 17)
        mp(99)
        gl.DrawLine(tp, 46, 18, 47, 18)
        mp(95)
        gl.DrawLine(tp, 46, 19, 47, 19)
        mp(92)
        gl.DrawLine(tp, 51, 20, 52, 20)
        gl.DrawLine(tp, 46, 20, 47, 20)
        mp(89)
        gl.DrawLine(tp, 46, 21, 52, 21)


        mp(col(50, 0))
        px(col(50, 0), 48, 17, gl)
        gl.DrawLine(tp, 46, 22, 52, 22)
        gl.DrawLine(tp, 50, 18, 56, 18)
        gl.DrawLine(tp, 52, 13, 54, 13) '									light version

        gd.InterpolationMode = 7
        mp(64)
        gd.DrawLine(tp, 46, 15, 48, 15)
        gd.DrawLine(tp, 50, 11, 56, 11)
        mp(56)
        gd.DrawLine(tp, 48, 20, 50, 20)
        gd.DrawLine(tp, 51, 19, 52, 19)
        gd.DrawLine(tp, 52, 16, 54, 16)

        tgb = New LinearGradientBrush(New Rectangle(50, 12, 7, 6), col(105), col(89), 90.0F)
        gd.SetClip(New Rectangle(52, 13, 3, 4), CombineMode.Exclude)
        gd.FillRectangle(tgb, tgb.Rectangle)
        gd.ResetClip()

        mp(105)
        gd.DrawLine(tp, 46, 16, 48, 16)
        mp(102)
        gd.DrawLine(tp, 46, 17, 47, 17)
        mp(99)
        gd.DrawLine(tp, 46, 18, 47, 18)
        mp(95)
        gd.DrawLine(tp, 46, 19, 47, 19)
        mp(92)
        gd.DrawLine(tp, 51, 20, 52, 20)
        gd.DrawLine(tp, 46, 20, 47, 20)
        mp(89)
        gd.DrawLine(tp, 46, 21, 52, 21)

        mp(col(50, 255))
        px(col(50, 255), 48, 17, gd)
        gd.DrawLine(tp, 46, 22, 52, 22)
        gd.DrawLine(tp, 50, 18, 56, 18)
        gd.DrawLine(tp, 52, 13, 54, 13) '									dark version

    End Sub
    Sub drawclose(gd As Graphics, gl As Graphics)
        Dim g As Graphics
        Static im As New Bitmap(13, 13)
        im = New Bitmap(Image.FromStream(New System.IO.MemoryStream(Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAA0AAAANCAYAAABy6+R8AAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwAAADsABataJCQAAABp0RVh0U29mdHdhcmUAUGFpbnQuTkVUIHYzLjUuMTAw9HKhAAAAxUlEQVQoU5WPQQqDMBREvZOiC1u1e8GiAau1YhEt3v8A00zgpzFSShcvPzPvBzQA8Df2UhQXPY4LguvNkecFuq4DpwgX31uxbRva9oYsy4wQmNnTS2clWdcXmkbhdDrriICTmb3skN0jsiwLlFJI09RMZn9nF4R5nlHXNTh9Rw5FkiSYpqeF2d/ZhTiOzT+M46gjAk5m9rJD7CWKIvNJw/DQ8bPAzJ5eOnOEYYiquqLv71a4sKfnHrMVZVma4huuP8jfIHgDP+3Ac95JZCsAAAAASUVORK5CYII="))))

        gl.InterpolationMode = 7

        Static cgl As New Bitmap(23, 23)
        g = Graphics.FromImage(cgl)
        g.InterpolationMode = 7
        g.CompositingQuality = 2

        px(col(16, 0), 2, 0, g)
        px(col(16, 0), 1, 1, g)
        px(col(16, 0), 9, 1, g)
        px(col(16, 0), 10, 0, g)
        px(col(16, 0), 11, 1, g)

        px(col(14, 0), 4, 2, g)
        px(col(14, 0), 5, 3, g)
        px(col(14, 0), 6, 4, g)
        px(col(14, 0), 7, 3, g)
        px(col(14, 0), 8, 2, g)
        px(col(14, 0), 3, 10, g)
        px(col(14, 0), 9, 10, g)

        px(col(12, 0), 2, 8, g)
        px(col(12, 0), 1, 9, g)
        px(col(12, 0), 0, 10, g)
        px(col(12, 0), 10, 8, g)
        px(col(12, 0), 11, 9, g)
        px(col(12, 0), 12, 10, g)

        px(col(38, 0), 1, 11, g)
        px(col(38, 0), 2, 12, g)
        px(col(38, 0), 3, 11, g)
        px(col(38, 0), 9, 11, g)
        px(col(38, 0), 10, 12, g)
        px(col(38, 0), 11, 11, g)

        px(col(40, 0), 6, 8, g)
        px(col(40, 0), 5, 9, g)
        px(col(40, 0), 4, 10, g)
        px(col(40, 0), 7, 9, g)
        px(col(40, 0), 8, 10, g)

        px(col(46, 0), 0, 2, g)
        px(col(46, 0), 1, 3, g)
        px(col(46, 0), 12, 2, g)
        px(col(46, 0), 11, 3, g)

        px(col(44, 0), 2, 4, g)
        px(col(44, 0), 3, 5, g)
        px(col(44, 0), 10, 4, g)
        px(col(44, 0), 9, 5, g)

        px(col(36, 0), 4, 6, g)
        px(col(36, 0), 8, 6, g) '																shadow



        gl.DrawImageUnscaled(im, 10, 10)
        gl.DrawImageUnscaled(cgl, 10, 10) '									light version (dark back)

        gd.InterpolationMode = 7
        g.CompositingQuality = 2

        Static cgd As New Bitmap(23, 23)
        g = Graphics.FromImage(cgd)
        g.InterpolationMode = 7

        px(col(16, 255), 2, 0, g)
        px(col(16, 255), 1, 1, g)
        px(col(16, 255), 9, 1, g)
        px(col(16, 255), 10, 0, g)
        px(col(16, 255), 11, 1, g)

        px(col(14, 255), 4, 2, g)
        px(col(14, 255), 5, 3, g)
        px(col(14, 255), 6, 4, g)
        px(col(14, 255), 7, 3, g)
        px(col(14, 255), 8, 2, g)
        px(col(14, 255), 3, 10, g)
        px(col(14, 255), 9, 10, g)

        px(col(12, 255), 2, 8, g)
        px(col(12, 255), 1, 9, g)
        px(col(12, 255), 0, 10, g)
        px(col(12, 255), 10, 8, g)
        px(col(12, 255), 11, 9, g)
        px(col(12, 255), 12, 10, g)

        px(col(38, 255), 1, 11, g)
        px(col(38, 255), 2, 12, g)
        px(col(38, 255), 3, 11, g)
        px(col(38, 255), 9, 11, g)
        px(col(38, 255), 10, 12, g)
        px(col(38, 255), 11, 11, g)

        px(col(40, 255), 6, 8, g)
        px(col(40, 255), 5, 9, g)
        px(col(40, 255), 4, 10, g)
        px(col(40, 255), 7, 9, g)
        px(col(40, 255), 8, 10, g)

        px(col(46, 255), 0, 2, g)
        px(col(46, 255), 1, 3, g)
        px(col(46, 255), 12, 2, g)
        px(col(46, 255), 11, 3, g)

        px(col(44, 255), 2, 4, g)
        px(col(44, 255), 3, 5, g)
        px(col(44, 255), 10, 4, g)
        px(col(44, 255), 9, 5, g)

        px(col(36, 255), 4, 6, g)
        px(col(36, 255), 8, 6, g)

        gd.DrawImageUnscaled(im, 10, 10)
        gd.DrawImageUnscaled(cgd, 10, 10) '									dark version

    End Sub
#End Region
    Dim ccy As Integer = 0
    Protected Overrides Sub OnCreateControl()

        'If Not animatedforms.Contains(Me) Then animatedforms.Add(Me)

        For i = 0 To 3
            lb(i) = New Bitmap(33, 33) : db(i) = New Bitmap(33, 33)
        Next

        drawmin(gri(db(0)), gri(lb(0)))
        drawmax(gri(db(1)), gri(lb(1)))
        drawrestore(gri(db(2)), gri(lb(2)))
        drawclose(gri(db(3)), gri(lb(3)))

        tgb.Dispose()

        ccy = 1
        DrawShadow()


        MyBase.OnCreateControl()

    End Sub

#End Region





End Class ' DISPOSE done














































'#Region "CONTROL BOX"

'#Region "Functions"
'	Sub mp(c As Byte)
'		tp = New Pen(col(c))
'	End Sub
'	Sub mp(c As Color)
'		tp = New Pen(c)
'	End Sub
'	Sub mp(a As Byte, c As Color)
'		tp = New Pen(col(a, c))
'	End Sub
'	Sub px(c As Color, x!, y!, g As Graphics)
'		tb = New SolidBrush(c)
'		g.FillRectangle(tb, x, y, 1, 1)
'		tb.Dispose()
'	End Sub
'	Function gri(b As Bitmap) As Graphics
'		Return Graphics.FromImage(b)
'	End Function
'#End Region

'#Region "Draw"
'	Sub drawmin(gd As Graphics, gl As Graphics)

'		gl.InterpolationMode = 7


'		mp(70)
'		gl.DrawLine(tp, 11, 20, 21, 20)
'		mp(97)
'		gl.DrawLine(tp, 11, 21, 21, 21)


'		mp(col(70, Color.Black))
'		gl.DrawLine(tp, 12, 22, 20, 22)
'		mp(col(40, Color.Black))
'		gl.DrawLine(tp, 12, 19, 20, 19)
'		px(col(40, Color.Black), 10, 20, gl)
'		px(col(35, Color.Black), 10, 21, gl)
'		px(col(40, Color.Black), 22, 20, gl)
'		px(col(35, Color.Black), 22, 21, gl) '									light version

'		gd.InterpolationMode = 7
'		mp(70)
'		gd.DrawLine(tp, 11, 20, 21, 20)
'		mp(97)
'		gd.DrawLine(tp, 11, 21, 21, 21)

'		If rescol(BackColor) = Color.White Then
'			mp(col(51, Color.Black))
'			gd.DrawLine(tp, 12, 20, 20, 20)
'		Else
'			mp(col(50, Color.White))
'			gd.DrawLine(tp, 12, 22, 20, 22)
'			mp(col(40, Color.White))
'			gd.DrawLine(tp, 12, 19, 20, 19)
'			px(col(40, Color.White), 10, 20, gd)
'			px(col(35, Color.White), 10, 21, gd)
'			px(col(40, Color.White), 22, 20, gd)
'			px(col(35, Color.White), 22, 21, gd)
'		End If '									dark version

'	End Sub
'	Sub drawmax(gd As Graphics, gl As Graphics)

'		gl.InterpolationMode = 7


'		gl.SetClip(New Rectangle(48, 13, 7, 7), CombineMode.Exclude)
'		tgb = New LinearGradientBrush(New Rectangle(46, 12, 11, 11), col(85), col(100), 90.0F)
'		gl.FillRectangle(tgb, New Rectangle(46, 11, 11, 11))
'		mp(col(22, Color.Black))
'		gl.DrawRectangle(tp, 46, 12, 10, 9)
'		mp(col(3, Color.White))
'		gl.DrawLine(tp, 46, 13, 56, 13)
'		mp(60)
'		gl.DrawLine(tp, 46, 11, 56, 11)
'		mp(51)
'		gl.DrawLine(tp, 48, 20, 54, 20)



'		gl.ResetClip()
'		mp(col(50, Color.Black))
'		gl.DrawLine(tp, 48, 13, 54, 13)
'		mp(col(24, Color.Black))
'		gl.DrawLine(tp, 48, 13, 48, 19)
'		mp(col(24, Color.Black))
'		gl.DrawLine(tp, 54, 13, 54, 19)
'		mp(col(12, Color.Black))
'		gl.DrawLine(tp, 48, 19, 54, 19)

'		mp(col(16, Color.Black))
'		gl.DrawLine(tp, 46, 10, 56, 10)
'		mp(col(38, Color.Black))
'		gl.DrawLine(tp, 46, 22, 56, 22)
'		mp(col(24, Color.Black))
'		gl.DrawLine(tp, 45, 11, 45, 21)
'		mp(col(24, Color.Black))
'		gl.DrawLine(tp, 57, 11, 57, 21)

'		px(col(2, Color.Black), 45, 10, gl)
'		px(col(2, Color.Black), 57, 10, gl)
'		px(col(8, Color.Black), 45, 22, gl)
'		px(col(2, Color.Black), 57, 22, gl)	'									light version

'		gd.InterpolationMode = 7

'		gd.SetClip(New Rectangle(48, 13, 7, 7), CombineMode.Exclude)
'		tgb = New LinearGradientBrush(New Rectangle(46, 12, 11, 11), col(85), col(100), 90.0F)
'		gd.FillRectangle(tgb, New Rectangle(46, 11, 11, 11))

'		mp(col(22, Color.Black))
'		gd.DrawRectangle(tp, 46, 12, 10, 9)
'		mp(col(3, Color.White))
'		gd.DrawLine(tp, 46, 13, 56, 13)
'		mp(60)
'		gd.DrawLine(tp, 46, 11, 56, 11)
'		mp(51)
'		gd.DrawLine(tp, 48, 20, 54, 20)
'		gd.ResetClip()
'		mp(col(50, Color.White))
'		gd.DrawLine(tp, 48, 13, 54, 13)
'		mp(col(24, Color.White))
'		gd.DrawLine(tp, 48, 13, 48, 19)
'		mp(col(24, Color.White))
'		gd.DrawLine(tp, 54, 13, 54, 19)
'		mp(col(12, Color.White))
'		gd.DrawLine(tp, 48, 19, 54, 19)

'		mp(col(16, Color.White))
'		gd.DrawLine(tp, 46, 10, 56, 10)
'		mp(col(38, Color.White))
'		gd.DrawLine(tp, 46, 22, 56, 22)
'		mp(col(24, Color.White))
'		gd.DrawLine(tp, 45, 11, 45, 21)
'		mp(col(24, Color.White))
'		gd.DrawLine(tp, 57, 11, 57, 21)

'		px(col(2, Color.White), 45, 10, gd)
'		px(col(2, Color.White), 57, 10, gd)
'		px(col(8, Color.White), 45, 22, gd)
'		px(col(2, Color.White), 57, 22, gd)	'									dark version

'	End Sub
'	Sub drawrestore(gd As Graphics, gl As Graphics)

'		gl.InterpolationMode = 7

'		mp(64)
'		gl.DrawLine(tp, 46, 15, 48, 15)
'		gl.DrawLine(tp, 50, 11, 56, 11)
'		mp(56)
'		gl.DrawLine(tp, 48, 20, 50, 20)
'		gl.DrawLine(tp, 51, 19, 52, 19)
'		gl.DrawLine(tp, 52, 16, 54, 16)

'		tgb = New LinearGradientBrush(New Rectangle(50, 12, 7, 6), col(105), col(89), 90.0F)
'		gl.SetClip(New Rectangle(52, 13, 3, 4), CombineMode.Exclude)
'		gl.FillRectangle(tgb, tgb.Rectangle)
'		gl.ResetClip()

'		mp(105)
'		gl.DrawLine(tp, 46, 16, 48, 16)
'		mp(102)
'		gl.DrawLine(tp, 46, 17, 47, 17)
'		mp(99)
'		gl.DrawLine(tp, 46, 18, 47, 18)
'		mp(95)
'		gl.DrawLine(tp, 46, 19, 47, 19)
'		mp(92)
'		gl.DrawLine(tp, 51, 20, 52, 20)
'		gl.DrawLine(tp, 46, 20, 47, 20)
'		mp(89)
'		gl.DrawLine(tp, 46, 21, 52, 21)


'		mp(col(50, Color.Black))
'		px(col(50, Color.Black), 48, 17, gl)
'		gl.DrawLine(tp, 46, 22, 52, 22)
'		gl.DrawLine(tp, 50, 18, 56, 18)
'		gl.DrawLine(tp, 52, 13, 54, 13)	'									light version

'		gd.InterpolationMode = 7
'		mp(64)
'		gd.DrawLine(tp, 46, 15, 48, 15)
'		gd.DrawLine(tp, 50, 11, 56, 11)
'		mp(56)
'		gd.DrawLine(tp, 48, 20, 50, 20)
'		gd.DrawLine(tp, 51, 19, 52, 19)
'		gd.DrawLine(tp, 52, 16, 54, 16)

'		tgb = New LinearGradientBrush(New Rectangle(50, 12, 7, 6), col(105), col(89), 90.0F)
'		gd.SetClip(New Rectangle(52, 13, 3, 4), CombineMode.Exclude)
'		gd.FillRectangle(tgb, tgb.Rectangle)
'		gd.ResetClip()

'		mp(105)
'		gd.DrawLine(tp, 46, 16, 48, 16)
'		mp(102)
'		gd.DrawLine(tp, 46, 17, 47, 17)
'		mp(99)
'		gd.DrawLine(tp, 46, 18, 47, 18)
'		mp(95)
'		gd.DrawLine(tp, 46, 19, 47, 19)
'		mp(92)
'		gd.DrawLine(tp, 51, 20, 52, 20)
'		gd.DrawLine(tp, 46, 20, 47, 20)
'		mp(89)
'		gd.DrawLine(tp, 46, 21, 52, 21)

'		mp(col(50, Color.White))
'		px(col(50, Color.White), 48, 17, gd)
'		gd.DrawLine(tp, 46, 22, 52, 22)
'		gd.DrawLine(tp, 50, 18, 56, 18)
'		gd.DrawLine(tp, 52, 13, 54, 13)	'									dark version

'	End Sub
'	Sub drawclose(gd As Graphics, gl As Graphics)
'		Dim g As Graphics
'		Static im As New Bitmap(13, 13)
'		im = New Bitmap(Image.FromStream(New System.IO.MemoryStream(Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAA0AAAANCAYAAABy6+R8AAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwAAADsABataJCQAAABp0RVh0U29mdHdhcmUAUGFpbnQuTkVUIHYzLjUuMTAw9HKhAAAAxUlEQVQoU5WPQQqDMBREvZOiC1u1e8GiAau1YhEt3v8A00zgpzFSShcvPzPvBzQA8Df2UhQXPY4LguvNkecFuq4DpwgX31uxbRva9oYsy4wQmNnTS2clWdcXmkbhdDrriICTmb3skN0jsiwLlFJI09RMZn9nF4R5nlHXNTh9Rw5FkiSYpqeF2d/ZhTiOzT+M46gjAk5m9rJD7CWKIvNJw/DQ8bPAzJ5eOnOEYYiquqLv71a4sKfnHrMVZVma4huuP8jfIHgDP+3Ac95JZCsAAAAASUVORK5CYII="))))

'		gl.InterpolationMode = 7

'		Static cgl As New Bitmap(103 - 80, 33 - 10)
'		g = Graphics.FromImage(cgl)
'		g.InterpolationMode = 7
'		g.CompositingQuality = 2

'		px(col(16, Color.Black), 2, 0, g)
'		px(col(16, Color.Black), 1, 1, g)
'		px(col(16, Color.Black), 9, 1, g)
'		px(col(16, Color.Black), 10, 0, g)
'		px(col(16, Color.Black), 11, 1, g)

'		px(col(14, Color.Black), 4, 2, g)
'		px(col(14, Color.Black), 5, 3, g)
'		px(col(14, Color.Black), 6, 4, g)
'		px(col(14, Color.Black), 7, 3, g)
'		px(col(14, Color.Black), 8, 2, g)
'		px(col(14, Color.Black), 3, 10, g)
'		px(col(14, Color.Black), 9, 10, g)

'		px(col(12, Color.Black), 2, 8, g)
'		px(col(12, Color.Black), 1, 9, g)
'		px(col(12, Color.Black), 0, 10, g)
'		px(col(12, Color.Black), 10, 8, g)
'		px(col(12, Color.Black), 11, 9, g)
'		px(col(12, Color.Black), 12, 10, g)

'		px(col(38, Color.Black), 1, 11, g)
'		px(col(38, Color.Black), 2, 12, g)
'		px(col(38, Color.Black), 3, 11, g)
'		px(col(38, Color.Black), 9, 11, g)
'		px(col(38, Color.Black), 10, 12, g)
'		px(col(38, Color.Black), 11, 11, g)

'		px(col(40, Color.Black), 6, 8, g)
'		px(col(40, Color.Black), 5, 9, g)
'		px(col(40, Color.Black), 4, 10, g)
'		px(col(40, Color.Black), 7, 9, g)
'		px(col(40, Color.Black), 8, 10, g)

'		px(col(46, Color.Black), 0, 2, g)
'		px(col(46, Color.Black), 1, 3, g)
'		px(col(46, Color.Black), 12, 2, g)
'		px(col(46, Color.Black), 11, 3, g)

'		px(col(44, Color.Black), 2, 4, g)
'		px(col(44, Color.Black), 3, 5, g)
'		px(col(44, Color.Black), 10, 4, g)
'		px(col(44, Color.Black), 9, 5, g)

'		px(col(36, Color.Black), 4, 6, g)
'		px(col(36, Color.Black), 8, 6, g) '																shadow



'		gl.DrawImageUnscaled(im, 80, 10)
'		gl.DrawImageUnscaled(cgl, 80, 10) '									light version

'		gd.InterpolationMode = 7
'		g.CompositingQuality = 2

'		Static cgd As New Bitmap(103 - 80, 33 - 10)
'		g = Graphics.FromImage(cgd)
'		g.InterpolationMode = 7

'		px(col(16, Color.White), 2, 0, g)
'		px(col(16, Color.White), 1, 1, g)
'		px(col(16, Color.White), 9, 1, g)
'		px(col(16, Color.White), 10, 0, g)
'		px(col(16, Color.White), 11, 1, g)

'		px(col(14, Color.White), 4, 2, g)
'		px(col(14, Color.White), 5, 3, g)
'		px(col(14, Color.White), 6, 4, g)
'		px(col(14, Color.White), 7, 3, g)
'		px(col(14, Color.White), 8, 2, g)
'		px(col(14, Color.White), 3, 10, g)
'		px(col(14, Color.White), 9, 10, g)

'		px(col(12, Color.White), 2, 8, g)
'		px(col(12, Color.White), 1, 9, g)
'		px(col(12, Color.White), 0, 10, g)
'		px(col(12, Color.White), 10, 8, g)
'		px(col(12, Color.White), 11, 9, g)
'		px(col(12, Color.White), 12, 10, g)

'		px(col(38, Color.White), 1, 11, g)
'		px(col(38, Color.White), 2, 12, g)
'		px(col(38, Color.White), 3, 11, g)
'		px(col(38, Color.White), 9, 11, g)
'		px(col(38, Color.White), 10, 12, g)
'		px(col(38, Color.White), 11, 11, g)

'		px(col(40, Color.White), 6, 8, g)
'		px(col(40, Color.White), 5, 9, g)
'		px(col(40, Color.White), 4, 10, g)
'		px(col(40, Color.White), 7, 9, g)
'		px(col(40, Color.White), 8, 10, g)

'		px(col(46, Color.White), 0, 2, g)
'		px(col(46, Color.White), 1, 3, g)
'		px(col(46, Color.White), 12, 2, g)
'		px(col(46, Color.White), 11, 3, g)

'		px(col(44, Color.White), 2, 4, g)
'		px(col(44, Color.White), 3, 5, g)
'		px(col(44, Color.White), 10, 4, g)
'		px(col(44, Color.White), 9, 5, g)

'		px(col(36, Color.White), 4, 6, g)
'		px(col(36, Color.White), 8, 6, g)

'		gd.DrawImageUnscaled(im, 80, 10)
'		gd.DrawImageUnscaled(cgd, 80, 10) '									dark version

'	End Sub
'#End Region

'	Protected Overrides Sub OnCreateControl()
'		MyBase.OnCreateControl()

'		For i = 0 To 3
'			lb(i) = New Bitmap(103, 33) : db(i) = New Bitmap(103, 33)
'		Next

'		drawmin(gri(db(0)), gri(lb(0)))
'		drawmax(gri(db(1)), gri(lb(1)))
'		drawrestore(gri(db(2)), gri(lb(2)))
'		drawclose(gri(db(3)), gri(lb(3)))

'		tgb.Dispose()
'	End Sub
'#End Region' old cb





'Dim g = e.Graphics
'bv1 = New LinearGradientBrush(New Rectangle(0, Height - CInt(sht), Width, CInt(sht)), Color.Transparent, Color.FromArgb(50, Color.Black), 90.0F)
'bh = New LinearGradientBrush(New Rectangle(0, Height - CInt(sht), Width, CInt(sht)), Color.FromArgb(60, Color.Black), Color.Transparent, 0.0F)
'bh1 = New LinearGradientBrush(New Rectangle(0, Height - CInt(sht), Width, CInt(sht)), Color.Transparent, Color.FromArgb(60, Color.Black), 0.0F)

'If rescol(BackColor) = Color.White Then
'tp = New Pen(Color.FromArgb(20, rescol(BackColor)), 0)
''		g.DrawRectangle(tp, 0, 0, Width - 1, Height - 1)
'Else
'tp = New Pen(Color.FromArgb(60, rescol(BackColor)), 0)
''		g.DrawRectangle(tp, 0, 0, Width - 1, Height - 1)
'tp = New Pen(DM.Invert(rescol(BackColor)), 0)
''		g.DrawRectangle(tp, 1, 1, Width - 3, Height - 2)
'End If




'If sst Then
'tb = New SolidBrush(ForeColor)
'If sst = True Then
'g.FillRectangle(tb, New Rectangle(0, Height - CInt(sht), Width, CInt(sht)))
'g.FillRectangle(bv1, New Rectangle(0, Height - CInt(sht), Width, CInt(sht)))
'g.FillRectangle(bh, New Rectangle(0, Height - CInt(sht), CInt(Width / 2) - 1, CInt(sht)))
'g.FillRectangle(bh1, New Rectangle(CInt(Width / 2) - 1, Height - CInt(sht), CInt(Width / 2) + 2, CInt(sht)))
'End If


'Dim px As Integer() = {13, 11, 9, 7}
'Dim py As Integer() = {7, 9, 11, 13}

'For i = 0 To 3
'For j = 0 To i
'tb = New SolidBrush(Color.FromArgb(180, Color.Black))
'g.FillRectangle(tb, Width - px(i), Height - py(j), 1, 1)
'tb = New SolidBrush(Color.FromArgb(80, Color.White))
'g.FillRectangle(tb, Width - px(i) - 1, Height - py(j) - 1, 1, 1)
'Next
'Next




'tp = New Pen(Color.FromArgb(40, Color.Black))
'g.DrawLine(tp, 0, Height - CInt(sht), Width - 1, Height - CInt(sht))
'tp = New Pen(Color.FromArgb(30, Color.Black))
'g.DrawLine(tp, 0, Height - CInt(sht) + 1, Width - 1, Height - CInt(sht) + 1)


'tp = New Pen(Color.FromArgb(13, Color.White), 0)
'g.DrawLine(tp, 0, Height - 1, Width - 2, Height - 1)
'g.DrawLine(tp, 0, Height - CInt(sht), 0, Height - 2)
'g.DrawLine(tp, Width - 1, Height - CInt(sht), Width - 1, Height - 1)

'bv1.Dispose() : bh.Dispose() : bh1.Dispose()
'tp.Dispose() : tb.Dispose()

'End If




'tp = New Pen(ForeColor, 2)
'If DesignMode Then Exit Sub
'		Select Case cb.l
'Case 0
'g.DrawLine(tp, Width - 86, 1, Width - 85 + 28 - 1, 1)
'Case 1
'g.DrawLine(tp, Width - 86 + 28 + 1, 1, Width - 85 + 28 + 28, 1)
'Case 2
'g.DrawLine(tp, Width - 86 + 28 + 28, 1, Width - 85 + 28 + 28 + 28 - 1, 1)
'Case 3
'End Select
''Sub loadfx()
''    Static lf As New Interpolation
''    If Not fxt = 0 Then Exit Sub
''    If Opacity < 1 Then
''        If ot < 5000 Then
''            Opacity = lf.GetValue(0, 1, ot, 5000, Interpolation.Type.EaseOut, Interpolation.EasingMethods.Exponent, 1)
''            op = Opacity
''            ot += 1
''        Else
''            animating=false
''            Opacity = 1
''            op = 1.0#
''            fxt = -1
''            ot = 0
''        End If
''    End If
''End Sub
'Sub closefx()
'    Static lf1 As New Interpolation
'    If Not fxt = 1 Then Exit Sub
'    If Opacity > 0 Then
'        If ot < 300 Then
'            Opacity = lf1.GetValue(1, 0, ot, 300, Interpolation.Type.EaseOut, Interpolation.EasingMethods.Exponent, 1.5)
'            op = Opacity
'            ot += 1
'        Else
'            animating=false
'            Opacity = 0
'            op = 0.0#
'            fxt = -1
'            ot = 0
'            tmr.Dispose()
'            End
'        End If
'    End If
'End Sub
