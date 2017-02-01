Imports System.Runtime.InteropServices

Public Class ShadowForm
    Inherits Form

    Private Shadows ParentForm As Form
    Public ShadowSize As Integer

#Region "Ctor"
    Public Sub New(ByVal Parent As Form, ByVal ShadowSize As Integer)

        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ContainerControl Or ControlStyles.SupportsTransparentBackColor Or ControlStyles.UserMouse Or ControlStyles.ResizeRedraw Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.Selectable Or ControlStyles.StandardClick Or ControlStyles.StandardDoubleClick Or ControlStyles.Opaque, False)
        DoubleBuffered = True

        Me.ParentForm = Parent
        Me.ShadowSize = ShadowSize

        FormBorderStyle = FormBorderStyle.None
        ShowInTaskbar = False
        ControlBox = False
        Text = ""


        AddHandler Parent.Resize, Sub() UpdateBounds()
        AddHandler Parent.Move, Sub() UpdateBounds()
        AddHandler Parent.Layout, Sub() UpdateBounds()

        AddOwnedForm(ParentForm)


    End Sub
#End Region
    Protected Overrides Sub OnPaintBackground(e As System.Windows.Forms.PaintEventArgs)
    End Sub
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        e.Graphics.InterpolationMode = 7
        e.Graphics.SmoothingMode = 2
    End Sub


    Private Shadows Sub UpdateBounds()
        Location = ParentForm.Location - New Point(ShadowSize, ShadowSize - 5)
        MyBase.Size = ParentForm.ClientSize + New Size(ShadowSize * 2 + 1, ShadowSize * 2 + 0)
    End Sub

    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim cParms As CreateParams = MyBase.CreateParams
            cParms.ExStyle = cParms.ExStyle Or &H80000
            cParms.ExStyle = cParms.ExStyle Or CInt(Fix(Win32.WS_EX_TOOLWINDOW))
            'cParms.ExStyle -= CInt(Fix(Win32.WS_EX_APPWINDOW))
            Return cParms

        End Get
    End Property

    Public Sub SetBits(B As Bitmap)
        If Not IsHandleCreated Or DesignMode Then Exit Sub

        If Not Bitmap.IsCanonicalPixelFormat(B.PixelFormat) OrElse Not Bitmap.IsAlphaPixelFormat(B.PixelFormat) Then
            Throw New ApplicationException("The picture must be 32bit picture with alpha channel.")
        End If

        Dim oldBits As IntPtr = IntPtr.Zero
        Dim screenDC As IntPtr = Win32.GetDC(IntPtr.Zero)
        Dim hBitmap As IntPtr = IntPtr.Zero
        Dim memDc As IntPtr = Win32.CreateCompatibleDC(screenDC)

        Try
            Dim topLoc As New Win32.Point(Left, Top)
            Dim bitMapSize As New Win32.Size(B.Width, B.Height)
            Dim blendFunc As New Win32.BLENDFUNCTION()
            Dim srcLoc As New Win32.Point(0, 0)

            hBitmap = B.GetHbitmap(Color.FromArgb(0))
            oldBits = Win32.SelectObject(memDc, hBitmap)

            blendFunc.BlendOp = Win32.AC_SRC_OVER
            blendFunc.SourceConstantAlpha = 250
            blendFunc.AlphaFormat = Win32.AC_SRC_ALPHA
            blendFunc.BlendFlags = 0

            Win32.UpdateLayeredWindow(Handle, screenDC, topLoc, bitMapSize, memDc, srcLoc,
             0, blendFunc, Win32.ULW_ALPHA)
        Finally
            If hBitmap <> IntPtr.Zero Then
                Win32.SelectObject(memDc, oldBits)
                Win32.DeleteObject(hBitmap)
            End If
            Win32.ReleaseDC(IntPtr.Zero, screenDC)
            Win32.DeleteDC(memDc)
        End Try
    End Sub
End Class

Friend Class Win32

    Public Const WS_EX_TOOLWINDOW As Long = &H80L
    Public Const WS_EX_APPWINDOW As Long = &H40000L
    Public Const AC_SRC_OVER As Byte = 0
    Public Const AC_SRC_ALPHA As Byte = 1
    Public Const ULW_ALPHA As Int32 = 2

    Public Declare Auto Function CreateCompatibleDC Lib "gdi32.dll" (hDC As IntPtr) As IntPtr
    Public Declare Auto Function GetDC Lib "user32.dll" (hWnd As IntPtr) As IntPtr

    <DllImport("gdi32.dll", ExactSpelling:=True)>
    Public Shared Function SelectObject(hDC As IntPtr, hObj As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll", ExactSpelling:=True)>
    Public Shared Function ReleaseDC(hWnd As IntPtr, hDC As IntPtr) As Integer
    End Function

    Public Declare Auto Function DeleteDC Lib "gdi32.dll" (hDC As IntPtr) As Integer
    Public Declare Auto Function DeleteObject Lib "gdi32.dll" (hObj As IntPtr) As Integer
    Public Declare Auto Function UpdateLayeredWindow Lib "user32.dll" (hwnd As IntPtr, hdcDst As IntPtr, ByRef pptDst As Point, ByRef psize As Size, hdcSrc As IntPtr, ByRef pptSrc As Point, crKey As Int32, ByRef pblend As BLENDFUNCTION, dwFlags As Int32) As Integer
    Public Declare Auto Function ExtCreateRegion Lib "gdi32.dll" (lpXform As IntPtr, nCount As UInteger, rgnData As IntPtr) As IntPtr

    <StructLayout(LayoutKind.Sequential)>
    Public Structure Size
        Public cx As Int32
        Public cy As Int32

        Public Sub New(x As Int32, y As Int32)
            cx = x
            cy = y
        End Sub
    End Structure

    <StructLayout(LayoutKind.Sequential, Pack:=1)>
    Public Structure BLENDFUNCTION
        Public BlendOp As Byte
        Public BlendFlags As Byte
        Public SourceConstantAlpha As Byte
        Public AlphaFormat As Byte
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Structure Point
        Public x As Int32
        Public y As Int32

        Public Sub New(x As Int32, y As Int32)
            Me.x = x
            Me.y = y
        End Sub
    End Structure
End Class