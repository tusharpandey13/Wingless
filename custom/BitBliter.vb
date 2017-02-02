Public Class BitBliter
    'by Fade (Amit BS) [codeproject]
#Region " BitBlt Declaration"
    Private Declare Auto Function BitBlt Lib "GDI32.DLL" (
        ByVal hdcDest As IntPtr,
        ByVal nXDest As Integer,
        ByVal nYDest As Integer,
        ByVal nWidth As Integer,
        ByVal nHeight As Integer,
        ByVal hdcSrc As IntPtr,
        ByVal nXSrc As Integer,
        ByVal nYSrc As Integer,
        ByVal dwRop As Int32) As Boolean
#End Region

#Region " Members "

    ' Public Holders
    Private MemGrp As Graphics
    Private MemHdc As IntPtr

    Private LastSize As Size

    Private Copied As Boolean

#End Region

#Region " Interface "
    'Interface

    Public Sub Copy(ByVal srcGraphics As Graphics, ByVal Size As Size)
        LastSize = Size ' Saving so we'l know how to Paste
        ' check if already copied
        If Copied Then DisposeObjects() ' so old objects are disposed
        ' Creating a temporary Bitmap to create objects of
        Dim srcBmp As New Bitmap(Size.Width, Size.Height, srcGraphics)
        ' Creating Objects
        MemGrp = Graphics.FromImage(srcBmp)     'Create a Graphics object 
        MemHdc = MemGrp.GetHdc                  'Get the Device Context

        '>>> get the picture <<<
        MyBitBlt(srcGraphics, MemHdc, Size.Width, Size.Height)
        ' Dispose of the BMP
        srcBmp.Dispose()

        Copied = True
    End Sub

    Public Sub Paste(ByVal TargetGraphics As Graphics, ByVal X As Integer, ByVal Y As Integer)
        Dim TargetHdc As IntPtr = TargetGraphics.GetHdc

        MyBitBlt(MemHdc, TargetHdc, LastSize.Width, LastSize.Height, X, Y)

        TargetGraphics.ReleaseHdc(TargetHdc)
    End Sub

#End Region

#Region " Internals "
    Const SRCCOPY As Integer = &HCC0020
    ' Wraping things up
    Private Sub MyBitBlt(ByVal SourceGraphics As Graphics, ByVal TargetHDC As IntPtr, ByVal width As Integer, ByVal Height As Integer)
        ' Creating a DeviceContext to capture from
        Dim SourceHDC As IntPtr = SourceGraphics.GetHdc
        ' Blitting (Copying) the data
        BitBlt(TargetHDC, 0, 0, width, Height, SourceHDC, 0, 0, SRCCOPY)
        ' Releasing the Device context used
        SourceGraphics.ReleaseHdc(SourceHDC)
    End Sub
    Private Sub MyBitBlt(ByVal SourceHDC As IntPtr, ByVal TargetHDC As IntPtr, ByVal width As Integer, ByVal Height As Integer, ByVal PosX As Integer, ByVal PosY As Integer)
        ' Copying data to a specific position on the target Device Context
        BitBlt(TargetHDC, PosX, PosY, width, Height, SourceHDC, 0, 0, SRCCOPY)
    End Sub

    ' Cleanning Up
    ' Before Destroying this class, dispose of objects to reclaim memory
    Protected Overrides Sub Finalize()
        DisposeObjects()
        MyBase.Finalize()
    End Sub

    ' Disposing of objects
    Private Sub DisposeObjects()
        MemGrp.ReleaseHdc(MemHdc) ' Release Device Context
        MemGrp.Dispose() ' Disposing of Graphics object
    End Sub

#End Region
End Class
