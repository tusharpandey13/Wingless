Imports System.Drawing.Drawing2D
Imports System.ComponentModel

Class custom_Numeric : Inherits customControl
#Region "declare"
    Dim tp As Pen : Dim tb As SolidBrush : Dim tgb As LinearGradientBrush : Dim tbt As Bitmap
    Dim t%, v!
    Dim sd% = 0 : Dim mpos% = 0
    Dim edt% = 0
    Dim ofst! = 0
#End Region

#Region "props"
    Dim _val% : <Browsable(True), EditorBrowsable(EditorBrowsableState.Always), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(0)> Property Value%
        Get
            Return _val
        End Get
        Set(v%)
            _val = v
            Invalidate()
        End Set
    End Property
    Dim _neg As Boolean : <Category("Behaviour"), DefaultValue(0)> Property AllowNegative As Boolean
        Get
            Return _neg
        End Get
        Set(value As Boolean)
            _neg = value
            Invalidate()
        End Set
    End Property
    Dim min%, max%
    <Category("Data")> Property Minimum%
        Get
            Return min
        End Get
        Set(value%)
            min = value
        End Set
    End Property
    <Category("Data")> Property Maximum%
        Get
            Return max
        End Get
        Set(value%)
            max = value
        End Set
    End Property

#End Region
    Sub New()
        animating = False
        LockHeight = 60
        Width = 30
        Font = New Font("Segoe UI", 16)
    End Sub




    Protected Overrides Sub PaintHook()
        calc()
        G.Clear(Parent.BackColor)
        G.SmoothingMode = 2 : G.InterpolationMode = 7 : G.PixelOffsetMode = 2
        If t = 0 Then
            drawdial()
            G.DrawImageUnscaled(tbt, pt(0, -Height / 2))
        Else
            G.DrawImageUnscaled(tbt, pt(0, -Height / 2 + ofst))
        End If

        tgb = New LinearGradientBrush(rct(0, 0, Width, 18), Parent.BackColor, col(0, 0), 90.0F)
        G.FillRectangle(tgb, rct(0, 0, Width, 18))
        tgb = New LinearGradientBrush(rct(0, Height - 18, Width, 18), col(0, 0), Parent.BackColor, 90.0F)
        G.FillRectangle(tgb, rct(0, Height - 18, Width, 18))



        tgb.Dispose()
    End Sub

    Sub drawdial()
        tbt = New Bitmap(Width, 2 * Height) : Dim tg As Graphics = Graphics.FromImage(tbt)

        tg.SmoothingMode = 2 : tg.PixelOffsetMode = 2 : tg.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
        tg.Clear(BackColor)
        tg.DrawString(_val, Font, mb(ForeColor), pt(Width / 2, Height / 2), CenterSF)

        If Not _val = max Then
            tg.DrawString(_val + 1, Font, mb(ForeColor), pt(Width / 2, Height - 4.5 - Height / 2), CenterSF)
            tg.DrawString(_val + 2, Font, mb(ForeColor), pt(Width / 2, Height - 4.5), CenterSF)
        End If

        If Not _val = min Then
            If _val = 0 Then
                If _neg Then
                    tg.DrawString(_val - 1, Font, mb(ForeColor), pt(Width / 2, 4.5 + Height / 2), CenterSF)
                    tg.DrawString(_val - 2, Font, mb(ForeColor), pt(Width / 2, 4.5), CenterSF)
                End If
            Else
                tg.DrawString(_val - 1, Font, mb(ForeColor), pt(Width / 2, 4.5 + Height / 2), CenterSF)
                tg.DrawString(_val - 2, Font, mb(ForeColor), pt(Width / 2, 4.5), CenterSF)
            End If

        End If
    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        If e.Y < 21 Then
            mpos = -1
        ElseIf e.Y > Height - 21 Then
            mpos = 1
        Else
            mpos = 0
        End If
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        If e.Y < 21 Then
            sd = -1
            edt = 0
        ElseIf e.Y > Height - 21 Then
            sd = 1
            edt = 0
        Else
            sd = 0
            edt = 1
        End If
        animating = True
    End Sub
    Sub calc()
        Dim dst!
        If sd = -1 Then dst = 30
        If sd = 1 Then dst = -30
        If t < 500 And animating then
            ofst = GetValue(0, dst, t, 500, Interpolation.Type.SmoothStep, 2)
        Else
            animating = False
            t = 0
            sd = 0
        End If
    End Sub

End Class
