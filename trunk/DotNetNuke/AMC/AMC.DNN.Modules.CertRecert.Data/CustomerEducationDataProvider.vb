Imports AMC.DNN.Modules.CertRecert.Data.Enums
Imports AMC.DNN.Modules.CertRecert.Data.Exception
Imports TIMSS.API.User.CustomerInfo
Imports TIMSS
Imports TIMSS.Enumerations
Imports TIMSS.API.CustomerInfo
Imports TIMSS.API.Core.Validation

Partial Class PersonifyDataProvider
    Inherits DataProvider



    Public Function GetEducationList(ByVal masterCustomerId As String,
                                      ByVal subCustomerId As Integer) As ICustomerEducationList
        Dim educationList As ICustomerEducationList
        Try
            educationList = CType(GetObjectFromCache(_organizationId,
                                                       _organizationUnitId,
                                                       CustomerObjectEnum.CUSEDUCATION.ToString(),
                                                       Me._certificationId.ToString(),
                                                       masterCustomerId,
                                                       subCustomerId), ICustomerEducationList)
            If educationList Is Nothing Then
                educationList = CType([Global].GetCollection(_organizationId, _organizationUnitId,
                                                               NamespaceEnum.CustomerInfo, "CustomerEducationList"), 
                                       ICustomerEducationList)
                With educationList.Filter
                    .Add("MasterCustomerId", QueryOperatorEnum.Equals, masterCustomerId)
                    .Add("SubcustomerId", QueryOperatorEnum.Equals, subCustomerId.ToString())
                End With
                educationList.Fill()
                StoreCacheObject(_organizationId, _organizationUnitId,
                                  CustomerObjectEnum.CUSEDUCATION.ToString(),
                                  Me._certificationId.ToString(),
                                  masterCustomerId, subCustomerId, educationList)
            End If
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return educationList
    End Function

    Public Function GetCustomerEducationByGuiID(ByVal guid As String,
                                                 ByVal masterCustomerId As String,
                                                 ByVal subCustomerId As Integer) As ICustomerEducation
        Dim customerEducationList As ICustomerEducationList
        Try
            customerEducationList = GetEducationList(masterCustomerId, subCustomerId)
            For Each customerEducation As CustomerEducation In customerEducationList
                If customerEducation.Guid = guid Then
                    Return customerEducation
                End If
            Next
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return Nothing
    End Function

    Public Function UpdateCustomerEducation(ByVal customerEducationItem As ICustomerEducation) As IIssuesCollection
        ''To Do:
        Dim customerEducationList As ICustomerEducationList
        Dim customerEducationItemData As ICustomerEducation
        Try
            customerEducationList = GetEducationList(customerEducationItem.MasterCustomerId,
                                                      customerEducationItem.SubCustomerId)
            customerEducationItemData = GetCustomerEducationByGuiID(customerEducationItem.Guid.ToString(),
                                                                     customerEducationItem.MasterCustomerId,
                                                                     customerEducationItem.SubCustomerId)
            If customerEducationItemData IsNot Nothing Then
                SynchronizeObject(customerEducationItem, customerEducationItemData)
                customerEducationList.Validate()
            End If
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return customerEducationList.ValidationIssues
    End Function

    Public Function InsertCustomerEducation(ByRef customerEducation As ICustomerEducation) As IIssuesCollection
        Dim customerEducationList As ICustomerEducationList
        Dim customerEducationItemData As ICustomerEducation
        Dim issueCollection As New IssuesCollection
        Try
            '' Get list cache object
            customerEducationList = GetEducationList(customerEducation.MasterCustomerId,
                                                      customerEducation.SubCustomerId)
            customerEducationItemData = customerEducationList.CreateNew()
            ''set properties for object data from business object
            With customerEducationItemData
                .BeginDate = customerEducation.BeginDate
                .Comments = customerEducation.Comments
                .EndDate = customerEducation.EndDate
                .Guid = customerEducation.Guid
                .Institution = customerEducation.Institution
                .InstitutionName = customerEducation.InstitutionName
                .ProgDegreeCode = customerEducation.ProgDegreeCode
                .ProgDegreeCodeString = customerEducation.ProgDegreeCodeString
                .ProgTypeCode = customerEducation.ProgTypeCode
                .ProgTypeCodeString = customerEducation.ProgTypeCodeString
                .IsNewObjectFlag = True
                .MasterCustomerId = customerEducation.MasterCustomerId
                .SubCustomerId = customerEducation.SubCustomerId
            End With
            customerEducationList.Add(customerEducationItemData)
            customerEducationList.Validate()
            '' have issue => remove item out of cache
            For Each issue As IIssue In customerEducationList.ValidationIssues
                issueCollection.Add(issue)
            Next
            If issueCollection.Count >= 1 Then
                customerEducationList.Remove(customerEducationItemData)
            Else
                customerEducation = customerEducationItemData
            End If
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return issueCollection
    End Function

    Public Function CommitCustomerEducation(ByVal masterCustomerId As String,
                                             ByVal subCustomerId As Integer) As IIssuesCollection
        Dim customerEducationList As ICustomerEducationList
        Try
            customerEducationList = GetEducationList(masterCustomerId, subCustomerId)
            customerEducationList.Save()
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return customerEducationList.ValidationIssues
    End Function
    Public Sub RefreshCustomerEducation(ByVal masterCustomerId As String,
                                             ByVal subCustomerId As Integer)
        RemoveCacheObject(_organizationId, _organizationUnitId,
                                    CustomerObjectEnum.CUSEDUCATION.ToString(), Me._certificationId.ToString(),
                                   masterCustomerId, subCustomerId)
    End Sub
    Public Function DeleteCustomerEducation(ByVal customerEducationItem As ICustomerEducation) As IIssuesCollection
        Dim customerEducationList As ICustomerEducationList
        Dim customerEducationItemData As ICustomerEducation
        Try
            customerEducationList = GetEducationList(customerEducationItem.MasterCustomerId,
                                                      customerEducationItem.SubCustomerId)
            customerEducationItemData = CType(
                customerEducationList.FindObject(
                    "Guid",
                    customerEducationItem.Guid.ToString()), 
                ICustomerEducation)

            For Each customerEducation As CustomerEducation In customerEducationList
                If customerEducation.Guid = customerEducationItem.Guid.ToString() Then
                    customerEducationItemData = customerEducation
                End If
            Next
            If customerEducationItemData IsNot Nothing Then
                customerEducationList.Remove(customerEducationItemData)
            End If
            customerEducationList.Validate()

        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return customerEducationList.ValidationIssues
    End Function




End Class
