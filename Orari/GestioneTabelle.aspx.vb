Public Class GestioneTabelle
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If idUtente = -1 Or idUtente=0 Or Utenza = "" Then
            Response.Redirect("Default.aspx")
            Exit Sub
        End If
    End Sub

    Protected Sub imgAnnulla_Click(sender As Object, e As ImageClickEventArgs) Handles imgAnnulla.Click
        Response.Redirect("Principale.aspx?Giorno=" & Request.QueryString("Giorno") & "&Mese=" & Request.QueryString("Mese") & "&Anno=" & Request.QueryString("Anno"))
    End Sub

    Protected Sub imgSocieta_Click(sender As Object, e As ImageClickEventArgs) Handles imgSocieta.Click
        Response.Redirect("GestioneSocieta.aspx?Maschera=GestTabelle&Giorno=" & Request.QueryString("Giorno") & "&Mese=" & Request.QueryString("Mese") & "&Anno=" & Request.QueryString("Anno"))
    End Sub

    Protected Sub imgCommesse_Click(sender As Object, e As ImageClickEventArgs) Handles imgCommesse.Click
        Response.Redirect("GestioneCommesse.aspx?Maschera=GestTabelle&Giorno=" & Request.QueryString("Giorno") & "&Mese=" & Request.QueryString("Mese") & "&Anno=" & Request.QueryString("Anno"))
    End Sub

    Protected Sub imgPasticche_Click(sender As Object, e As ImageClickEventArgs) Handles imgPasticche.Click
        Response.Redirect("GestionePasticche.aspx?Maschera=GestTabelle&Giorno=" & Request.QueryString("Giorno") & "&Mese=" & Request.QueryString("Mese") & "&Anno=" & Request.QueryString("Anno"))
    End Sub

    Protected Sub imgPortate_Click(sender As Object, e As ImageClickEventArgs) Handles imgPortate.Click
        Response.Redirect("GestionePortate.aspx?Maschera=GestTabelle&Giorno=" & Request.QueryString("Giorno") & "&Mese=" & Request.QueryString("Mese") & "&Anno=" & Request.QueryString("Anno"))
    End Sub

    Protected Sub imgMezziA_Click(sender As Object, e As ImageClickEventArgs) Handles imgMezziA.Click
        Response.Redirect("GestioneMezzi.aspx?Maschera=GestTabelle&Tipo=Andata&Giorno=" & Request.QueryString("Giorno") & "&Mese=" & Request.QueryString("Mese") & "&Anno=" & Request.QueryString("Anno"))
    End Sub

    Protected Sub imgMezziR_Click(sender As Object, e As ImageClickEventArgs) Handles imgMezziR.Click
        Response.Redirect("GestioneMezzi.aspx?Maschera=GestTabelle&Tipo=Ritorno&Giorno=" & Request.QueryString("Giorno") & "&Mese=" & Request.QueryString("Mese") & "&Anno=" & Request.QueryString("Anno"))
    End Sub

    Protected Sub imgTempo_Click(sender As Object, e As ImageClickEventArgs) Handles imgTempo.Click
        Response.Redirect("GestioneTempo.aspx?Maschera=GestTabelle&Giorno=" & Request.QueryString("Giorno") & "&Mese=" & Request.QueryString("Mese") & "&Anno=" & Request.QueryString("Anno"))
    End Sub

    Protected Sub imgAbitazioni_Click(sender As Object, e As ImageClickEventArgs) Handles imgAbitazioni.Click
        Response.Redirect("GestioneAbitazioni.aspx?Maschera=GestTabelle&Giorno=" & Request.QueryString("Giorno") & "&Mese=" & Request.QueryString("Mese") & "&Anno=" & Request.QueryString("Anno"))
    End Sub

    Protected Sub imgLinguaggi_Click(sender As Object, e As ImageClickEventArgs) Handles imgLinguaggi.Click
        Response.Redirect("GestioneLinguaggi.aspx?Maschera=GestTabelle&Giorno=" & Request.QueryString("Giorno") & "&Mese=" & Request.QueryString("Mese") & "&Anno=" & Request.QueryString("Anno"))
    End Sub

    Protected Sub imgStudi_Click(sender As Object, e As ImageClickEventArgs) Handles imgStudi.Click
        Response.Redirect("GestioneStudi.aspx?Maschera=GestTabelle&Giorno=" & Request.QueryString("Giorno") & "&Mese=" & Request.QueryString("Mese") & "&Anno=" & Request.QueryString("Anno"))
    End Sub

    Protected Sub imgUscita_Click(sender As Object, e As ImageClickEventArgs) Handles imgUscita.Click
        idUtente = -1
        Utenza = ""
        Response.Redirect("Default.aspx")
    End Sub
End Class