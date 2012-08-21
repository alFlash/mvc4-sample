Imports AMC.DNN.Modules.CertRecert.Data.Exception
Imports TIMSS.API.CustomerInfo
Imports TIMSS.API.Core.Validation
Imports TIMSS.API.User.UserDefinedInfo

Partial Class PersonifyDataProvider
    Inherits DataProvider

#Region "Supervisors"

    Public Function InsertSupervisor (ByVal customerInfo As ICustomer, ByVal supervisor As ICustomer,
                                      ByVal relatedMasterCustomerId As String,
                                      ByVal relatedSubCustomerId As Integer) As IIssuesCollection
        Try
            Dim newSupervisor = customerInfo.Relationships.AddNew()
            SynchronizeObject (supervisor, newSupervisor.Customer, "FirstName", "MiddleName", "LastName",
                               "PrimaryJobTitle", "OrganizationId", "OrganizationUnitId")
            newSupervisor.ReciprocalCode = newSupervisor.ReciprocalCode.List ("RECRUITER").ToCodeObject()
            newSupervisor.RelationshipCode = newSupervisor.RelationshipCode.List ("RECRUITED").ToCodeObject()
            newSupervisor.RelationshipType = newSupervisor.RelationshipType.List ("INFLUENCE").ToCodeObject()

            newSupervisor.MasterCustomerId = relatedMasterCustomerId
            newSupervisor.SubCustomerId = relatedSubCustomerId
            'newSupervisor.RelatedMasterCustomerId = relatedMasterCustomerId
            'newSupervisor.RelatedSubCustomerId = relatedSubCustomerId

            newSupervisor.Customer.IsNewObjectFlag = True
            newSupervisor.IsNewObjectFlag = True
            customerInfo.Relationships.Add (newSupervisor)
            Dim issues = customerInfo.Relationships.Validate()
            If (issues) Then
                customerInfo.Relationships.Save()
            End If
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
        Return customerInfo.Relationships.ValidationIssues
    End Function

#End Region

#Region "Supervisors"

    Public Function UpdateSupervisor (ByVal customerInfo As ICustomer, ByVal supervisor As ICustomer) _
        As IIssuesCollection

        Try
            Dim newSupervisorObj = customerInfo.Relationships.FindObject("Guid", supervisor.Guid)
            If newSupervisorObj IsNot Nothing Then
                Dim newSupervisor = CType(newSupervisorObj, ICustomerRelationship)
                SynchronizeObject(supervisor, newSupervisor.Customer, "FirstName", "MiddleName", "LastName",
                                   "PrimaryJobTitle")
                newSupervisor.Customer.IsNewObjectFlag = True
                newSupervisor.IsNewObjectFlag = True
                customerInfo.Relationships.Add(newSupervisor)
                Dim issues = customerInfo.Relationships.Validate()
                If (Not issues) Then
                    customerInfo.Relationships.Save()
                End If
                Return customerInfo.Relationships.ValidationIssues
            End If
        Catch ex As System.Exception
            DataAccessExceptionHandler.HandleException(ex)
        End Try
       
        Return Nothing
    End Function

    

#End Region
End Class
