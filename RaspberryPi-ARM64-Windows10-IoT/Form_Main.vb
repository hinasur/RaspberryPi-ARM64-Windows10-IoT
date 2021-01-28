Public Class Form_Main

    Private ReadOnly m_gpioPinButton As New GpioPinButton
    Private ReadOnly m_socketManager As New SocketManager

    Public Sub New()

        ' この呼び出しはデザイナーで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後で初期化を追加します。

        Timer_Main.Start()

    End Sub

    Private Sub Timer_main_Tick(sender As Object, e As EventArgs) Handles Timer_Main.Tick

        If m_gpioPinButton.IsPushed() = True Then

            Dim pushedButtonNum As Integer = m_gpioPinButton.GetPushedButton()

            m_socketManager.SendMessage(pushedButtonNum)
            TextBox_Log.Text = "Pushed"

        Else

            TextBox_Log.Text = "NONE"

        End If

    End Sub
End Class
