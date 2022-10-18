Imports System.Threading
Imports System.IO
Public Class Form1

    Dim ruta As String = "D:\Hilo3\"

    Dim archivo As String = "Prueba2.txt"
    Dim hiloEscrituraArchivo As Thread

    Private synchronizationContext As SynchronizationContext

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        synchronizationContext = SynchronizationContext.Current
    End Sub

    Private Sub Escribir()
        Dim fs As FileStream
        Dim cantidad As UInteger = Integer.Parse(TextBox2.Text)
        Dim incremento As Double = 100 / cantidad
        Dim valor As Double = 0

        Try
            If File.Exists(ruta & archivo) Then

                fs = File.Create(ruta & archivo)
                fs.Close()

            Else

                Directory.CreateDirectory(ruta)
                fs = File.Create(ruta & archivo)
                fs.Close()

            End If
           Catch ex As Exception
            MsgBox("Error al crear el archivo")
        End Try
        Dim i As Integer


        Dim escribir As New StreamWriter(ruta & archivo)
        Try

            For i = 1 To cantidad
                escribir.WriteLine("lineas= " & i)
                 valor += incremento
                synchronizationContext.Send(New SendOrPostCallback(Sub()
                                                                       Label1.Text = ProgressBar1.Value & "%"
                                                                       ProgressBar1.Value = valor
                                                                       Label1.Refresh()
                                                                   End Sub), Nothing)
            Next

        Catch ex As Exception
            MsgBox("Se cancelo la escritura")
        Finally
            escribir.Dispose()
            synchronizationContext.Send(New SendOrPostCallback(Sub()
                                                                   Label1.Text = 0 & "%"
                                                                   ProgressBar1.Value = 0
                                                                   Label1.Refresh()
                                                               End Sub), Nothing)
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        hiloEscrituraArchivo = New Thread(AddressOf Escribir) With {.IsBackground = True}
        hiloEscrituraArchivo.Start()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If hiloEscrituraArchivo.IsAlive Then
            hiloEscrituraArchivo.Abort()
        End If
    End Sub

End Class
