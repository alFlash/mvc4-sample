Imports System
Imports System.Collections.Generic

Namespace Personify.ApplicationManager.PersonifyDataObjects
    Public Class CustomerOptInOptOutOptions
        ' Fields
        Public MasterCustomerId As String
        Public MyAvailableOptions As List(Of OptInOptOutOptions) = New List(Of OptInOptOutOptions)
        Public MyOptInCount As Integer
        Public MyOptInsOptOuts As List(Of OptInOptOutOptions) = New List(Of OptInOptOutOptions)
        Public MyOptOutCount As Integer
        Public SubCustomerId As Integer
    End Class
End Namespace

