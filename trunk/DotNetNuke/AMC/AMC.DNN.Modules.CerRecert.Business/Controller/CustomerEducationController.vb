Imports TIMSS.API.CustomerInfo
Imports TIMSS.API.Core.Validation

Namespace Controller
    Partial Class AmcCertRecertController

#Region "Customer Education"

        Public Function GetEducationList (ByVal masterCustomerId As String,
                                          ByVal subCustomerId As Integer) As ICustomerEducationList
            Dim listObject As ICustomerEducationList = Nothing

            listObject = _personifyDataProvider.GetEducationList (masterCustomerId, subCustomerId)

            Return listObject
        End Function

        Public Function GetCustomerEducationByGuiId(ByVal guid As String,
                                                     ByVal masterCustomerId As String,
                                                     ByVal subCustomerId As Integer) As ICustomerEducation
            Dim objectItem As ICustomerEducation

            objectItem = _personifyDataProvider.GetCustomerEducationByGuiID(guid, masterCustomerId,
                                                                             subCustomerId)

            Return objectItem
        End Function
        Public Sub RefreshCustomerEducation(ByVal masterCustomerId As String,
                                             ByVal subCustomerId As Integer)
            _personifyDataProvider.RefreshCustomerEducation(masterCustomerId, subCustomerId)
        End Sub
        Public Function UpdateCustomerEducation (ByVal customerExternalCEActivitytItem As ICustomerEducation) _
            As IIssuesCollection
            Dim issuesObject As IIssuesCollection = Nothing
            customerExternalCEActivitytItem.Validate()
            issuesObject = customerExternalCEActivitytItem.ValidationIssues
            If issuesObject IsNot Nothing Then
                If issuesObject.Count > 0 Then
                    Return issuesObject
                End If
            End If
            '' Check Validation logic to calculate time in between courses, If there is a 2 months gap between Undergrad and Masters, the comments field is required to be filled 
            'Dim flagDay = True
            'Dim customerEducationList As ICustomerEducationList
            'customerEducationList = GetEducationList(customerExternalCEActivitytItem.MasterCustomerId,
            '                                      customerExternalCEActivitytItem.SubCustomerId)
            'For Each customerEducationItemtemp As ICustomerEducation In customerEducationList
            '    If customerEducationItemtemp.Guid <> customerExternalCEActivitytItem.Guid Then '' check with out customerExternalCEActivitytItem
            '        If Not AsGetDayNumberOfTwoDay(customerEducationItemtemp.EndDate, customerExternalCEActivitytItem.BeginDate) Then
            '            If customerExternalCEActivitytItem.Comments Is String.Empty Then
            '                flagDay = False
            '            End If
            '        End If
            '    End If
            'Next
            'If flagDay Then  ''
            '    issuesObject = _personifyDataProvider.UpdateCustomerEducation(customerExternalCEActivitytItem)
            'Else
            '    '' customerEducationList.ValidationIssues.Add(New IssueBase())
            '    ''raise IssuesCollection message
            'End If
            issuesObject = _personifyDataProvider.UpdateCustomerEducation (customerExternalCEActivitytItem)

            Return issuesObject
        End Function

        Public Function InsertCustomerEducation(ByRef customerExternalCEActivitytItem As ICustomerEducation) _
            As IIssuesCollection
            Dim issuesObject As IIssuesCollection = Nothing
            customerExternalCEActivitytItem.Validate()
            issuesObject = customerExternalCEActivitytItem.ValidationIssues
            If issuesObject IsNot Nothing Then
                If issuesObject.Count > 0 Then
                    Return issuesObject
                End If
            End If
            '' Check Validation logic to calculate time in between courses, If there is a 2 months gap between Undergrad and Masters, the comments field is required to be filled 
            'Dim flagDay = True
            'Dim customerEducationList As ICustomerEducationList
            'customerEducationList = GetEducationList(customerExternalCEActivitytItem.MasterCustomerId,
            '                                      customerExternalCEActivitytItem.SubCustomerId)
            'For Each customerEducationItemtemp As ICustomerEducation In customerEducationList
            '    If Not AsGetDayNumberOfTwoDay(customerEducationItemtemp.EndDate, customerExternalCEActivitytItem.BeginDate) Then
            '        If customerExternalCEActivitytItem.Comments Is String.Empty Then
            '            flagDay = False
            '        End If
            '    End If
            'Next
            'If flagDay Then  ''
            '    issuesObject = _personifyDataProvider.InsertCustomerEducation(customerExternalCEActivitytItem)
            'Else
            '    '' customerEducationList.ValidationIssues.Add(New IssueBase())
            '    ''raise IssuesCollection message
            'End If
            issuesObject = _personifyDataProvider.InsertCustomerEducation(customerExternalCEActivitytItem)

            Return issuesObject
        End Function

        Public Function CommitCustomerEducation(ByVal masterCustomerId As String,
                                                 ByVal subCustomerId As Integer) As IIssuesCollection
            Dim issuesObject As IIssuesCollection = Nothing

            issuesObject = _personifyDataProvider.CommitCustomerEducation(masterCustomerId, subCustomerId)

            Return issuesObject
        End Function

        Public Function DeleteCustomerEducation (ByVal customerExternalCEActivityItem As ICustomerEducation) _
            As IIssuesCollection
            Dim issuesObject As IIssuesCollection = Nothing
            If customerExternalCEActivityItem IsNot Nothing Then
                issuesObject = _personifyDataProvider.DeleteCustomerEducation(customerExternalCEActivityItem)
            End If
            Return issuesObject
        End Function

        Private Function AsGetDayNumberOfTwoDay (ByVal startDate As DateTime, ByVal endDate As DateTime) As Boolean
            Dim flag As Boolean = False

            Dim travelTime As TimeSpan = endDate - startDate
            If travelTime.Days > 60 Then
                flag = True
            End If

            Return flag
        End Function

#End Region
    End Class
End Namespace