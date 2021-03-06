﻿Imports Windows.Devices.Gpio
Public Class GpioPinButton

    Private ReadOnly m_testPin As New TestPin

    Public Sub New()

        InitPin(m_testPin)

    End Sub

    ''' <summary>
    ''' 接続されているPinのいずれかがHIGHになった1回目だけTrueを返す
    ''' </summary>
    ''' <returns>0:立ち上がっていない, 1:立ち上がった</returns>
    Public Function IsPushed() As Boolean

        If m_testPin.GpioPin.Read <> 1 Then
            m_testPin.PinState_pre = 0
            Return False
        End If

        If m_testPin.PinState_pre = 1 Then
            m_testPin.PinState_pre = 1
            Return False
        End If

        m_testPin.PinState_pre = 1
        Return True

    End Function

    ''' <summary>
    ''' Highになっているピンを返す。同時に押された場合は一番若いPinを返す。
    ''' </summary>
    ''' <returns>ピン番号</returns>
    Public Function GetPushedButton() As Integer

        Return m_testPin.GpioNum

    End Function

    ''' <summary>
    ''' ボタン用ピンのオープン、Inpit設定、デバウンスタイム設定
    ''' </summary>
    ''' <param name="pin">ピン</param>
    Private Sub InitPin(ByRef pin As ButtonPin)

        Dim gpio = GpioController.GetDefault()

        pin.GpioPin = gpio.OpenPin(pin.GpioNum)

        If pin.GpioPin.IsDriveModeSupported(GpioPinDriveMode.InputPullUp) Then
            pin.GpioPin.SetDriveMode(GpioPinDriveMode.InputPullUp)
        Else
            pin.GpioPin.SetDriveMode(GpioPinDriveMode.Input)
        End If

        pin.GpioPin.DebounceTimeout = TimeSpan.FromMilliseconds(20) '20 [msec] for debounce

    End Sub

    Private MustInherit Class ButtonPin

        Public Property GpioPin As GpioPin
        Public MustOverride Property GpioNum As Integer
        Public Property PinState_pre As Integer = 0

    End Class

    Private Class TestPin
        Inherits ButtonPin

        Public Overrides Property GpioNum As Integer = 17

    End Class

End Class
