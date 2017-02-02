
Module timer
    Public hasTimerStarted As Boolean = 0
    Public T% = 0
    Private WithEvents tmr As Multimedia.Timer
    Friend animatedforms As New List(Of Form)
    Friend animatedcontrols As New List(Of customControl)
    Public indesignmode As Boolean = 0

    Public Sub startTimer()
        If hasTimerStarted = 0 Then
            tmr = New Multimedia.Timer With {.Period = 1, .Resolution = 1, .Mode = 1}
            tmr.Start()
            hasTimerStarted = 1
        End If
    End Sub
    Public Sub stopTimer()
        If hasTimerStarted = 0 Then Return
        tmr.Stop()
        hasTimerStarted = 0
        tmr.Dispose()
    End Sub
    Public Sub addAnimatedForm(f As Form)
        If Not animatedforms.Contains(f) Then animatedforms.Add(f)
        If TypeOf f Is CustomWindow And DirectCast(f, CustomWindow).designing = True Then
            indesignmode = 1
            Return
        End If
        startTimer()
    End Sub
    Public Sub addAnimatedcontrol(c As Control)
        If Not animatedcontrols.Contains(c) Then animatedcontrols.Add(c)
        If TypeOf c Is customControl Then
            If DirectCast(c, customControl).designing Then
                indesignmode = 1
                Return
            End If
        End If
        startTimer()
    End Sub
    Public Sub removeAnimatedControl(c As Control)
        If animatedcontrols.Contains(c) Then animatedcontrols.Remove(c)
        If animatedcontrols.Count = 0 And animatedforms.Count = 0 Then stopTimer()
    End Sub
    Public Sub removeAnimatedform(c As Form)
        If animatedforms.Contains(c) Then animatedforms.Remove(c)
        If animatedcontrols.Count = 0 And animatedforms.Count = 0 Then stopTimer()
    End Sub
    Async Sub inv(c As Control)
        c.Invalidate()
    End Sub
    Async Sub tck() Handles tmr.Tick
        If indesignmode = 1 Then Return
        If T >= 1000 Then T = 0

        For Each f As Form In animatedforms
            If DirectCast(f, CustomWindow).animating Then f.Invalidate()
            If f.AccessibleDescription = "Animated Form" Then f.Invalidate()
        Next

        For Each c As customControl In animatedcontrols
            If c.animating Then inv(c)
            'If c.AccessibleDescription = "AnimateOnce" Then c.animating=false
        Next
        T += 1
    End Sub

End Module

