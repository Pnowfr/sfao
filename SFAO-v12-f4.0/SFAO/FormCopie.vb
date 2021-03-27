Public Class FormCopie
    Private Sub FormCopie_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = 80
        ProgressBar1.Value = 70
    End Sub

End Class