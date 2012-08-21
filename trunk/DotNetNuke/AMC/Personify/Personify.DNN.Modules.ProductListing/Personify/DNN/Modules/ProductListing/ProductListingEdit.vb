Imports DotNetNuke.Common
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.UI.UserControls
Imports Microsoft.VisualBasic.CompilerServices
Imports Personify.ApplicationManager
Imports Personify.WebControls
Imports System
Imports System.Collections
Imports System.Diagnostics
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Web.UI.WebControls
Imports TIMSS.API
Imports TIMSS.API.ApplicationInfo
Imports TIMSS.Enumerations
Imports Microsoft.VisualBasic


Namespace Personify.DNN.Modules.ProductListing
    Public MustInherit Class ProductListingEdit
        Inherits PersonifyDNNBaseFormEditSettings
        ' Methods
        <DebuggerNonUserCode> _
        Public Sub New()
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.Page_Load)
            AddHandler MyBase.Init, New EventHandler(AddressOf Me.Page_Init)
        End Sub

        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try 
                Me.Response.Redirect(DotNetNuke.Common.Globals.NavigateURL, True)
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exc As Exception = exception1
                Exceptions.ProcessModuleLoadException(DirectCast(Me, PortalModuleBase), exc)
                ProjectData.ClearProjectError
            End Try
        End Sub

        Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try 
                Me.Response.Redirect(DotNetNuke.Common.Globals.NavigateURL, True)
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exc As Exception = exception1
                Exceptions.ProcessModuleLoadException(DirectCast(Me, PortalModuleBase), exc)
                ProjectData.ClearProjectError
            End Try
        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try 
                If Me.Page.IsValid Then
                    Dim controller As New ModuleController
                    controller.UpdateModuleSetting(Me.ModuleId, "OnDemandDataLoad", Me.chkEnableOnDemand.Checked.ToString)
                    controller.UpdateModuleSetting(Me.ModuleId, "Attributes", Me.DropDownList_Attributes.SelectedValue)
                    controller.UpdateModuleSetting(Me.ModuleId, "ProductIDs", Me.TextBoxBase_ProductIDs.Text)
                    controller.UpdateModuleSetting(Me.ModuleId, "Columns", Me.DropDownList_Columns.SelectedValue)
                    controller.UpdateModuleSetting(Me.ModuleId, "MainProduct", Me.DropDownList_MainProduct.SelectedValue)
                    controller.UpdateModuleSetting(Me.ModuleId, "MembersOnly", Me.DropDownList_MembersOnly.SelectedValue)
                    controller.UpdateModuleSetting(Me.ModuleId, "AddToCart", Me.DropDownList_AddToCart.SelectedValue)
                    controller.UpdateModuleSetting(Me.ModuleId, "AddToWishList", Me.DropDownList_AddToWishList.SelectedValue)
                    controller.UpdateModuleSetting(Me.ModuleId, "BuyForGroup", Me.DropDownList_BuyForGroup.SelectedValue)
                    controller.UpdateModuleSetting(Me.ModuleId, "DisplayImage", Me.DropDownList_DisplayImage.SelectedValue)
                    controller.UpdateModuleSetting(Me.ModuleId, "Layout", Me.DropDownList_Layout.SelectedValue)
                    controller.UpdateModuleSetting(Me.ModuleId, "ItemsPerPage", Me.DropDownList_DefaultPerPage.SelectedValue)
                    controller.UpdateModuleSetting(Me.ModuleId, "Sorting", Me.DropDownList_DefaultSorting.SelectedValue)
                    controller.UpdateModuleSetting(Me.ModuleId, "DefaultQuantity", Me.TextBox_DefaultQuantity.Text)
                    controller.UpdateModuleSetting(Me.ModuleId, "MaxProduct", Me.TextBoxBase_MaxProduct.Text)
                    controller.UpdateModuleSetting(Me.ModuleId, "Truncate", Me.TextBoxBase_TruncateDescription.Text)
                    controller.UpdateModuleSetting(Me.ModuleId, "DetailUrl", Me.Urlcontrol_DetailUrl.Url)
                    controller.UpdateModuleSetting(Me.ModuleId, "DetailUrlType", Me.Urlcontrol_DetailUrl.UrlType)
                    controller.UpdateModuleSetting(Me.ModuleId, "BuyForGroupUrl", Me.Urlcontrol_BuyForGroup.Url)
                    controller.UpdateModuleSetting(Me.ModuleId, "BuyForGroupUrlType", Me.Urlcontrol_BuyForGroup.UrlType)
                    controller.UpdateModuleSetting(Me.ModuleId, "Randomize", Convert.ToString(Me.Randomize_CheckBox.Checked))
                    Dim settingValue As String = ""
                    If (Me.ListBox_Subsystems.Items.Count > 0) Then
                        Dim num2 As Integer = (Me.ListBox_Subsystems.Items.Count - 1)
                        Dim i As Integer = 0
                        Do While (i <= num2)
                            If ((i = 0) AndAlso Me.ListBox_Subsystems.Items.Item(i).Selected) Then
                                Exit Do
                            End If
                            If Me.ListBox_Subsystems.Items.Item(i).Selected Then
                                If (settingValue.Length > 0) Then
                                    settingValue = (settingValue & "|")
                                End If
                                settingValue = (settingValue & Me.ListBox_Subsystems.Items.Item(i).Value)
                            End If
                            i += 1
                        Loop
                    End If
                    controller.UpdateModuleSetting(Me.ModuleId, "Subsystens", settingValue)
                    Me.Response.Redirect(DotNetNuke.Common.Globals.NavigateURL, True)
                End If
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exc As Exception = exception1
                Exceptions.ProcessModuleLoadException(DirectCast(Me, PortalModuleBase), exc)
                ProjectData.ClearProjectError
            End Try
        End Sub

        Private Function df_GetProductRelatedActiveSubsystemList() As IApplicationSubsystems
            Dim subSystems As IApplicationSubsystems = CachedApplicationData.ApplicationDataCache.SubSystems
            Dim subsystems2 As IApplicationSubsystems = DirectCast(Me.PersonifyGetCollection(NamespaceEnum.ApplicationInfo, "ApplicationSubsystems"), IApplicationSubsystems)
            Dim num2 As Integer = (subSystems.Count - 1)
            Dim i As Integer = 0
            Do While (i <= num2)
                If (subSystems.Item(i).ActiveFlag AndAlso subSystems.Item(i).ProductFlag) Then
                    subsystems2.Add(subSystems.Item(i))
                End If
                i += 1
            Loop
            Return subsystems2
        End Function

        Private Function GetTemplates() As ListItemCollection
            Dim items As ListItemCollection
            Try 
                Dim items2 As New ListItemCollection
                Dim info As New DirectoryInfo(Me.MapPathSecure((Me.ModulePath & "Templates")))
                Dim info2 As FileInfo
                For Each info2 In info.GetFiles("*.?s*")
                    Dim item As New ListItem
                    item.Text = info2.Name
                    item.Value = info2.Name
                    items2.Add(item)
                Next
                items = items2
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exception As Exception = exception1
                Throw exception
            End Try
            Return items
        End Function

        <DebuggerStepThrough> _
        Private Sub InitializeComponent()
        End Sub

        Private Sub LoadDropDownListSetting(ByVal aDropDownList As DropDownList, ByVal aSettingString As String, ByVal aSettingList As String)
            If (aDropDownList.Items.Count <= 0) Then
                Dim strArray As String() = Localization.GetString(aSettingList, Me.LocalResourceFile).Split(New Char() { "|"c })
                Dim str As String = Convert.ToString(RuntimeHelpers.GetObjectValue(Me.Settings.Item(aSettingString)))
                Dim num2 As Integer = (Convert.ToInt32(CDbl((CDbl(strArray.Length) / 2))) - 1)
                Dim i As Integer = 0
                Do While (i <= num2)
                    aDropDownList.Items.Add(New ListItem(strArray(((2 * i) + 1)), strArray((2 * i))))
                    If (aDropDownList.Items.Item((aDropDownList.Items.Count - 1)).Value = str) Then
                        aDropDownList.SelectedValue = str
                    End If
                    i += 1
                Loop
            End If
        End Sub

        Private Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
            Me.InitializeComponent
        End Sub

        Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
            Try 
                If Not Me.IsPostBack Then
                    Dim subsystems As IApplicationSubsystems = Me.df_GetProductRelatedActiveSubsystemList
                    If (Me.ListBox_Subsystems.Items.Count = 0) Then
                        Dim enumerator As IEnumerator
                        Me.ListBox_Subsystems.Items.Add(New ListItem("All", ""))
                        Try 
                            enumerator = subsystems.GetEnumerator
                            Do While enumerator.MoveNext
                                Dim current As IApplicationSubsystem = DirectCast(enumerator.Current, IApplicationSubsystem)
                                Me.ListBox_Subsystems.Items.Add(New ListItem(current.SubsystemName, current.Subsystem))
                            Loop
                        Finally
                            If TypeOf enumerator Is IDisposable Then
                                TryCast(enumerator,IDisposable).Dispose
                            End If
                        End Try
                        Dim str As String = Convert.ToString(RuntimeHelpers.GetObjectValue(Me.Settings.Item("Subsystens")))
                        If ((Not str Is Nothing) AndAlso (str.Length > 0)) Then
                            Dim strArray As String() = str.Split(New Char() { "|"c })
                            If ((Not strArray Is Nothing) AndAlso (strArray.Length > 0)) Then
                                Dim num3 As Integer = (strArray.Length - 1)
                                Dim i As Integer = 0
                                Do While (i <= num3)
                                    Dim index As Integer = Me.ListBox_Subsystems.Items.IndexOf(Me.ListBox_Subsystems.Items.FindByValue(strArray(i)))
                                    If (index >= 0) Then
                                        Me.ListBox_Subsystems.Items.Item(index).Selected = True
                                    End If
                                    i += 1
                                Loop
                            End If
                        End If
                        If (Me.ListBox_Subsystems.SelectedIndex < 1) Then
                            Me.ListBox_Subsystems.SelectedIndex = 0
                        End If
                    End If
                    Me.LoadDropDownListSetting(Me.DropDownList_Attributes, "Attributes", "Attributes.List")
                    If (Not Me.Settings.Item("DefaultQuantity") Is Nothing) Then
                        Me.TextBox_DefaultQuantity.Text = Conversions.ToString(Me.Settings.Item("DefaultQuantity"))
                    Else
                        Me.TextBox_DefaultQuantity.Text = "1"
                    End If
                    If (Not Me.Settings.Item("ProductIDs") Is Nothing) Then
                        Me.TextBoxBase_ProductIDs.Text = Convert.ToString(RuntimeHelpers.GetObjectValue(Me.Settings.Item("ProductIDs")))
                    End If
                    Me.LoadDropDownListSetting(Me.DropDownList_Columns, "Columns", "Columns.List")
                    Me.LoadDropDownListSetting(Me.DropDownList_MainProduct, "MainProduct", "YesNo.List")
                    Me.LoadDropDownListSetting(Me.DropDownList_MembersOnly, "MembersOnly", "YesNoBoth.List")
                    Me.LoadDropDownListSetting(Me.DropDownList_AddToCart, "AddToCart", "YesNo.List")
                    Me.LoadDropDownListSetting(Me.DropDownList_AddToWishList, "AddToWishList", "YesNo.List")
                    Me.LoadDropDownListSetting(Me.DropDownList_BuyForGroup, "BuyForGroup", "YesNo.List")
                    Me.LoadDropDownListSetting(Me.DropDownList_DisplayImage, "DisplayImage", "YesNo.List")
                    If (Me.DropDownList_Layout.Items.Count <= 0) Then
                        Dim enumerator2 As IEnumerator
                        Try 
                            enumerator2 = Me.GetTemplates.GetEnumerator
                            Do While enumerator2.MoveNext
                                Dim item As ListItem = DirectCast(enumerator2.Current, ListItem)
                                Me.DropDownList_Layout.Items.Add(item)
                            Loop
                        Finally
                            If TypeOf enumerator2 Is IDisposable Then
                                TryCast(enumerator2,IDisposable).Dispose
                            End If
                        End Try
                        Me.DropDownList_Layout.SelectedIndex = Me.DropDownList_Layout.Items.IndexOf(Me.DropDownList_Layout.Items.FindByValue(Convert.ToString(RuntimeHelpers.GetObjectValue(Me.Settings.Item("Layout")))))
                    End If
                    Me.LoadDropDownListSetting(Me.DropDownList_DefaultPerPage, "ItemsPerPage", "ItemsPerPage.List")
                    Me.LoadDropDownListSetting(Me.DropDownList_DefaultSorting, "Sorting", "Sorting.List")
                    If (Not Me.Settings.Item("MaxProduct") Is Nothing) Then
                        Me.TextBoxBase_MaxProduct.Text = Convert.ToString(RuntimeHelpers.GetObjectValue(Me.Settings.Item("MaxProduct")))
                    Else
                        Me.TextBoxBase_MaxProduct.Text = "0"
                    End If
                    If (Not Me.Settings.Item("Truncate") Is Nothing) Then
                        Me.TextBoxBase_TruncateDescription.Text = Convert.ToString(RuntimeHelpers.GetObjectValue(Me.Settings.Item("Truncate")))
                    Else
                        Me.TextBoxBase_TruncateDescription.Text = "0"
                    End If
                    If (Not Me.Settings.Item("Randomize") Is Nothing) Then
                        Me.Randomize_CheckBox.Checked = Convert.ToBoolean(RuntimeHelpers.GetObjectValue(Me.Settings.Item("Randomize")))
                    Else
                        Me.Randomize_CheckBox.Checked = False
                    End If
                    If ((Not Me.Settings.Item("DetailUrl") Is Nothing) AndAlso (Not Me.Settings.Item("DetailUrlType") Is Nothing)) Then
                        Me.Urlcontrol_DetailUrl.Url = Convert.ToString(RuntimeHelpers.GetObjectValue(Me.Settings.Item("DetailUrl")))
                        Me.Urlcontrol_DetailUrl.UrlType = Convert.ToString(RuntimeHelpers.GetObjectValue(Me.Settings.Item("DetailUrlType")))
                    End If
                    If ((Not Me.Settings.Item("BuyForGroupUrl") Is Nothing) AndAlso (Not Me.Settings.Item("BuyForGroupUrlType") Is Nothing)) Then
                        Me.Urlcontrol_BuyForGroup.Url = Convert.ToString(RuntimeHelpers.GetObjectValue(Me.Settings.Item("BuyForGroupUrl")))
                        Me.Urlcontrol_BuyForGroup.UrlType = Convert.ToString(RuntimeHelpers.GetObjectValue(Me.Settings.Item("BuyForGroupUrlType")))
                    End If
                End If
            Catch exception1 As Exception
                ProjectData.SetProjectError(exception1)
                Dim exc As Exception = exception1
                Exceptions.ProcessModuleLoadException(DirectCast(Me, PortalModuleBase), exc)
                ProjectData.ClearProjectError
            End Try
        End Sub


        ' Properties
        Protected Overridable Property cmdCancel As LinkButton
            <DebuggerNonUserCode> _
            Get
                Return Me._cmdCancel
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), DebuggerNonUserCode> _
            Set(ByVal WithEventsValue As LinkButton)
                If (Not Me._cmdCancel Is Nothing) Then
                    RemoveHandler Me._cmdCancel.Click, New EventHandler(AddressOf Me.cmdCancel_Click)
                End If
                Me._cmdCancel = WithEventsValue
                If (Not Me._cmdCancel Is Nothing) Then
                    AddHandler Me._cmdCancel.Click, New EventHandler(AddressOf Me.cmdCancel_Click)
                End If
            End Set
        End Property

        Protected Overridable Property cmdDelete As LinkButton
            <DebuggerNonUserCode> _
            Get
                Return Me._cmdDelete
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), DebuggerNonUserCode> _
            Set(ByVal WithEventsValue As LinkButton)
                If (Not Me._cmdDelete Is Nothing) Then
                    RemoveHandler Me._cmdDelete.Click, New EventHandler(AddressOf Me.cmdDelete_Click)
                End If
                Me._cmdDelete = WithEventsValue
                If (Not Me._cmdDelete Is Nothing) Then
                    AddHandler Me._cmdDelete.Click, New EventHandler(AddressOf Me.cmdDelete_Click)
                End If
            End Set
        End Property

        Protected Overridable Property cmdUpdate As LinkButton
            <DebuggerNonUserCode> _
            Get
                Return Me._cmdUpdate
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), DebuggerNonUserCode> _
            Set(ByVal WithEventsValue As LinkButton)
                If (Not Me._cmdUpdate Is Nothing) Then
                    RemoveHandler Me._cmdUpdate.Click, New EventHandler(AddressOf Me.cmdUpdate_Click)
                End If
                Me._cmdUpdate = WithEventsValue
                If (Not Me._cmdUpdate Is Nothing) Then
                    AddHandler Me._cmdUpdate.Click, New EventHandler(AddressOf Me.cmdUpdate_Click)
                End If
            End Set
        End Property

        Protected Overridable Property DropDownList_AddToCart As DropDownList
            <DebuggerNonUserCode> _
            Get
                Return Me._DropDownList_AddToCart
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), DebuggerNonUserCode> _
            Set(ByVal WithEventsValue As DropDownList)
                Me._DropDownList_AddToCart = WithEventsValue
            End Set
        End Property

        Protected Overridable Property DropDownList_AddToWishList As DropDownList
            <DebuggerNonUserCode> _
            Get
                Return Me._DropDownList_AddToWishList
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), DebuggerNonUserCode> _
            Set(ByVal WithEventsValue As DropDownList)
                Me._DropDownList_AddToWishList = WithEventsValue
            End Set
        End Property

        Protected Overridable Property DropDownList_Attributes As DropDownList
            <DebuggerNonUserCode> _
            Get
                Return Me._DropDownList_Attributes
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), DebuggerNonUserCode> _
            Set(ByVal WithEventsValue As DropDownList)
                Me._DropDownList_Attributes = WithEventsValue
            End Set
        End Property

        Protected Overridable Property DropDownList_BuyForGroup As DropDownList
            <DebuggerNonUserCode> _
            Get
                Return Me._DropDownList_BuyForGroup
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), DebuggerNonUserCode> _
            Set(ByVal WithEventsValue As DropDownList)
                Me._DropDownList_BuyForGroup = WithEventsValue
            End Set
        End Property

        Protected Overridable Property DropDownList_Columns As DropDownList
            <DebuggerNonUserCode> _
            Get
                Return Me._DropDownList_Columns
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), DebuggerNonUserCode> _
            Set(ByVal WithEventsValue As DropDownList)
                Me._DropDownList_Columns = WithEventsValue
            End Set
        End Property

        Protected Overridable Property DropDownList_DefaultPerPage As DropDownList
            <DebuggerNonUserCode> _
            Get
                Return Me._DropDownList_DefaultPerPage
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), DebuggerNonUserCode> _
            Set(ByVal WithEventsValue As DropDownList)
                Me._DropDownList_DefaultPerPage = WithEventsValue
            End Set
        End Property

        Protected Overridable Property DropDownList_DefaultSorting As DropDownList
            <DebuggerNonUserCode> _
            Get
                Return Me._DropDownList_DefaultSorting
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), DebuggerNonUserCode> _
            Set(ByVal WithEventsValue As DropDownList)
                Me._DropDownList_DefaultSorting = WithEventsValue
            End Set
        End Property

        Protected Overridable Property DropDownList_DisplayImage As DropDownList
            <DebuggerNonUserCode> _
            Get
                Return Me._DropDownList_DisplayImage
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), DebuggerNonUserCode> _
            Set(ByVal WithEventsValue As DropDownList)
                Me._DropDownList_DisplayImage = WithEventsValue
            End Set
        End Property

        Protected Overridable Property DropDownList_Layout As DropDownList
            <DebuggerNonUserCode> _
            Get
                Return Me._DropDownList_Layout
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), DebuggerNonUserCode> _
            Set(ByVal WithEventsValue As DropDownList)
                Me._DropDownList_Layout = WithEventsValue
            End Set
        End Property

        Protected Overridable Property DropDownList_MainProduct As DropDownList
            <DebuggerNonUserCode> _
            Get
                Return Me._DropDownList_MainProduct
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), DebuggerNonUserCode> _
            Set(ByVal WithEventsValue As DropDownList)
                Me._DropDownList_MainProduct = WithEventsValue
            End Set
        End Property

        Protected Overridable Property DropDownList_MembersOnly As DropDownList
            <DebuggerNonUserCode> _
            Get
                Return Me._DropDownList_MembersOnly
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), DebuggerNonUserCode> _
            Set(ByVal WithEventsValue As DropDownList)
                Me._DropDownList_MembersOnly = WithEventsValue
            End Set
        End Property

        Protected Overridable Property ListBox_Subsystems As ListBox
            <DebuggerNonUserCode> _
            Get
                Return Me._ListBox_Subsystems
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), DebuggerNonUserCode> _
            Set(ByVal WithEventsValue As ListBox)
                Me._ListBox_Subsystems = WithEventsValue
            End Set
        End Property

        Protected Overridable Property Randomize_CheckBox As CheckBox
            <DebuggerNonUserCode> _
            Get
                Return Me._Randomize_CheckBox
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), DebuggerNonUserCode> _
            Set(ByVal WithEventsValue As CheckBox)
                Me._Randomize_CheckBox = WithEventsValue
            End Set
        End Property

        Protected Overridable Property TextBox_DefaultQuantity As TextBox
            <DebuggerNonUserCode> _
            Get
                Return Me._TextBox_DefaultQuantity
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), DebuggerNonUserCode> _
            Set(ByVal WithEventsValue As TextBox)
                Me._TextBox_DefaultQuantity = WithEventsValue
            End Set
        End Property

        Protected Overridable Property TextBoxBase_MaxProduct As TextBoxBase
            <DebuggerNonUserCode> _
            Get
                Return Me._TextBoxBase_MaxProduct
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), DebuggerNonUserCode> _
            Set(ByVal WithEventsValue As TextBoxBase)
                Me._TextBoxBase_MaxProduct = WithEventsValue
            End Set
        End Property

        Protected Overridable Property TextBoxBase_ProductIDs As TextBoxBase
            <DebuggerNonUserCode> _
            Get
                Return Me._TextBoxBase_ProductIDs
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), DebuggerNonUserCode> _
            Set(ByVal WithEventsValue As TextBoxBase)
                Me._TextBoxBase_ProductIDs = WithEventsValue
            End Set
        End Property

        Protected Overridable Property TextBoxBase_TruncateDescription As TextBoxBase
            <DebuggerNonUserCode> _
            Get
                Return Me._TextBoxBase_TruncateDescription
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), DebuggerNonUserCode> _
            Set(ByVal WithEventsValue As TextBoxBase)
                Me._TextBoxBase_TruncateDescription = WithEventsValue
            End Set
        End Property

        Protected Overridable Property Urlcontrol_BuyForGroup As UrlControl
            <DebuggerNonUserCode> _
            Get
                Return Me._Urlcontrol_BuyForGroup
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), DebuggerNonUserCode> _
            Set(ByVal WithEventsValue As UrlControl)
                Me._Urlcontrol_BuyForGroup = WithEventsValue
            End Set
        End Property

        Protected Overridable Property Urlcontrol_DetailUrl As UrlControl
            <DebuggerNonUserCode> _
            Get
                Return Me._Urlcontrol_DetailUrl
            End Get
            <MethodImpl(MethodImplOptions.Synchronized), DebuggerNonUserCode> _
            Set(ByVal WithEventsValue As UrlControl)
                Me._Urlcontrol_DetailUrl = WithEventsValue
            End Set
        End Property


        ' Fields
        <AccessedThroughProperty("cmdCancel")> _
        Private _cmdCancel As LinkButton
        <AccessedThroughProperty("cmdDelete")> _
        Private _cmdDelete As LinkButton
        <AccessedThroughProperty("cmdUpdate")> _
        Private _cmdUpdate As LinkButton
        <AccessedThroughProperty("DropDownList_AddToCart")> _
        Private _DropDownList_AddToCart As DropDownList
        <AccessedThroughProperty("DropDownList_AddToWishList")> _
        Private _DropDownList_AddToWishList As DropDownList
        <AccessedThroughProperty("DropDownList_Attributes")> _
        Private _DropDownList_Attributes As DropDownList
        <AccessedThroughProperty("DropDownList_BuyForGroup")> _
        Private _DropDownList_BuyForGroup As DropDownList
        <AccessedThroughProperty("DropDownList_Columns")> _
        Private _DropDownList_Columns As DropDownList
        <AccessedThroughProperty("DropDownList_DefaultPerPage")> _
        Private _DropDownList_DefaultPerPage As DropDownList
        <AccessedThroughProperty("DropDownList_DefaultSorting")> _
        Private _DropDownList_DefaultSorting As DropDownList
        <AccessedThroughProperty("DropDownList_DisplayImage")> _
        Private _DropDownList_DisplayImage As DropDownList
        <AccessedThroughProperty("DropDownList_Layout")> _
        Private _DropDownList_Layout As DropDownList
        <AccessedThroughProperty("DropDownList_MainProduct")> _
        Private _DropDownList_MainProduct As DropDownList
        <AccessedThroughProperty("DropDownList_MembersOnly")> _
        Private _DropDownList_MembersOnly As DropDownList
        <AccessedThroughProperty("ListBox_Subsystems")> _
        Private _ListBox_Subsystems As ListBox
        <AccessedThroughProperty("Randomize_CheckBox")> _
        Private _Randomize_CheckBox As CheckBox
        <AccessedThroughProperty("TextBox_DefaultQuantity")> _
        Private _TextBox_DefaultQuantity As TextBox
        <AccessedThroughProperty("TextBoxBase_MaxProduct")> _
        Private _TextBoxBase_MaxProduct As TextBoxBase
        <AccessedThroughProperty("TextBoxBase_ProductIDs")> _
        Private _TextBoxBase_ProductIDs As TextBoxBase
        <AccessedThroughProperty("TextBoxBase_TruncateDescription")> _
        Private _TextBoxBase_TruncateDescription As TextBoxBase
        <AccessedThroughProperty("Urlcontrol_BuyForGroup")> _
        Private _Urlcontrol_BuyForGroup As UrlControl
        <AccessedThroughProperty("Urlcontrol_DetailUrl")> _
        Private _Urlcontrol_DetailUrl As UrlControl
        Private Const C_FILEPATTERN As String = "*.?s*"
        Private Const C_TEMPLATES As String = "Templates"
        Private designerPlaceholderDeclaration As Object
        Private itemId As Integer
    End Class
End Namespace

