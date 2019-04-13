Imports System.IO

Public Class PuliziaPregresso
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If idUtente = -1 Or idUtente = 0 Or Utenza = "" Then
            Response.Redirect("Default.aspx")
            Exit Sub
        End If

        If Page.IsPostBack = False Then
            CaricaMesi()
        End If
    End Sub

    Private Sub CaricaMesi()
        Dim Directories() As String
        Dim d As DirectoryInfo
        Dim Percorso As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni"
        Dim Mesi() As String
        Dim QuantiMesi As Integer = 0
        Dim AppoMesi() As String

        Directories = Directory.GetDirectories(Percorso)
        For Each Dir As String In Directories
            d = New DirectoryInfo(Dir)
            AppoMesi = LeggeMesi(Percorso, d.Name)
            For i As Integer = 1 To UBound(AppoMesi)
                QuantiMesi += 1
                ReDim Preserve Mesi(QuantiMesi)
                Mesi(QuantiMesi) = AppoMesi(i) & " " & d.Name
            Next
        Next

        Dim dMesi As New DataColumn("Mese")
        Dim rigaR As DataRow
        Dim dttTabella As New DataTable()

        dttTabella = New DataTable
        dttTabella.Columns.Add(dMesi)

        For i As Integer = 1 To UBound(Mesi)
            rigaR = dttTabella.NewRow()
            rigaR(0) = Mesi(i)
            dttTabella.Rows.Add(rigaR)
        Next

        grdMesi.DataSource = dttTabella
        grdMesi.DataBind()
        grdMesi.SelectedIndex = -1
    End Sub

    Private Function LeggeMesi(Percorso As String, sDirectory As String) As String()
        Dim Directories() As String
        Dim d As DirectoryInfo
        Dim Mesi() As String
        Dim QuantiMesi As Integer = 0
        Dim NomeMesi() As String = {"GENNAIO", "FEBBRAIO", "MARZO", "APRILE", "MAGGIO", "GIUGNO", "LUGLIO", "AGOSTO", "SETTEMBRE", "OTTOBRE", "NOVEMBRE", "DICEMBRE"}
        Dim NumMese As Integer

        Directories = Directory.GetDirectories(Percorso & "\" & sDirectory)
        For Each Dir As String In Directories
            d = New DirectoryInfo(Dir)
            QuantiMesi += 1
            ReDim Preserve Mesi(QuantiMesi)
            NumMese = -1
            For i As Integer = 0 To UBound(NomeMesi)
                If NomeMesi(i) = d.Name.Trim.ToUpper Then
                    NumMese = i + 1
                    Exit For
                End If
            Next
            If NumMese > -1 Then
                Mesi(QuantiMesi) = Format(NumMese, "00") & "-" & d.Name
            Else
                Mesi(QuantiMesi) = "00-" & d.Name
            End If
        Next

        Return Mesi
    End Function

    Protected Sub imgAnnulla_Click(sender As Object, e As ImageClickEventArgs) Handles imgAnnulla.Click
        Response.Redirect("Principale.aspx?Giorno=" & Request.QueryString("Giorno") & "&Mese=" & Request.QueryString("Mese") & "&Anno=" & Request.QueryString("Anno"))
    End Sub

    Protected Sub imgUscita_Click(sender As Object, e As ImageClickEventArgs) Handles imgUscita.Click
        idUtente = -1
        Utenza = ""
        Response.Redirect("Default.aspx")
    End Sub

    Private Sub grdMesi_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdMesi.PageIndexChanging
        grdMesi.PageIndex = e.NewPageIndex
        grdMesi.DataBind()

        CaricaMesi()
    End Sub

    Protected Sub imgElimina_Click(sender As Object, e As ImageClickEventArgs) Handles imgElimina.Click
        Dim Checketto As CheckBox
        Dim Percorso As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni"
        Dim Mese As String
        Dim Anno As String

        For i As Integer = 0 To grdMesi.Rows.Count - 1
            Checketto = grdMesi.Rows(i).FindControl("chkScelto")
            If Checketto.Checked = True Then
                Mese = grdMesi.Rows(i).Cells(0).Text
                Anno = Mid(Mese, InStr(Mese, " ") + 1, Mese.Length).Trim
                Mese = Mid(Mese, 1, InStr(Mese, " ") - 1).Trim
                Mese = Mid(Mese, InStr(Mese, "-") + 1, Mese.Length).Trim
                Mese = Percorso & "\" & Anno & "\" & Mese

                EliminaMese(Mese)

                Try
                    RmDir(Percorso & "\" & Anno)
                Catch ex As Exception

                End Try
            End If
        Next

        CaricaMesi()
    End Sub

    Private Sub EliminaMese(Mese As String)
        Dim Files() As String
        Dim f As FileInfo

        Files = Directory.GetFiles(Mese)
        For Each sFile As String In Files
            f = New FileInfo(sFile)

            Kill(Mese & "\" & f.Name)
        Next

        RmDir(Mese)
    End Sub
End Class