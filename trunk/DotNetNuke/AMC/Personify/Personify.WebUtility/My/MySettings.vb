Imports System.CodeDom.Compiler
Imports System.ComponentModel
Imports System.Configuration
Imports System.Runtime.CompilerServices

Namespace My
    <GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "8.0.0.0"), CompilerGenerated, EditorBrowsable(EditorBrowsableState.Advanced)> _
    Friend NotInheritable Class MySettings
        Inherits ApplicationSettingsBase
        ' Properties
        Public Shared ReadOnly Property [Default] As MySettings
            Get
                Return MySettings.defaultInstance
            End Get
        End Property


        ' Fields
        Private Shared defaultInstance As MySettings = DirectCast(SettingsBase.Synchronized(New MySettings), MySettings)
    End Class
End Namespace

