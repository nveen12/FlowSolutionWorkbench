Option Explicit On
Option Strict On

Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Drawing.Bitmap
Imports System.Drawing.Graphics
Imports System.IO

Partial Public Class clsBarcodeRendering
    'Inherits System.Web.UI.Page

    'http://www.atalasoft.com/cs/blogs/loufranco/archive/2008/04/25/writing-code-39-barcodes-with-javascript.aspx-generation.aspx
    'With a bit of this thrown in:
    'http://www.atalasoft.com/cs/blogs/loufranco/archive/2008/03/24/code-39-barcode

    Private _encoding As Hashtable = New Hashtable
    Private Const _wideBarWidth As Short = 8
    Private Const _narrowBarWidth As Short = 2
    Private Const _barHeight As Short = 100

    Public Sub New(ByVal barcode As String)

        Me.BarcodeCode39()
    End Sub


    Sub BarcodeCode39()
        _encoding.Add("*", "bWbwBwBwb")
        _encoding.Add("-", "bWbwbwBwB")
        _encoding.Add("$", "bWbWbWbwb")
        _encoding.Add("%", "bwbWbWbWb")
        _encoding.Add(" ", "bWBwbwBwb")
        _encoding.Add(".", "BWbwbwBwb")
        _encoding.Add("/", "bWbWbwbWb")
        _encoding.Add("+", "bWbwbWbWb")
        _encoding.Add("0", "bwbWBwBwb")
        _encoding.Add("1", "BwbWbwbwB")
        _encoding.Add("2", "bwBWbwbwB")
        _encoding.Add("3", "BwBWbwbwb")
        _encoding.Add("4", "bwbWBwbwB")
        _encoding.Add("5", "BwbWBwbwb")
        _encoding.Add("6", "bwBWBwbwb")
        _encoding.Add("7", "bwbWbwBwB")
        _encoding.Add("8", "BwbWbwBwb")
        _encoding.Add("9", "bwBWbwBwb")
        _encoding.Add("A", "BwbwbWbwB")
        _encoding.Add("B", "bwBwbWbwB")
        _encoding.Add("C", "BwBwbWbwb")
        _encoding.Add("D", "bwbwBWbwB")
        _encoding.Add("E", "BwbwBWbwb")
        _encoding.Add("F", "bwBwBWbwb")
        _encoding.Add("G", "bwbwbWBwB")
        _encoding.Add("H", "BwbwbWBwb")
        _encoding.Add("I", "bwBwbWBwb")
        _encoding.Add("J", "bwbwBWBwb")
        _encoding.Add("K", "BwbwbwbWB")
        _encoding.Add("L", "bwBwbwbWB")
        _encoding.Add("M", "BwBwbwbWb")
        _encoding.Add("N", "bwbwBwbWB")
        _encoding.Add("O", "BwbwBwbWb")
        _encoding.Add("P", "bwBwBwbWb")
        _encoding.Add("Q", "bwbwbwBWB")
        _encoding.Add("R", "BwbwbwBWb")
        _encoding.Add("S", "bwBwbwBWb")
        _encoding.Add("T", "bwbwBwBWb")
        _encoding.Add("U", "BWbwbwbwB")
        _encoding.Add("V", "bWBwbwbwB")
        _encoding.Add("W", "BWBwbwbwb")
        _encoding.Add("X", "bWbwBwbwB")
        _encoding.Add("Y", "BWbwBwbwb")
        _encoding.Add("Z", "bWBwBwbwb")
    End Sub



    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    'BarcodeCode39()
    '    Dim barcode As String = String.Empty
    '    'If Not IsNothing(Request("barcode")) AndAlso Not (Request("barcode").Length = 0) Then
    '    '       barcode = Request("barcode")
    '    '      Response.ContentType = "image/png"
    '    '     Response.AddHeader("Content-Disposition", String.Format("attachment; filename=barcode_{0}.png", barcode))

    '    'TODO: Depending on the length of the string, determine how wide the image will be
    '    '    GenerateBarcodeImage(250, 140, barcode).WriteTo(Response.OutputStream)
    '    'End If

    'End Sub

    Protected Function getBCSymbolColor(ByVal symbol As String) As System.Drawing.Brush
        getBCSymbolColor = Brushes.Black
        If symbol = "W" Or symbol = "w" Then
            getBCSymbolColor = Brushes.White
        End If
    End Function

    Protected Function getBCSymbolWidth(ByVal symbol As String) As Short
        getBCSymbolWidth = _narrowBarWidth
        If symbol = "B" Or symbol = "W" Then
            getBCSymbolWidth = _wideBarWidth
        End If
    End Function

    Public Function GenerateBarcodeImage(ByVal imageWidth As Short, ByVal imageHeight As Short, ByVal Code As String) As Bitmap

        'create a new bitmap
        Dim b As New Bitmap(imageWidth, imageHeight, Imaging.PixelFormat.Format32bppArgb)

        'create a canvas to paint on
        Dim canvas As New Rectangle(0, 0, imageWidth, imageHeight)

        'draw a white background
        Dim g As Graphics = Graphics.FromImage(b)
        g.FillRectangle(Brushes.White, 0, 0, imageWidth, imageHeight)

        'write the unaltered code at the bottom
        'TODO: truely center this text
        Dim textBrush As New SolidBrush(Color.Black)
        g.DrawString(Code, New Font("Courier New", 12), textBrush, 100, 110)

        'Code has to be surrounded by asterisks to make it a valid Code39 barcode
        Dim UseCode As String = String.Format("{0}{1}{0}", "*", Code)

        'Start drawing at 10, 10
        Dim XPosition As Short = 10
        Dim YPosition As Short = 10

        Dim invalidCharacter As Boolean = False
        Dim CurrentSymbol As String = String.Empty

        For j As Short = 0 To CShort(UseCode.Length - 1)
            CurrentSymbol = UseCode.Substring(j, 1)
            'check if symbol can be used
            If Not IsNothing(_encoding(CurrentSymbol)) Then
                Dim EncodedSymbol As String = _encoding(CurrentSymbol).ToString

                For i As Short = 0 To CShort(EncodedSymbol.Length - 1)
                    Dim CurrentCode As String = EncodedSymbol.Substring(i, 1)
                    g.FillRectangle(getBCSymbolColor(CurrentCode), XPosition, YPosition, getBCSymbolWidth(CurrentCode), _barHeight)
                    XPosition = XPosition + getBCSymbolWidth(CurrentCode)
                Next

                'After each written full symbol we need a whitespace (narrow width)
                g.FillRectangle(getBCSymbolColor("w"), XPosition, YPosition, getBCSymbolWidth("w"), _barHeight)
                XPosition = XPosition + getBCSymbolWidth("w")
            Else
                invalidCharacter = True
            End If
        Next

        'errorhandling when an invalidcharacter is found
        If invalidCharacter Then
            g.FillRectangle(Brushes.White, 0, 0, imageWidth, imageHeight)
            g.DrawString("Invalid characters found,", New Font("Courier New", 8), textBrush, 0, 0)
            g.DrawString("no barcode generated", New Font("Courier New", 8), textBrush, 0, 10)
            g.DrawString("Input was: ", New Font("Courier New", 8), textBrush, 0, 30)
            g.DrawString(Code, New Font("Courier New", 8), textBrush, 0, 40)
        End If

        'write the image into a memorystream
        Dim ms As New MemoryStream

        Dim encodingParams As New EncoderParameters
        encodingParams.Param(0) = New EncoderParameter(Encoder.Quality, 100)

        Dim encodingInfo As ImageCodecInfo = FindCodecInfo("PNG")

        'b.Save(ms, encodingInfo, encodingParams)

        'dispose of the object we won't need any more
        g.Dispose()
        'b.Dispose()

        Return b
    End Function

    Protected Overridable Function FindCodecInfo(ByVal codec As String) As ImageCodecInfo
        Dim encoders As ImageCodecInfo() = ImageCodecInfo.GetImageEncoders
        For Each e As ImageCodecInfo In encoders
            If e.FormatDescription.Equals(codec) Then Return e
        Next
        Return Nothing
    End Function
End Class

