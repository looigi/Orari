﻿Imports System.IO

Public Class GestioneFilesDirectory
    Private DirectoryRilevate() As String
    Private FilesRilevati() As String
    Private QuantiFilesRilevati As Integer
    Private QuanteDirRilevate As Integer
    Private RootDir As String
    Private Eliminati As Boolean
    Private Barra As String = "\"
	Private DimensioniArrayAttualeDir As Long
	Private DimensioniArrayAttualeFiles As Long
	Private conta As Integer

	Public Sub PrendeRoot(R As String)
        RootDir = R
    End Sub

    Public Sub CreaAggiornaFileVisMese(NomeFile As String, Cosa As String)
        Dim path As String = NomeFile

        ' Create or overwrite the file.
        Dim fs As FileStream = File.Create(path)

        ' Add text to the file.
        Dim info As Byte() = New UTF8Encoding(True).GetBytes(Cosa)
        fs.Write(info, 0, info.Length)
        fs.Close()
    End Sub

    Public Function TornaNomeFileDaPath(Percorso As String) As String
        Dim Ritorno As String = ""

        For i As Integer = Percorso.Length To 1 Step -1
            If Mid(Percorso, i, 1) = "/" Or Mid(Percorso, i, 1) = barra Then
                Ritorno = Mid(Percorso, i + 1, Percorso.Length)
                Exit For
            End If
        Next

        Return Ritorno
    End Function

    Public Function TornaEstensioneFileDaPath(Percorso As String) As String
        Dim Ritorno As String = ""

        For i As Integer = Percorso.Length To 1 Step -1
            If Mid(Percorso, i, 1) = "." Then
                Ritorno = Mid(Percorso, i, Percorso.Length)
                Exit For
            End If
        Next
        If Ritorno.Length > 5 Then
            Ritorno = ""
        End If

        Return Ritorno
    End Function

    Public Function TornaNomeDirectoryDaPath(Percorso As String) As String
        Dim Ritorno As String = ""

        For i As Integer = Percorso.Length To 1 Step -1
            If Mid(Percorso, i, 1) = "/" Or Mid(Percorso, i, 1) = barra Then
                Ritorno = Mid(Percorso, 1, i - 1)
                Exit For
            End If
        Next

        Return Ritorno
    End Function

    Public Sub CreaAggiornaFile(NomeFile As String, Cosa As String)
        Try
            Dim sw As StreamWriter
            If (Not File.Exists(NomeFile)) Then
                sw = File.CreateText(NomeFile)
            Else
                sw = File.AppendText(NomeFile)
            End If
            sw.WriteLine(Cosa)
            sw.Close()
        Catch ex As Exception
            'Dim StringaPassaggio As String
            'Dim H As HttpApplication = HttpContext.Current.ApplicationInstance

            'StringaPassaggio = "?Errore=Errore CreaAggiornaFileVisMese: " & Err.Description.Replace(" ", "%20").Replace(vbCrLf, "")
            'StringaPassaggio = StringaPassaggio & "&Utente=" & H.Session("Nick")
            'StringaPassaggio = StringaPassaggio & "&Chiamante=" & H.Request.CurrentExecutionFilePath.ToUpper.Trim
            'H.Response.Redirect("Errore.aspx" & StringaPassaggio)
        End Try
    End Sub

    Public Function LeggeFile(NomeFile As String) As String
        Dim objReader As New StreamReader(NomeFile)
        Dim sLine As String = ""

        Do
            sLine= objReader.ReadLine()
        Loop Until sLine Is Nothing
        objReader.Close()

        Return sLine
    End Function

	Public Function RitornaFilesRilevati() As String()
		Return FilesRilevati
	End Function

	Public Function RitornaQuantiFilesRilevati() As Long
		Return QuantiFilesRilevati
	End Function

	Public Sub PulisceInfo()
		Erase FilesRilevati
		QuantiFilesRilevati = 0
		Erase DirectoryRilevate
		QuanteDirRilevate = 0

		DimensioniArrayAttualeDir = 10000
		DimensioniArrayAttualeFiles = 10000

		ReDim DirectoryRilevate(DimensioniArrayAttualeDir)
		ReDim FilesRilevati(DimensioniArrayAttualeFiles)
	End Sub

	Public Sub LeggeFilesDaDirectory(Percorso As String, Optional Filtro As String = "")
		If Directory.Exists(Percorso) Then
			Dim di As New IO.DirectoryInfo(Percorso)

			Dim fi As New IO.DirectoryInfo(Percorso)
			Dim fiar1 As IO.FileInfo() = di.GetFiles
			Dim fra As IO.FileInfo
			Dim Ok As Boolean = True
			Dim Filtri() As String = Filtro.Split(";")

			For Each fra In fiar1
				Ok = False
				If Filtro <> "" Then
					For i As Integer = 0 To Filtri.Length - 1
						If fra.FullName.ToUpper.IndexOf(Filtri(i).ToUpper.Trim.Replace("*", "")) > -1 Then
							Ok = True
							Exit For
						End If
					Next
				Else
					Ok = True
				End If
				If Ok = True Then
					QuantiFilesRilevati += 1
					If QuantiFilesRilevati > DimensioniArrayAttualeFiles Then
						DimensioniArrayAttualeFiles += 10000
						ReDim Preserve FilesRilevati(DimensioniArrayAttualeFiles)
					End If
					FilesRilevati(QuantiFilesRilevati) = fra.FullName
				End If
			Next
		End If
	End Sub

	Private Sub LeggeTutto(Percorso As String, Filtro As String, lblAggiornamento As Label, SoloRoot As Boolean)
		If Directory.Exists(Percorso) Then
			Dim di As New IO.DirectoryInfo(Percorso)
			Dim diar1 As IO.DirectoryInfo() = di.GetDirectories
			Dim dra As IO.DirectoryInfo

			For Each dra In diar1
				If lblAggiornamento Is Nothing = False Then
					Conta += 1
					If Conta = 2 Then
						Conta = 0
					End If
				End If

				QuanteDirRilevate += 1
				If QuanteDirRilevate > DimensioniArrayAttualeDir Then
					DimensioniArrayAttualeDir += 10000
					ReDim Preserve DirectoryRilevate(DimensioniArrayAttualeDir)
				End If
				DirectoryRilevate(QuanteDirRilevate) = dra.FullName

				LeggeFilesDaDirectory(dra.FullName, Filtro)

				If Not SoloRoot Then
					LeggeTutto(dra.FullName, Filtro, lblAggiornamento, SoloRoot)
				End If
			Next
		End If
	End Sub

	Public Sub ScansionaDirectorySingola(Percorso As String, Optional Filtro As String = "", Optional lblAggiornamento As Label = Nothing, Optional SoloRoot As Boolean = False)
		Eliminati = False

		PulisceInfo()

		QuanteDirRilevate += 1
		DirectoryRilevate(QuanteDirRilevate) = Percorso

		LeggeFilesDaDirectory(Percorso, Filtro)

		LeggeTutto(Percorso, Filtro, lblAggiornamento, SoloRoot)
	End Sub

	Public Sub ScansionaDirectoryVecchia(Percorso As String, Modalita As String)
		Dim di As New IO.DirectoryInfo(Percorso)
		Dim diar1 As IO.DirectoryInfo() = di.GetDirectories
		Dim dra As IO.DirectoryInfo

		Eliminati = False

		Erase FilesRilevati
		QuantiFilesRilevati = 0
		Erase DirectoryRilevate
		QuanteDirRilevate = 0

		For Each dra In diar1
			QuanteDirRilevate += 1
			ReDim Preserve DirectoryRilevate(QuanteDirRilevate)
			DirectoryRilevate(QuanteDirRilevate) = dra.FullName

			LeggeDirectory(dra.FullName)
		Next

		Select Case Modalita.ToUpper.Trim
			Case "ELIMINA"
				EliminaFilesRilevati()
		End Select

		Erase FilesRilevati
		Erase DirectoryRilevate
	End Sub

	Public Function RitornaEliminati() As Boolean
        Return Eliminati
    End Function

    Private Sub EliminaFilesRilevati()
        Dim AnnoAttuale As String = Now.Year.ToString.Trim
        Dim MeseAttuale As String = NomeMesi(Now.Month).Trim
        Dim Percorso As String = RootDir & "\App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & AnnoAttuale
        Dim PercorsoConMese As String = RootDir & "\App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & AnnoAttuale & "\" & MeseAttuale
        'Dim NonEliminare As String = "\" & AnnoAttuale & "\" & MeseAttuale & "\"

        For i As Integer = 1 To QuantiFilesRilevati
            If FilesRilevati(i).IndexOf(PercorsoConMese) = -1 Then
                Kill(FilesRilevati(i))
                Eliminati = True
            End If
        Next

        If QuanteDirRilevate > 0 Then
            Dim Ancora As Boolean = True

            For i As Integer = 1 To QuanteDirRilevate
                If DirectoryRilevate(i) = Percorso Or DirectoryRilevate(i) = PercorsoConMese Then
                    DirectoryRilevate(i) = "*"
                End If
            Next

            While Ancora = True
                Ancora = False
                For i As Integer = 1 To QuanteDirRilevate
                    If DirectoryRilevate(i) <> "*" Then
                        Try
                            RmDir(DirectoryRilevate(i))
                            DirectoryRilevate(i) = "*"
                            Eliminati = True
                        Catch ex As Exception
                            Ancora = True
                        End Try
                    End If
                Next
            End While
        End If
    End Sub

    Public Sub CreaDirectoryDaPercorso(Percorso As String)
        Dim Ritorno As String = Percorso

        For i As Integer = 1 To Ritorno.Length
            If Mid(Ritorno, i, 1) = "\" Or Mid(Ritorno, i, 1) = "/" Then
                On Error Resume Next
                MkDir(Mid(Ritorno, 1, i))
                On Error GoTo 0
            End If
        Next
    End Sub

    Private Sub LeggeDirectory(Percorso As String)
        Dim di As New IO.DirectoryInfo(Percorso)
        Dim diar1 As IO.DirectoryInfo() = di.GetDirectories
        Dim dra As IO.DirectoryInfo

        ' Lettura directory
        For Each dra In diar1
            QuanteDirRilevate += 1
            ReDim Preserve DirectoryRilevate(QuanteDirRilevate)
            DirectoryRilevate(QuanteDirRilevate) = dra.FullName

            LeggeDirectory(dra.FullName)
        Next

        Dim fi As New IO.DirectoryInfo(Percorso)
        Dim fiar1 As IO.FileInfo() = di.GetFiles
        Dim fra As IO.FileInfo

        For Each fra In fiar1
            QuantiFilesRilevati += 1
            ReDim Preserve FilesRilevati(QuantiFilesRilevati)
            FilesRilevati(QuantiFilesRilevati) = fra.FullName
        Next
    End Sub
End Class
