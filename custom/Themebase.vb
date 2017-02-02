#Region "Imports"
Imports System.Drawing.Drawing2D
Imports System.ComponentModel
#End Region



#Region "Themebase"
MustInherit Class customControl
    Inherits Control


#Region " Initialization "

    Protected G As Graphics, B As Bitmap

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)

        _ImageSize = Size.Empty
        Font = New Font("Segoe UI", 9S)

        upcust()
    End Sub

    Protected NotOverridable Overrides Sub OnHandleCreated(ByVal e As EventArgs)

        If Not _LockWidth = 0 Then Width = _LockWidth
        If Not _LockHeight = 0 Then Height = _LockHeight

        Transparent = _Transparent
        If _Transparent AndAlso _BackColor Then BackColor = Color.Transparent

        addAnimatedcontrol(Me)

        MyBase.OnHandleCreated(e)
    End Sub

    Private DoneCreation As Boolean
    Protected NotOverridable Overrides Sub OnParentChanged(ByVal e As EventArgs)
        If Parent IsNot Nothing Then
            OnCreation()

            DoneCreation = True
            upcust()
        End If
        MyBase.OnParentChanged(e)
    End Sub

#End Region

#Region "Paint"
    Protected NotOverridable Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        If Width = 0 OrElse Height = 0 Then Return

        If _Transparent Then
            PaintHook()
            e.Graphics.DrawImage(B, 0, 0)
        Else
            G = e.Graphics
            PaintHook()
        End If
    End Sub
    Friend Function retbit() As Bitmap
        If Width = 0 OrElse Height = 0 Then Return New Bitmap(0, 0)
        PaintHook()
        Return B
    End Function

#End Region

#Region " Size Handling "

    Protected NotOverridable Overrides Sub OnSizeChanged(ByVal e As EventArgs)
        If _Transparent Then
            InvalidateBitmap()
        End If

        Invalidate()
        MyBase.OnSizeChanged(e)
    End Sub

    Protected Overrides Sub SetBoundsCore(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal specified As BoundsSpecified)
        If Not _LockWidth = 0 Then width = _LockWidth
        If Not _LockHeight = 0 Then height = _LockHeight
        MyBase.SetBoundsCore(x, y, width, height, specified)
    End Sub

#End Region

#Region " State Handling "

    Private InPosition As Boolean
    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        InPosition = True
        SetState(MouseState.Over)
        For Each c As Control In Parent.Controls
            If TypeOf c Is customControl Then
                DirectCast(c, customControl).leavemouse(e)
                c.Invalidate()
            End If
        Next
        MyBase.OnMouseEnter(e)
    End Sub
    Public Sub leavemouse(e As EventArgs)
        OnMouseLeave(e)
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        If InPosition Then SetState(MouseState.Over)
        MyBase.OnMouseUp(e)
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        If e.Button = MouseButtons.Left Then SetState(MouseState.Down)
        MyBase.OnMouseDown(e)
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        InPosition = False
        SetState(MouseState.None)
        MyBase.OnMouseLeave(e)
    End Sub
    Protected Overrides Sub OnEnabledChanged(ByVal e As EventArgs)
        If Enabled Then SetState(MouseState.None) Else SetState(MouseState.Block)
        MyBase.OnEnabledChanged(e)
    End Sub
    Protected State As MouseState
    Private Sub SetState(ByVal current As MouseState)
        State = current
        Invalidate()
    End Sub

#End Region

#Region "Props"
#Region " Base Properties "

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Overrides Property BackgroundImage() As Image
        Get
            Return Nothing
        End Get
        Set(ByVal value As Image)
        End Set
    End Property
    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Overrides Property BackgroundImageLayout() As ImageLayout
        Get
            Return ImageLayout.None
        End Get
        Set(ByVal value As ImageLayout)
        End Set
    End Property

    Overrides Property Text() As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            MyBase.Text = value
            Invalidate()
            upcust()
        End Set
    End Property
    Overrides Property Font() As Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As Font)
            MyBase.Font = value
            Invalidate()
            upcust()

        End Set
    End Property

    Private _BackColor As Boolean : <Category("Appearance")> Overrides Property BackColor() As Color
        Get
            Return MyBase.BackColor
        End Get
        Set(ByVal value As Color)
            If Not IsHandleCreated AndAlso value = Color.Transparent Then
                _BackColor = True
                Return
            End If
            MyBase.BackColor = value
            Invalidate()
            upcust()
        End Set
    End Property

#End Region

#Region " Public Properties "

    Public ReadOnly Property designing As Boolean
        Get
            Return DesignMode
        End Get
    End Property
    Public Property animating As Boolean


    Private _Image As Image
    Property Image() As Image
        Get
            Return _Image
        End Get
        Set(ByVal value As Image)
            If value Is Nothing Then
                _ImageSize = Size.Empty
            Else
                _ImageSize = value.Size
            End If

            _Image = value
            Invalidate()
        End Set
    End Property

    Private _Transparent As Boolean
    Property Transparent() As Boolean
        Get
            Return _Transparent
        End Get
        Set(ByVal value As Boolean)
            _Transparent = value
            If Not IsHandleCreated Then Return

            If Not value AndAlso Not BackColor.A = 255 Then
                Throw New Exception("Unable to change value to false while a transparent BackColor is in use.")
            End If

            SetStyle(ControlStyles.Opaque, Not value)
            SetStyle(ControlStyles.SupportsTransparentBackColor, value)

            If value Then InvalidateBitmap() Else B = Nothing
            Invalidate()
        End Set
    End Property


    Private cstmstr$ : <Category("Design")> Public Property Customization As String
        Get
            Return cstmstr
        End Get
        Set(value As String)
            cstmstr = value
            Dim s(17) As String
            s = cstmstr.Split(",")

            Width = Val(s(0)) : Height = Val(s(1))
            BackColor = col(Val(s(2)), Val(s(3)), Val(s(4)), Val(s(5)))
            ForeColor = col(Val(s(6)), Val(s(7)), Val(s(8)), Val(s(9)))
            Text = s(10)

            Dim b As Boolean = False : Dim u As Boolean = False : Dim i As Boolean = False : Dim st As Boolean = False
            If s(13) = "True" Then b = True
            If s(14) = "True" Then i = True
            If s(15) = "True" Then st = True
            If s(16) = "True" Then u = True


            Font = New Font(s(11), Val(s(12)))

            Invalidate()
        End Set
    End Property


#End Region

#Region " Private Properties "

    Private _ImageSize As Size
    Protected ReadOnly Property ImageSize() As Size
        Get
            Return _ImageSize
        End Get
    End Property

    Private _LockWidth As Integer
    Protected Property LockWidth() As Integer
        Get
            Return _LockWidth
        End Get
        Set(ByVal value As Integer)
            _LockWidth = value
            If Not LockWidth = 0 AndAlso IsHandleCreated Then Width = LockWidth
        End Set
    End Property

    Private _LockHeight As Integer
    Protected Property LockHeight() As Integer
        Get
            Return _LockHeight
        End Get
        Set(ByVal value As Integer)
            _LockHeight = value
            If Not LockHeight = 0 AndAlso IsHandleCreated Then Height = LockHeight
        End Set
    End Property


#End Region


#Region " Property Helpers "
    Protected Overridable Sub InvalidateBitmap()
        If Width = 0 OrElse Height = 0 Then Return
        B = New Bitmap(Width, Height, Imaging.PixelFormat.Format32bppPArgb)
        G = Graphics.FromImage(B)
    End Sub

#End Region

#End Region

#Region " User Hooks "

    Protected MustOverride Sub PaintHook()

    Protected Overridable Sub OnCreation()
    End Sub

    Private Sub upcust()
        cstmstr = CStr(Width) + "," + CStr(Height) + "," +
                  CStr(BackColor.A) + "," + CStr(BackColor.R) + "," + CStr(BackColor.G) + "," + CStr(BackColor.B) + "," +
                  CStr(ForeColor.A) + "," + CStr(ForeColor.R) + "," + CStr(ForeColor.G) + "," + CStr(ForeColor.B) + "," +
                  CStr(Text) + "," +
                  CStr(Font.FontFamily.Name) + "," + CStr(Font.Size) + "," + CStr(Font.Bold) + "," + CStr(Font.Italic) + "," + CStr(Font.Strikeout) + "," + CStr(Font.Underline)
    End Sub


#End Region

#Region "Fn"
#Region " Measure "

    Private MeasureBitmap As Bitmap
    Private MeasureGraphics As Graphics

    Protected Function Measure() As Size
        SyncLock MeasureGraphics
            Return MeasureGraphics.MeasureString(Text, Font, Width).ToSize
        End SyncLock
    End Function
    Protected Function Measure(ByVal text As String) As Size
        SyncLock MeasureGraphics
            Return MeasureGraphics.MeasureString(text, Font, Width).ToSize
        End SyncLock
    End Function

#End Region
#Region " DrawText "

    Private DrawTextPoint As Point
    Private DrawTextSize As Size

    Protected Sub DrawText(ByVal b1 As Brush, ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer)
        DrawText(b1, Text, a, x, y)
    End Sub
    Protected Sub DrawText(ByVal b1 As Brush, ByVal text As String, ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer)
        If text.Length = 0 Then Return

        DrawTextSize = Measure(text)
        DrawTextPoint = New Point(Width \ 2 - DrawTextSize.Width \ 2, Height \ 2 - DrawTextSize.Height \ 2)

        Select Case a
            Case HorizontalAlignment.Left
                G.DrawString(text, Font, b1, x, DrawTextPoint.Y + y)
            Case HorizontalAlignment.Center
                G.DrawString(text, Font, b1, DrawTextPoint.X + x, DrawTextPoint.Y + y)
            Case HorizontalAlignment.Right
                G.DrawString(text, Font, b1, Width - DrawTextSize.Width - x, DrawTextPoint.Y + y)
        End Select
    End Sub

    Protected Sub DrawText(ByVal b1 As Brush, ByVal p1 As Point)
        If Text.Length = 0 Then Return
        G.DrawString(Text, Font, b1, p1)
    End Sub
    Protected Sub DrawText(ByVal b1 As Brush, ByVal x As Integer, ByVal y As Integer)
        If Text.Length = 0 Then Return
        G.DrawString(Text, Font, b1, x, y)
    End Sub

#End Region
#Region " CreateRound "

    Private CreateRoundPath As GraphicsPath
    Private CreateRoundRectangle As Rectangle

    Function CreateRound(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal slope As Integer) As GraphicsPath
        CreateRoundRectangle = New Rectangle(x, y, width, height)
        Return CreateRound(CreateRoundRectangle, slope)
    End Function

    Function CreateRound(ByVal r As Rectangle, ByVal slope As Integer) As GraphicsPath
        CreateRoundPath = New GraphicsPath(FillMode.Winding)
        CreateRoundPath.AddArc(r.X, r.Y, slope, slope, 180.0F, 90.0F)
        CreateRoundPath.AddArc(r.Right - slope, r.Y, slope, slope, 270.0F, 90.0F)
        CreateRoundPath.AddArc(r.Right - slope, r.Bottom - slope, slope, slope, 0.0F, 90.0F)
        CreateRoundPath.AddArc(r.X, r.Bottom - slope, slope, slope, 90.0F, 90.0F)
        CreateRoundPath.CloseFigure()
        Return CreateRoundPath
    End Function

#End Region
#End Region

#Region "Enum"
    Public Enum MouseState As Byte
        None = 0
        Over = 1
        Down = 2
        Block = 3
    End Enum

    ''' <summary>
    ''' The different types of Interpolation.
    ''' </summary>
    Public Enum Type
        ''' <summary>
        ''' Start at full speed, and then retard.
        ''' </summary>
        EaseIn
        ''' <summary>
        ''' Start at zero speed, and then accelerate.
        ''' </summary>
        EaseOut
        ''' <summary>
        ''' Retard during the first half of the motion and then accelerate during the second half.
        ''' </summary>
        EaseInOut
        ''' <summary>
        ''' Complete the motion with no change in speed.
        ''' </summary>
        Linear
        ''' <summary>
        ''' Represents motion according to the CatmullRom spline.
        ''' 2 points, Pt1 and Pt2 are specifyied to represent the 2 tangents to the curve.
        ''' </summary>
        CatmullRom
        ''' <summary>
        ''' Generates a curve with slope approaching zero near the start and end points, and approaching 1 near the middle, resulting in a smooth, natural motion.
        ''' </summary>
        SmoothStep
        ''' <summary>
        ''' A smoother version of the SmoothStep Interpolation Type, with the curve flatter at the ends.
        ''' </summary>
        Smootherstep
    End Enum

    ''' <summary>
    ''' The different methods of easing for easing Interpolations.
    ''' </summary>
    Public Enum EasingMethods
        ''' <summary>
        ''' Regualar easing. The motion depends upon the power specifyied and corresponds to the curve of degree equal to the given power.
        ''' </summary>
        Regular
        ''' <summary>
        ''' The motion corresponds to the curve of 2 raised to the power 10(x-1). Exponential curve has a lot of curvature and thus results in a sudden change in value as the slope approaches infinity near the end.
        ''' </summary>
        Exponent
        ''' <summary>
        ''' Motion corresponds to a circular plot, resulting in a more 'sudden' change in values near the mid point.
        ''' </summary>
        Circular
        ''' <summary>
        ''' A gentle easing is apllied, based on the sinusoidal curve. Slope approaches 1 for most of the part of the motion.
        ''' </summary>
        Sine
        ''' <summary>
        ''' Motion resembles that of bouncing ball, reversing back a little distance near the ends.
        ''' </summary>
        Bounce
        ''' <summary>
        ''' Causes the motion to go beyond the Ending Point and then return back. Note that a similar effect can be achieved using the CatmullRom Interpolation technique with values of Pt1 and Pt2 equal to -10 and 0.
        ''' </summary>
        Jumpback
        ''' <summary>
        ''' Resembles to the motion of an elastic band, stretched and then released. Slope alternatively approaches -infinity and +infinity.
        ''' </summary>
        Elastic
        ''' <summary>
        ''' Motion represents forced stopping of a heavy object moving with high velocity. Slope approaches infinity near the start and zero near the ends.
        ''' </summary>
        Critical_Damping
        '		Flash
    End Enum
#End Region

    Protected Overrides Sub Dispose(disposing As Boolean)
        MyBase.Dispose(disposing)
        animatedcontrols.Remove(Me)
    End Sub


#Region "Animation"
    Private Function GetValue(StartV#, EndV#, Time#, Duration#, IType As Type, Method As EasingMethods, Power#, Q#, R#) As Single
        Dim i As New Interpolation
        Return i.GetValue(StartV, EndV, Time, Duration, IType, Method, Power, Q, R)
    End Function
    Public Function GetValue(StartV#, EndV#, Time#, Duration#, Q#, R#) As Single
        Return GetValue(StartV, EndV, Time, Duration, Interpolation.Type.CatmullRom, Interpolation.EasingMethods.Regular, 1, Q, R)
    End Function
    Public Function GetValue(StartV#, EndV#, Time#, Duration#, IType As Interpolation.Type, Power#) As Single
        Return GetValue(StartV, EndV, Time, Duration, IType, Interpolation.EasingMethods.Regular, Power, 0, 0)
    End Function
    Public Function GetValue(StartV#, EndV#, Time#, Duration#, IType As Interpolation.Type, Method As Interpolation.EasingMethods, Power#) As Single
        Return GetValue(StartV, EndV, Time, Duration, IType, Method, Power, 0, 0)
    End Function

    Public Shared Function blend(ParamArray Colors() As Color) As Color
        Dim c As Color
        Dim r% = 0
        Dim b% = 0
        Dim g% = 0
        Dim n% = 0
        Dim a% = 0
        For Each col As Color In Colors
            n += 1
            r += col.R
            g += col.G
            b += col.B
            a += col.A
        Next
        r /= n
        g /= n
        b /= n
        a /= n
        c = Color.FromArgb(a, r, g, b)
        Return c
    End Function
#End Region
End Class
#End Region

