Imports DotNetNuke.Services.Localization
Imports System

Namespace Personify.ApplicationManager
    Public Class LocalizedText
        ' Methods
        Public Shared Function GetLocalizedText(ByVal Name As String, ByVal Path As String) As String
            If (Not Localization.GetString(Name, Path) Is Nothing) Then
                Return Localization.GetString(Name, Path)
            End If
            If (Not Localization.GetString(Name, "~/App_GlobalResources/PersonifyResources.resx") Is Nothing) Then
                Return Localization.GetString(Name, "~/App_GlobalResources/PersonifyResources.resx")
            End If
            Return "Unknown error, please contact site administrator"
        End Function

    End Class
End Namespace

