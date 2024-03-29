'
' DotNetNukeŽ - http://www.dotnetnuke.com
' Copyright (c) 2002-2005
' by Perpetual Motion Interactive Systems Inc. ( http://www.perpetualmotion.ca )
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
'
Imports System.Web.UI
Imports System.Collections.Generic
Imports System.Reflection
Imports DotNetNuke
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization

''' -----------------------------------------------------------------------------
''' <summary>
''' The ViewModuleName1 class displays the content
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' </history>
''' -----------------------------------------------------------------------------
''' 

Partial Class ViewModuleName1
    Inherits Entities.Modules.PortalModuleBase
    Implements Entities.Modules.IActionable

#Region "Event Handlers"

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Page_Load runs when the control is loaded
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As EventArgs) Handles MyBase.Load
        Try
            'TabId
            'TabModuleId
            'ModuleId

            ucDependencyUC1.ParentControl = String.Format("Sample.{0}{1}{2}{3}", ID, TabId, TabModuleId, ModuleId) 'Must be Unique
            ucDependencyUC1.RelatedControls = New List(Of String)
            ucDependencyUC1.RelatedControls.Add("ucDependencyUC2")

            ucDependencyUC2.ParentControl = String.Format("Sample.{0}{1}{2}{3}", ID, TabId, TabModuleId, ModuleId)
            ucDependencyUC2.RelatedControls = New List(Of String)
            ucDependencyUC2.RelatedControls.Add("ucDependencyUC1")
        Catch exc As Exception        'Module failed to load
            ProcessModuleLoadException(Me, exc)
        End Try
    End Sub

#End Region

#Region "Optional Interfaces"

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Registers the module actions required for interfacing with the portal framework
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public ReadOnly Property ModuleActions() As Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
        Get
            Dim Actions As New Entities.Modules.Actions.ModuleActionCollection
            Actions.Add(GetNextActionID, Localization.GetString(Entities.Modules.Actions.ModuleActionType.AddContent, LocalResourceFile), Entities.Modules.Actions.ModuleActionType.AddContent, "", "", EditUrl(), False, Security.SecurityAccessLevel.Edit, True, False)
            Return Actions
        End Get
    End Property

#End Region

End Class