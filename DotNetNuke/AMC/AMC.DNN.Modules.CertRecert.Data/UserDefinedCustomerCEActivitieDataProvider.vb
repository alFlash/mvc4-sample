Imports TIMSS.API.User.ApplicationInfo
Imports TIMSS
Imports TIMSS.Enumerations
Imports TIMSS.API.User.UserDefinedInfo
Imports TIMSS.API.Core.Validation
Imports AMC.DNN.Modules.CertRecert.Data.Exception

Partial Class PersonifyDataProvider
    Inherits DataProvider

#Region "CE Activity"
    Public Function GetCETypesByProgramType(ByVal programType As String) As ApplicationCodes
        Try
            Dim results = GetObjectFromCache(String.Format("__CC_CETypes_By_ProgramType_{0}", programType))
            If results Is Nothing Then
                Dim applicationCodes = CType(TIMSS.Global.GetCollection(_organizationId, _organizationUnitId, NamespaceEnum.ApplicationInfo, "ApplicationCodes"), 
                        ApplicationCodes)
                With applicationCodes.Filter
                    .Add("Option1", programType)
                    .Add("Type", "USR_CUS_CE_CE_TYPE")
                    .Add("SubSystem", "USR")
                End With
                applicationCodes.Fill()
                StoreCacheObject(String.Format("__CC_CETypes_By_ProgramType_{0}", programType), applicationCodes)
                Return applicationCodes
            Else
                Return CType(results, ApplicationCodes)
            End If
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return Nothing
    End Function

    Public Function GetExternalActivityList(ByVal ceActivityType As String,
                                             ByVal masterCustomerId As String,
                                             ByVal subCustomerId As Integer) As IUserDefinedCustomerCEActivities
        Dim cEActivityExternals As IUserDefinedCustomerCEActivities
        Try
            cEActivityExternals = CType(GetObjectFromCache(_organizationId,
                                                             _organizationUnitId,
                                                             ceActivityType,
                                                             Me._certificationId.ToString(),
                                                             masterCustomerId,
                                                             subCustomerId), 
                                         IUserDefinedCustomerCEActivities)
            If cEActivityExternals Is Nothing Then
                cEActivityExternals = CType([Global].GetCollection(_organizationId,
                                                                     _organizationUnitId,
                                                                     NamespaceEnum.UserDefinedInfo,
                                                                     "UserDefinedCustomerCEActivities"), 
                                             IUserDefinedCustomerCEActivities)
                With cEActivityExternals.Filter
                    .Add("CertificationId", QueryOperatorEnum.Equals, Me._certificationId.ToString())
                    .Add("ProgramType", QueryOperatorEnum.Equals, ceActivityType)
                    .Add("MasterCustomerId", QueryOperatorEnum.Equals, masterCustomerId)
                    .Add("SubcustomerId", QueryOperatorEnum.Equals, subCustomerId.ToString())
                End With
                cEActivityExternals.Fill()
                StoreCacheObject(_organizationId, _organizationUnitId, ceActivityType,
                                  Me._certificationId.ToString(), masterCustomerId, subCustomerId, cEActivityExternals)
            End If
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return cEActivityExternals
    End Function

    Public Function GetCustomerExternalCEActivityByGuiId(ByVal guid As String,
                                                          ByVal type As String,
                                                          ByVal masterCustomerId As String,
                                                          ByVal subCustomerId As Integer) _
        As IUserDefinedCustomerCEActivity
        Dim customerExternalCEActivityReturn As IUserDefinedCustomerCEActivity
        Dim customerExternalCEActivityList As IUserDefinedCustomerCEActivities
        Try
            customerExternalCEActivityList = GetExternalActivityList(type, masterCustomerId, subCustomerId)
            customerExternalCEActivityReturn = CType(customerExternalCEActivityList.FindObject("Guid", guid), 
                                                      IUserDefinedCustomerCEActivity)

        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return customerExternalCEActivityReturn
    End Function

    Public Function UpdateCustomerExternalCEActivity(
                                                      ByRef customerExternalCEActivityItem As  _
                                                         IUserDefinedCustomerCEActivity
                                                      ) As IIssuesCollection
        ''To Do:
        Dim customerExternalCEList As IUserDefinedCustomerCEActivities
        Dim customerExternalCEActivityItemData As IUserDefinedCustomerCEActivity
        Try
            customerExternalCEList = GetExternalActivityList(customerExternalCEActivityItem.ProgramTypeString,
                                                              customerExternalCEActivityItem.MasterCustomerId,
                                                              customerExternalCEActivityItem.SubcustomerId)
            customerExternalCEActivityItemData =
                GetCustomerExternalCEActivityByGuiId(customerExternalCEActivityItem.Guid.ToString(),
                                                      customerExternalCEActivityItem.ProgramTypeString,
                                                      customerExternalCEActivityItem.MasterCustomerId,
                                                      customerExternalCEActivityItem.SubcustomerId)
            If customerExternalCEActivityItemData IsNot Nothing Then
                ''SynchronizeObject(customerExternalCEActivityItem, customerExternalCEActivityItemData)
                customerExternalCEList.Validate()
            End If
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return customerExternalCEActivityItemData.ValidationIssuesForMe
    End Function

    Public Function InsertCustomerExternalCEACtivity(ByRef customerExternalCEActivity As IUserDefinedCustomerCEActivity) _
        As IIssuesCollection
        Dim customerExternalCEActivityList As IUserDefinedCustomerCEActivities
        Dim customerExternalCEActivityItemData As IUserDefinedCustomerCEActivity
        Dim issueCollection As New IssuesCollection
        Try
            '' Get list cache object
            customerExternalCEActivityList = GetExternalActivityList(customerExternalCEActivity.ProgramTypeString,
                                                                      customerExternalCEActivity.MasterCustomerId,
                                                                      customerExternalCEActivity.SubcustomerId)
            customerExternalCEActivityItemData = customerExternalCEActivityList.CreateNew()
            ''set properties for object data from business object
            ''SynchronizeObject(customerExternalDocumentItem, customerExternalDocumentItemData)
            With customerExternalCEActivityItemData
                .CertificationId = Me._certificationId
                .CEHours = customerExternalCEActivity.CEHours
                .CEHoursAwarded = customerExternalCEActivity.CEHoursAwarded
                .ProgramType = customerExternalCEActivity.ProgramType
                If .CEType Is Nothing OrElse .CEType.List.Count < 1 Then
                    .CEType.FillList()
                End If
                .CEType = customerExternalCEActivity.CEType
                .Comments = customerExternalCEActivity.Comments
                .EndDate = customerExternalCEActivity.EndDate
                .OrganizationApproving = customerExternalCEActivity.OrganizationApproving
                .OrganizationProviding = customerExternalCEActivity.OrganizationProviding
                .Position = customerExternalCEActivity.Position
                .ProgramDate = customerExternalCEActivity.ProgramDate
                .ProgramTitle = customerExternalCEActivity.ProgramTitle
                .PublicationTitle = customerExternalCEActivity.PublicationTitle
                .PublicationType = customerExternalCEActivity.PublicationType
                .StartDate = customerExternalCEActivity.StartDate
                .Comments = customerExternalCEActivity.Comments
                .Audience = customerExternalCEActivity.Audience
                .IsNewObjectFlag = True
                .MasterCustomerId = customerExternalCEActivity.MasterCustomerId
                .SubcustomerId = customerExternalCEActivity.SubcustomerId
                .ArticlePage = customerExternalCEActivity.ArticlePage
                .Role = customerExternalCEActivity.Role
                .ProgramSponsor = customerExternalCEActivity.ProgramSponsor
                .Degree = customerExternalCEActivity.Degree
                ''.DegreeString = customerExternalCEActivity.DegreeString
            End With
            ''Add object
            customerExternalCEActivityList.Add(customerExternalCEActivityItemData)
            ''validate object
            customerExternalCEActivityList.Validate()
            For Each issue As IIssue In customerExternalCEActivityItemData.ValidationIssuesForMe
                issueCollection.Add(issue)
            Next
            ''remove Item error
            If issueCollection.Count >= 1 Then
                customerExternalCEActivityList.Remove(customerExternalCEActivityItemData)
            Else
                customerExternalCEActivity = customerExternalCEActivityItemData
            End If
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return issueCollection
    End Function

    Public Function CommitCustomerExternalCEActivity(ByVal type As String,
                                                      ByVal masterCustomerId As String,
                                                      ByVal subCustomerId As Integer) As IIssuesCollection
        Dim customerExternalCEActivityList As IUserDefinedCustomerCEActivities
        Try
            customerExternalCEActivityList = GetExternalActivityList(type, masterCustomerId, subCustomerId)
            customerExternalCEActivityList.Save()
            If (customerExternalCEActivityList.ValidationIssues.Count < 1) Then
                
            End If
       Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return customerExternalCEActivityList.ValidationIssues
    End Function

    Public Sub RefreshCustomerExternalActivity(ByVal type As String,
                                               ByVal masterCustomerId As String,
                                               ByVal subCustomerId As Integer)
        RemoveCacheObject(_organizationId, _organizationUnitId,
                                   type, Me._certificationId.ToString(),
                                   masterCustomerId, subCustomerId)
    End Sub


    Public Function DeleteCustomerExternalCEActivity(
                                                      ByVal customerExternalCEActivityItem As  _
                                                         IUserDefinedCustomerCEActivity
                                                      ) As IIssuesCollection
        Dim customerExternalCEActivityList As IUserDefinedCustomerCEActivities = Nothing
        Dim customerExternalCEActivityItemData As IUserDefinedCustomerCEActivity
        Try
            customerExternalCEActivityList = GetExternalActivityList(customerExternalCEActivityItem.ProgramTypeString,
                                                                      customerExternalCEActivityItem.MasterCustomerId,
                                                                      customerExternalCEActivityItem.SubcustomerId)
            customerExternalCEActivityItemData =
                CType(customerExternalCEActivityList.FindObject("Guid",
                                                                  customerExternalCEActivityItem.Guid.ToString()), 
                       IUserDefinedCustomerCEActivity)
            If customerExternalCEActivityItemData IsNot Nothing Then
                customerExternalCEActivityList.Remove(customerExternalCEActivityItemData)
            End If
            customerExternalCEActivityList.Validate()
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return customerExternalCEActivityList.ValidationIssues
    End Function

    Public Function GetCETotal(ByVal masterCustomerId As String, ByVal subCustomerId As Integer,
                                ByVal ceActivityType As String, ByVal certificationIdInput As Integer) As Decimal
        Dim cEActivityExternals As IUserDefinedCustomerCEActivities
        Dim totalResult As Decimal = 0
        Try
            cEActivityExternals = CType(GetObjectFromCache(_organizationId, _organizationUnitId,
                                                             ceActivityType, certificationIdInput.ToString(),
                                                             masterCustomerId, subCustomerId), 
                                         IUserDefinedCustomerCEActivities)
            If cEActivityExternals Is Nothing Then
                cEActivityExternals = CType([Global].GetCollection(_organizationId,
                                                                     _organizationUnitId,
                                                                     NamespaceEnum.UserDefinedInfo,
                                                                     "UserDefinedCustomerCEActivities"), 
                                             IUserDefinedCustomerCEActivities)
                With cEActivityExternals.Filter
                    .Add("CertificationId", QueryOperatorEnum.Equals, Me._certificationId.ToString())
                    .Add("ProgramType", QueryOperatorEnum.Equals, ceActivityType)
                    .Add("MasterCustomerId", QueryOperatorEnum.Equals, masterCustomerId)
                    .Add("SubcustomerId", QueryOperatorEnum.Equals, subCustomerId.ToString())
                End With
                cEActivityExternals.Fill()
            End If
            If cEActivityExternals IsNot Nothing AndAlso cEActivityExternals.Count > 0 Then
                For Each ceActivityItem As UserDefinedCustomerCEActivity In cEActivityExternals
                    totalResult += ceActivityItem.CEHours
                Next
            End If
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return totalResult
    End Function

#End Region


#Region "CE Activities"

    ''' <summary>
    ''' Gets the CE activities.
    ''' </summary>
    ''' <param name="ceType">Type of the ce.</param>
    ''' <param name="masterCustomerId">The master customer id.</param>
    ''' <param name="subCustomerId">The sub customer id.</param>
    ''' <returns></returns>
    Public Function GetCEActivities(ByVal ceType As String, ByVal masterCustomerId As String,
                                     ByVal subCustomerId As Integer) As IUserDefinedCustomerCEActivities
        Dim userDefinedCustomerActivities As IUserDefinedCustomerCEActivities = Nothing
        Try
            userDefinedCustomerActivities = CType(GetObjectFromCache(_organizationId, _organizationUnitId, ceType,
                                                                       Me._certificationId.ToString(), masterCustomerId,
                                                                       subCustomerId), 
                                                   IUserDefinedCustomerCEActivities)
            If (userDefinedCustomerActivities Is Nothing) Then
                userDefinedCustomerActivities = CType([Global].GetCollection(_organizationId, _organizationUnitId,
                                                                               NamespaceEnum.UserDefinedInfo,
                                                                               "UserDefinedCustomerCEActivities"), 
                                                       IUserDefinedCustomerCEActivities)
                With userDefinedCustomerActivities.Filter
                    .Add("CETypeString", QueryOperatorEnum.Equals, ceType)
                    .Add("RelatedMasterCustomerId", QueryOperatorEnum.Equals, masterCustomerId)
                    .Add("RelatedSubCustomerId", QueryOperatorEnum.Equals, subCustomerId.ToString())
                End With
                userDefinedCustomerActivities.Fill()
                StoreCacheObject(_organizationId, _organizationUnitId, ceType, Me._certificationId.ToString(),
                                  masterCustomerId, subCustomerId, userDefinedCustomerActivities)
            End If

        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return userDefinedCustomerActivities
    End Function

#End Region


#Region "Customer Services"

    Public Function CommitCEActivity(ByVal ceType As String, ByVal masterCustomerId As String,
                                      ByVal subCustomerId As Integer) As IIssuesCollection
        Dim userDefinedCEActivities As IUserDefinedCustomerCEActivities = Nothing
        Try
            userDefinedCEActivities.Save()
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return userDefinedCEActivities.ValidationIssues
    End Function

#End Region

#Region "CE Activity"

    Public Function InsertCEActivity(ByVal ceActivityItem As IUserDefinedCustomerCEActivity) As IIssuesCollection
        Dim userDefinedCEActivities As IUserDefinedCustomerCEActivities = Nothing
        Try
            ''userDefinedCEActivities = GetCEActivities(organizationId, organizationUnitId, ceActivityItem.CETypeString, ceActivityItem.MasterCustomerId, ceActivityItem.SubcustomerId)
            'erDefinedCEActivities.Add(ceActivityItem)

            'userDefinedCEActivities.Validate()
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return userDefinedCEActivities.ValidationIssues
    End Function

#End Region


    Public Function DeleteCEActivity(ByVal index As Integer, ByVal ceActivityItem As IUserDefinedCustomerCEActivity) _
        As IIssuesCollection
        Dim userDefinedCEActivities As IUserDefinedCustomerCEActivities = Nothing
        Return userDefinedCEActivities.ValidationIssues
    End Function

    Public Function UpdateCEActivity(ByVal index As Integer, ByVal ceActivityItem As IUserDefinedCustomerCEActivity) _
        As IIssuesCollection
        'TODO: 
        ''Get list cache by GetCEActivities function 
        ''Get object at index 
        ''Update object
        ''Return validate colection by calling Validatd function of list cache object
        Dim userDefinedCEActivities As IUserDefinedCustomerCEActivities = Nothing
        Dim ceItem = userDefinedCEActivities(index)
        With ceItem
            .CEActivityId = ceActivityItem.CEActivityId
        End With
        userDefinedCEActivities.Validate()
        Return userDefinedCEActivities.ValidationIssues
    End Function

End Class
