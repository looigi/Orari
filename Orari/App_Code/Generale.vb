Imports System.Drawing
Imports System.IO

Module Generale
    Public Password As String
    Public UtenzaEntrata As String
    Public Entrato As Boolean
    Public NomeFileMese As String

    Public NomeMesi(12) As String
    Public NumeroMese As Integer
    Public NumeroAnno As Integer

    Public blackBrush As New SolidBrush(Color.Black)
    Public blueBrush As New SolidBrush(Color.Blue)
    Public redBrush As New SolidBrush(Color.Red)
    Public greenBrush As New SolidBrush(Color.DarkGreen)
    Public yellowBrush As New SolidBrush(Color.FromArgb(255, 255, 153)) '#FFFF99
    Public grayBrush As New SolidBrush(Color.LightGray)
    Public transparentBrush As New SolidBrush(Color.Transparent)

    Public drawFontGrosso As Font = New Font("Arial", 16, FontStyle.Bold)
    Public drawFontMedio As Font = New Font("Courier New", 13, FontStyle.Bold)
    Public drawFontPiccolo As Font = New Font("Verdana", 11, FontStyle.Italic)
    Public drawFontPiccolissimo As Font = New Font("Verdana", 9, FontStyle.Italic)
    Public drawFontCorsivo As Font = New Font("Verdana", 10, FontStyle.Italic)
    Public drawFontCorsivoGrande As Font = New Font("Verdana", 12, FontStyle.Italic)

    Public Nero As Pen = Pens.Black
    Public Rosso As Pen = Pens.Red
    Public Verde As Pen = Pens.Green
    Public Grigio As Pen = Pens.LightGray

    Public idRigaAnd() As Integer
    Public QuanteRigheAndata As Integer
    Public idRigaRit() As Integer
    Public QuanteRigheRitorno As Integer
    Public idRigaPortata() As Integer
    Public QuanteRighePortata As Integer

    Public OreStandard As Single

    Public idLavoroDefault As Integer
    Public CommessaDefault As String

    Public idUtente As Integer
    Public Utenza As String

    Public CosaSelezionato As String
    Public idSelezionato As String

    Public ModalitaStatistica As String = ""
    Public SocietaSel As String = ""
    Public GiornoSel As String = ""
    Public MeseSel As String = ""

    Public MeseVisualizzato As Boolean = False
    Public PrefissoTabelle As String = ""

    Dim PuntoVirgola As String = ","

    Public Function ScriveNumero(ByVal Numero As String) As String
        Dim Ritorno As String = Numero '.ToString("R")
        If Ritorno = "" Then
            Return ""
        End If

        Dim A As Integer

        'Ritorno = Numero.ToString.Trim
        ' A = InStr(Ritorno, ",")
        A = InStr(Ritorno, PuntoVirgola)

        Dim PrimaParte As Single
        If A > 0 Then
            PrimaParte = Mid(Ritorno, 1, A - 1)
        Else
            PrimaParte = Ritorno
        End If
        Dim sPrimaParte As String = PrimaParte.ToString.Trim
        Dim sPrimo As String = ""
        Dim Contatore As Integer = 0

        For i As Integer = sPrimaParte.Length To 1 Step -1
            Contatore += 1
            If Contatore = 4 Then
                Contatore = 0
                sPrimo += "." & Mid(sPrimaParte, i, 1)
            Else
                sPrimo += Mid(sPrimaParte, i, 1)
            End If
        Next
        sPrimaParte = ""
        For i As Integer = sPrimo.Length To 1 Step -1
            sPrimaParte += Mid(sPrimo, i, 1)
        Next

        Dim sSecondaParte As String = "00"

        If A > 0 Then
            sSecondaParte = Mid(Ritorno, A + 1, 2)
            'sSecondaParte = CInt(SecondaParte).ToString.Trim
            If sSecondaParte.Length = 1 Then
                sSecondaParte += "0"
            Else
                If sSecondaParte.Length > 2 Then
                    sSecondaParte = Mid(sSecondaParte, 1, 2)
                End If
            End If
        End If

        Ritorno = sPrimaParte & "," & sSecondaParte

        Return Ritorno
    End Function

    Public Function CreaStringaPerRefresh() As String
        Dim Stringa As String = ""
        Dim Lettere As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ "
        Dim x As Integer
        Dim y As Integer

        Randomize()
        x = Int(Rnd(1) * 7) + 2
        For i As Integer = 1 To x
            Randomize()
            y = Int(Rnd(1) * Lettere.Length - 1) + 1
            If y = 0 Then y = 1
            Stringa += Mid(Lettere, y, 1)
        Next

        Return Stringa
    End Function

    Public Function ContaGiorniLavorativi(NumMese As Integer, NumAnno As Integer) As String
        Dim QuantiTotali As Integer = Date.DaysInMonth(NumAnno, NumMese)
        Dim Quanti As Integer = 0
        Dim Datella As Date
        Dim Settimane As Integer = 0

        For i As Integer = 1 To QuantiTotali
            Datella = i & "/" & NumMese & "/" & NumAnno
            If ControllaFestivo(Datella) = False Then
                Quanti += 1
            End If
        Next
        Settimane = (Quanti / 5) ' + 1

        Return Quanti & "/" & QuantiTotali & "*" & "Settimane: " & Settimane
    End Function

    Public Function MetteMaiuscoleDopoPunto(Cosa As String) As String
        Dim Ritorno As String = Cosa.ToLower.Trim

        If Ritorno <> "" Then
            If Asc(Mid(Ritorno, 1, 1)) >= Asc("a") And Asc(Mid(Ritorno, 1, 1)) <= Asc("z") Then
                Ritorno = Chr(Asc(Mid(Ritorno, 1, 1)) - 32) & Mid(Ritorno, 2, Len(Ritorno))
            End If
            Ritorno = Ritorno.Replace("  ", " ")
            Ritorno = Ritorno.Replace(". ", ".")
            For i As Integer = 2 To Len(Ritorno)
                If Mid(Ritorno, i, 1) = "." Then
                    If i + 1 < Ritorno.Length Then
                        If Asc(Mid(Ritorno, i + 1, 1)) >= Asc("a") And Asc(Mid(Ritorno, i + 1, 1)) <= Asc("z") Then
                            Ritorno = Mid(Ritorno, 1, i) & Chr(Asc(Mid(Ritorno, i + 1, 1)) - 32) & Mid(Ritorno, i + 2, Len(Ritorno))
                        End If
                    End If
                End If
            Next
            Ritorno = Ritorno.Replace(".", ". ")
        End If

        Return Ritorno
    End Function

    Public Function MetteMaiuscole(Cosa As String) As String
        Dim Ritorno As String = Cosa.ToLower.Trim

        If Ritorno <> "" Then
            If Asc(Mid(Ritorno, 1, 1)) >= Asc("a") And Asc(Mid(Ritorno, 1, 1)) <= Asc("z") Then
                Ritorno = Chr(Asc(Mid(Ritorno, 1, 1)) - 32) & Mid(Ritorno, 2, Len(Ritorno))
            End If
            Ritorno = Ritorno.Replace("  ", " ")
            For i As Integer = 2 To Len(Ritorno)
                If Mid(Ritorno, i, 1) = " " Then
                    If Asc(Mid(Ritorno, i + 1, 1)) >= Asc("a") And Asc(Mid(Ritorno, i + 1, 1)) <= Asc("z") Then
                        Ritorno = Mid(Ritorno, 1, i) & Chr(Asc(Mid(Ritorno, i + 1, 1)) - 32) & Mid(Ritorno, i + 2, Len(Ritorno))
                    End If
                End If
            Next
        End If

        Return Ritorno
    End Function

    Public Function ControllaFestivo(Datella As Date) As Boolean
        Dim Ritorno As Boolean = False
        Dim Giorno As Integer = Datella.Day
        Dim Mese As Integer = Datella.Month

        Dim DataPasqua As Date = Pasqua(Datella.Year)
        Dim DatellaDopo As Date = DataPasqua.AddDays(1)
        Dim GiornoDopo As Integer = DatellaDopo.Day
        Dim Mesedopo As Integer = DatellaDopo.Month

        If Giorno = GiornoDopo And Mese = Mesedopo Then
            Ritorno = True
        Else
            If Datella.DayOfWeek = 0 Or Datella.DayOfWeek = 6 Then
                Ritorno = True
            Else
                If Giorno = 1 And Mese = 1 Then
                    Ritorno = True
                Else
                    If Giorno = 6 And Mese = 1 Then
                        Ritorno = True
                    Else
                        If Giorno = 25 And Mese = 4 Then
                            Ritorno = True
                        Else
                            If Giorno = 1 And Mese = 5 Then
                                Ritorno = True
                            Else
                                If Giorno = 2 And Mese = 6 Then
                                    Ritorno = True
                                Else
                                    If Giorno = 29 And Mese = 6 Then
                                        Ritorno = True
                                    Else
                                        If Giorno = 15 And Mese = 8 Then
                                            Ritorno = True
                                        Else
                                            If Giorno = 1 And Mese = 11 Then
                                                Ritorno = True
                                            Else
                                                If Giorno = 8 And Mese = 12 Then
                                                    Ritorno = True
                                                Else
                                                    If Giorno = 25 And Mese = 12 Then
                                                        Ritorno = True
                                                    Else
                                                        If Giorno = 26 And Mese = 12 Then
                                                            Ritorno = True
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If

        Return Ritorno
    End Function

    Private Function Pasqua(Anno%) As Date
        Dim A%, b%, c%, p%, q%, R%
        Dim Pasq As Date

        A = Anno% Mod 19 : b = Anno% \ 100 : c = Anno% Mod 100
        p = (19 * A + b - (b / 4) - ((b + ((b + 8) \ 25) + 1) \ 3) + 15) Mod 30
        q = (32 + 2 * ((b Mod 4) + (c \ 4)) - p - (c Mod 4)) Mod 7
        R = (p + q - 7 * ((A + 11 * p + 22 * q) \ 451) + 114)
        Pasq = DateSerial(Anno%, R \ 31, (R Mod 31) + 1)

        Return Pasqua
    End Function

    Public Sub VisualizzaMessaggioInPopup(ByVal MessaggioPopup As String, ByVal PaginaMaster As MasterPage)
        Dim Pannello As UpdatePanel = DirectCast(PaginaMaster.FindControl("uppPopup"), UpdatePanel)
        Dim divBloccaFinestra As HtmlGenericControl = DirectCast(Pannello.FindControl("divBloccaFinestra"), HtmlGenericControl)
        Dim divErrore As HtmlGenericControl = DirectCast(Pannello.FindControl("divPopup"), HtmlGenericControl)
        Dim ulPopup As HtmlGenericControl = DirectCast(Pannello.FindControl("ulPopup"), HtmlGenericControl)
        Dim Ritorno As String = ""

        Ritorno = "<li><span class=""etichettapopup"">" & MessaggioPopup & "</span></li>"
        If Left(Ritorno, 4).ToUpper.Trim <> "<LI>" Then
            Ritorno = "<li>" & Ritorno
        End If
        If Right(Ritorno, 5).ToUpper.Trim <> "</LI>" Then
            Ritorno = Ritorno & "</li>"
        End If
        ulPopup.InnerHtml = Ritorno

        divBloccaFinestra.Visible = True
        divErrore.Visible = True
    End Sub

    Public Sub ResizeImage(ByVal percorsoImmagine As String, dimeX As Integer, dimeY As Integer, Optional Destinazione As String = "")
        Dim bm As Bitmap
        'dimensioni  originali
        Dim originalX As Integer
        Dim originalY As Integer
        'dimensioni finali
        Dim destinationX As Integer
        Dim destinationY As Integer
        'carica immagine originale
        bm = New Bitmap(percorsoImmagine)
        'ricava dimensioni originali dell'immagine
        originalX = bm.Width
        originalY = bm.Height
        'imposta dimensioni finali
        destinationX = dimeX
        destinationY = dimeY
        'riduzione dell'immagine
        Dim thumb As New Bitmap(destinationX, destinationY)
        Dim g As Graphics = Graphics.FromImage(thumb)
        'impostazione del metodo di interpolazione utilizzato per il resize
        g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        g.DrawImage(bm, New Rectangle(0, 0, destinationX, destinationY), New Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel)
        g.Dispose()
        'salvataggio dell'immagine ridimensionata
        thumb.Save(percorsoImmagine & ".rsz", System.Drawing.Imaging.ImageFormat.Jpeg)
        bm.Dispose()
        thumb.Dispose()

        If Destinazione = "" Then
            Try
                Kill(percorsoImmagine)
            Catch ex As Exception

            End Try

            Rename(percorsoImmagine & ".rsz", percorsoImmagine)
        Else
            FileCopy(percorsoImmagine & ".rsz", Destinazione)

            Try
                Kill(percorsoImmagine & ".rsz")
            Catch ex As Exception

            End Try
        End If
    End Sub

    Public Sub ArrotondaIcona(ByVal percorsoImmagine As String)
        Dim bm As Bitmap
        Dim originalX As Integer
        Dim originalY As Integer

        'carica immagine originale
        bm = New Bitmap(percorsoImmagine)
        'ricava dimensioni originali dell'immagine
        originalX = bm.Width
        originalY = bm.Height

        Dim thumb As New Bitmap(originalX, originalY)
        Dim g As Graphics = Graphics.FromImage(thumb)

        g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        g.DrawImage(bm, New Rectangle(0, 0, originalX, originalY), New Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel)

        Dim dimeX As Integer = 48
        Dim dimeY As Integer = 48

        Dim r As System.Drawing.Rectangle
        r.X = (originalX / 2) - (dimeX / 2)
        r.Y = (originalY / 2) - (dimeY / 2)
        Dim s As System.Drawing.Size
        s.Width = dimeX
        s.Height = dimeY
        r.Size = s
        g.DrawEllipse(Pens.Gray, r)

        For dimeX = 49 To 753
            r.X = (originalX / 2) - (dimeX / 2)
            r.Y = (originalY / 2) - (dimeX / 2)
            s.Width = dimeX
            s.Height = dimeX
            r.Size = s
            g.DrawEllipse(Pens.Red, r)
        Next

        For dimeX = 49 To 75
            r.X = (originalX / 2) - (dimeX / 2)
            r.Y = (originalY / 2) - (dimeX / 2)
            s.Width = dimeX + 1
            s.Height = dimeX + 1
            r.Size = s
            g.DrawEllipse(Pens.Red, r)
        Next

        dimeX = 48
        dimeY = 48

        r.X = (originalX / 2) - (dimeX / 2)
        r.Y = (originalY / 2) - (dimeY / 2)
        s.Width = dimeX
        s.Height = dimeY
        r.Size = s
        g.DrawEllipse(Pens.Gray, r)

        g.Dispose()

        thumb.MakeTransparent(Color.Red)

        thumb.Save(percorsoImmagine & ".tsz", System.Drawing.Imaging.ImageFormat.Png)
        bm.Dispose()
        thumb.Dispose()

        Try
            Kill(percorsoImmagine)
        Catch ex As Exception

        End Try

        Rename(percorsoImmagine & ".tsz", percorsoImmagine)
    End Sub

    Public Sub RuotaFoto(Nome As String)
        Dim bm As Bitmap
        Dim originalX As Integer
        Dim originalY As Integer

        bm = New Bitmap(Nome)

        originalX = 100
        originalY = 100

        Dim thumb As New Bitmap(originalX + 30, originalY + 25)
        Dim g As Graphics = Graphics.FromImage(thumb)

        g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        g.RotateTransform(-12.0F)

        Dim fillRect As New Rectangle(0, 0, originalX + 30, originalY + 25)
        Dim fillRegion As New [Region](fillRect)

        g.FillRegion(Brushes.Red, fillRegion)

        g.DrawImage(bm, New Rectangle(5, 23, originalX, originalY), New Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel)
        g.DrawRectangle(Pens.DarkGray, New Rectangle(5, 23, originalX, originalY))
        g.DrawRectangle(Pens.DarkGray, New Rectangle(4, 22, originalX + 1, originalY + 1))
        thumb.MakeTransparent(Color.Red)

        thumb.Save(Nome & ".ruo", System.Drawing.Imaging.ImageFormat.Png)

        bm.Dispose()
        thumb.Dispose()

        Kill(Nome)
        Rename(Nome & ".ruo", Nome)
    End Sub

    Public Function SistemaCaratteriSpeciali(Cosa As String) As String
        Dim Cosa2 As String = Cosa

        Cosa2 = Cosa2.Replace(Chr(34), "&quot;")
        'Cosa2 = Cosa2.Replace("'", "&lsquo;")
        Cosa2 = Cosa2.Replace("&", "&amp;")
        Cosa2 = Cosa2.Replace("©", "&copy;")
        Cosa2 = Cosa2.Replace(">", "&gt;")
        Cosa2 = Cosa2.Replace("<", "&lt;")
        Cosa2 = Cosa2.Replace("€", "&euro;")
        Cosa2 = Cosa2.Replace("á", "&aacute;")
        Cosa2 = Cosa2.Replace("Á", "&Aacute;")
        Cosa2 = Cosa2.Replace("à", "&agrave;")
        Cosa2 = Cosa2.Replace("À", "&Agrave;")
        Cosa2 = Cosa2.Replace("ç", "&ccedil;")
        Cosa2 = Cosa2.Replace("Ç", "&Ccedil;")
        Cosa2 = Cosa2.Replace("é", "&eacute;")
        Cosa2 = Cosa2.Replace("É", "&Eacute;")
        Cosa2 = Cosa2.Replace("è", "&egrave;")
        Cosa2 = Cosa2.Replace("È", "&Egrave;")
        Cosa2 = Cosa2.Replace("í", "&iacute;")
        Cosa2 = Cosa2.Replace("Í", "&Iacute;")
        Cosa2 = Cosa2.Replace("ì", "&igrave;")
        Cosa2 = Cosa2.Replace("Ì", "&Igrave;")
        Cosa2 = Cosa2.Replace("ó", "&oacute;")
        Cosa2 = Cosa2.Replace("Ó", "&Oacute;")
        Cosa2 = Cosa2.Replace("ò", "&ograve;")
        Cosa2 = Cosa2.Replace("Ò", "&Ograve;")
        Cosa2 = Cosa2.Replace("ú", "&uacute;")
        Cosa2 = Cosa2.Replace("Ú", "&Uacute;")
        Cosa2 = Cosa2.Replace("ù", "&ugrave;")
        Cosa2 = Cosa2.Replace("Ù", "&Ugrave;")

        Return Cosa2
    End Function
End Module
