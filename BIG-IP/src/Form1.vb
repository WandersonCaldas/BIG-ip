Imports Microsoft.VisualBasic.FileIO
Imports System.Text
Imports System.IO

Public Class Form1

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Application.Exit()
    End Sub

    Private Sub Gerar_Click(sender As Object, e As EventArgs) Handles Gerar.Click
        Dim txtArquivoCSV As String = TextBox1.Text
        Dim txtArquivoDestino As String = TextBox2.Text

        'verifica se o nome do arquivo foi informado
        Dim caminho As String = ""
        If Not String.IsNullOrEmpty(txtArquivoDestino) Then
            caminho = txtArquivoDestino
        Else
            MessageBox.Show("Informe o nome do arquivo texto ", "Nome do arquivo", MessageBoxButtons.OK, MessageBoxIcon.Information)
            TextBox2.Focus()
            Return
        End If

        Using leitorArquivoCSV As New TextFieldParser(txtArquivoCSV, Encoding.Default)
            'define o tipo de arquivo como delimitado
            leitorArquivoCSV.TextFieldType = FileIO.FieldType.Delimited

            'define o delimitador usado no arquivo
            leitorArquivoCSV.SetDelimiters(";")

            'Define o caractere que indica que a linha é um comentário
            'leitorArquivoCSV.CommentTokens = New String() {"#"}

            'Informa que existe um campo que esta envolto em aspas duplas (")
            'leitorArquivoCSV.HasFieldsEnclosedInQuotes = True

            'define um array de string
            Dim linhaAtual As String()

            'pula a primeira linha do arquivo 
            leitorArquivoCSV.ReadLine()

            ' Cria um arquivo para escrita
            Using sw As FileStream = File.Create(caminho)
                
            End Using

            'inicia o laço para iterar no arquivo texto
            While Not leitorArquivoCSV.EndOfData
                'ler uma linha do arquivo
                linhaAtual = leitorArquivoCSV.ReadFields()

                'MessageBox.Show(linhaAtual(0) & ";" & linhaAtual(1) & ";" & linhaAtual(2) & ";" & linhaAtual(3) & ";" & linhaAtual(4) & ";" & linhaAtual(5) & ";" & linhaAtual(6))

                'verifica se o arquivo existe
                If File.Exists(caminho) Then
                    Dim nd = linhaAtual(3).Split("/")
                    Dim nd_exibir As String = nd(UBound(nd))
                    Dim a_mib = linhaAtual(4).Split(".")
                    Dim mib As String = a_mib(UBound(a_mib))

                    Using sw As StreamWriter = New StreamWriter(caminho, True)
                        sw.WriteLine()
                        sw.WriteLine("define service{")
                        sw.WriteLine("        use                             linux-rem-srv")
                        sw.WriteLine("        host_name                       " & linhaAtual(1).Trim & "")
                        sw.WriteLine("        contact_groups                  geope_produ,geope_infra_telecom,geope_infra_webapp_n1")
                        sw.WriteLine("        service_description             Check Pool " & linhaAtual(2).Replace("/Common/", "").Trim & " Member " & nd_exibir.ToString.Trim & ":" & mib.Trim)
                        sw.WriteLine("        notes                           Monitoramento de checagem de status do Pool Member " & nd_exibir.ToString.Trim & ":" & mib.Trim & " - " & linhaAtual(5).Trim)
                        sw.WriteLine("        check_command                   check_pool_bigip!192.168.104.121!@cabalbrasil!" & linhaAtual(4).Trim)
                        sw.WriteLine("}")
                        sw.WriteLine()
                    End Using
                Else
                    MessageBox.Show("Arquivo não encontrado ", "Nome do arquivo", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Application.Exit()
                End If
            End While

            MessageBox.Show("ROTINA ENCERRADA")
            Application.Exit()
        End Using
    End Sub


End Class
